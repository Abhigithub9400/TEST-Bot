using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace MediAssist.Application.Services
{
    public class UserSessionService : IUserSessionService
    {
        #region Private variables
        private readonly MediAssistDbContext _context;
        private readonly ILogger<UserSessionService> _logger;
        #endregion

        #region constructor
        public UserSessionService(MediAssistDbContext mediAssistDbContext, ILogger<UserSessionService> logger)
        {
            _context = mediAssistDbContext;
            _logger = logger;
        }
        #endregion

        #region Public Methods
        public async Task<IUserSessionDetails> StartOrResumeSession(IStartSessionDetails startSessionDetails)
        {
            try
            {
                UserSessionDetails result = new UserSessionDetails();

                if (!startSessionDetails.UserId.IsNullOrEmpty())
                {
                    long sessionDurationLimit = await _context.UserConfiguration
                                                     .Where(x => x.UserId == startSessionDetails.UserId)
                                                     .Select(x => x.SessionDurationLimit)
                                                     .FirstOrDefaultAsync();

                    sessionDurationLimit = Math.Max(sessionDurationLimit, 0);

                    if (startSessionDetails.SessionId > 0)
                    {
                        UserSession? userSession = await _context.UserSession
                                                                .Where(x => x.UserId == startSessionDetails.UserId && x.SessionId == startSessionDetails.SessionId)
                                                                .FirstOrDefaultAsync().ConfigureAwait(false);
                        if (userSession == null)
                        {
                            throw new ArgumentException("No data Found in userSession Table for userId {0}", startSessionDetails.UserId);                            
                        }

                        var usedTime = TimeSpan.FromMinutes(sessionDurationLimit) - userSession.SessionRemainingTime;

                        userSession.SessionStartTime = DateTime.Now - usedTime;
                        userSession.SessionVersion = userSession.SessionVersion + 1;
                        userSession.ModifiedDate   = DateTime.Now;
                        userSession.ModifiedBy     = startSessionDetails.UserId;
                        userSession.SessionRemainingTime = CalculateRemainingTime(userSession); 
                        userSession.TotalToken = startSessionDetails.TotalToken;
                        userSession.TotalCost = startSessionDetails.TotalCost;

                        await _context.SaveChangesAsync().ConfigureAwait(false);

                        result = createSessionResponse(userSession);
                        return result;
                    }
                    else
                    {
                        long maxSessionId = await _context.UserSession?
                                                     .Where(x => x.UserId == startSessionDetails.UserId)?
                                                     .MaxAsync(s => (long?)s.SessionId) ?? 0;

                        UserSession userSession = new UserSession()
                        {
                            UserId = startSessionDetails.UserId,
                            SessionId = maxSessionId + 1,
                            SessionVersion = 1,
                            FeaturePlanId = 1,
                            ReportGenerated = false,
                            SessionExpired = false,
                            SessionRemainingTime = TimeSpan.FromMinutes(sessionDurationLimit),
                            SessionStartTime = DateTime.Now,
                            CreatedBy = startSessionDetails.UserId,
                            CreatedDate = DateTime.Now,
                            ModifiedBy = startSessionDetails.UserId,
                            TotalToken = startSessionDetails.TotalToken,
                            TotalCost = startSessionDetails.TotalCost,
                            IsPotentialDiagnosisOn = startSessionDetails.IsPotentialDiagnosisOn

                        };

                        await _context.UserSession.AddAsync(userSession).ConfigureAwait(false);
                        await _context.SaveChangesAsync().ConfigureAwait(false);

                        result = createSessionResponse(userSession);

                        return result;
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                throw;
            }

        }
        public async Task<IUserSessionDetails> StopUserSession(int sessionId, string userId,int totalToken, decimal totalCost)
        {
            try
            {
                UserSessionDetails result = new UserSessionDetails();

                if (!userId.IsNullOrEmpty() && sessionId > 0)
                {
                    UserSession? userSession = await _context.UserSession
                                                       .Where(x => x.UserId == userId && x.SessionId == sessionId)
                                                       .FirstOrDefaultAsync();
                    if (userSession != null)
                    {

                        userSession.SessionExpired = true;
                        userSession.SessionRemainingTime = CalculateRemainingTime(userSession);
                        userSession.ModifiedBy = userId;
                        userSession.ModifiedDate = DateTime.Now;
                        userSession.TotalToken = totalToken;
                        userSession.TotalCost = totalCost;


                        await _context.SaveChangesAsync().ConfigureAwait(false);

                        UpdateUserConfigurations(userSession);

                        result = createSessionResponse(userSession);

                        return result;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                throw;
            }

        }

        public async Task<IUserSessionDetails> UpdateUserSession(int sessionId, string userId, int totalToken, decimal totalCost)
        {
            try
            {


                UserSession? userSession = await _context.UserSession
                                                        .Where(x => x.UserId == userId && x.SessionId == sessionId)
                                                        .FirstOrDefaultAsync().ConfigureAwait(false);

                if (userSession != null)
                {
                    TimeSpan remainigTime = CalculateRemainingTime(userSession);

                    userSession.SessionVersion = userSession.SessionVersion + 1;
                    if (remainigTime == TimeSpan.Zero)
                    {
                        userSession.SessionExpired = true;
                    }
                    userSession.SessionRemainingTime = remainigTime;
                    userSession.ModifiedDate  = DateTime.Now;
                    userSession.TotalToken = totalToken;
                    userSession.TotalCost = totalCost;

                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                UserSessionDetails result = createSessionResponse(userSession);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                throw;
            }

        }

        public async Task<IUserSessionDetails> ReportGenerated(int sessionId, string userId, int totalToken, decimal totalCost)
        {
            try
            {

                UserConfiguration userConfiguration = await _context.UserConfiguration.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                if (userConfiguration != null)
                {
                    userConfiguration.Transcriptions = userConfiguration.Transcriptions - 1;
                }

                UserSession userSession = new UserSession();
                if (sessionId > 0)
                {
                    userSession =  await _context.UserSession.Where(x => x.UserId == userId && x.SessionId == sessionId).FirstOrDefaultAsync();
                    if (userSession != null)
                    {
                        userSession.ReportGenerated = true;
                        userSession.TotalToken = totalToken;
                        userSession.TotalCost = totalCost;
                    }
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);

                UserSessionDetails result = createSessionResponse(userSession);

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                throw;
            }
        }
        #endregion


        #region Private Methods 
        private TimeSpan CalculateRemainingTime(UserSession userSession)
        {

            long sessionDurationLimit = _context.UserConfiguration
                                                       .Where(x => x.UserId == userSession.UserId).Select(x => x.SessionDurationLimit)
                                                       .FirstOrDefault();

            TimeSpan remainingTime = TimeSpan.FromMinutes(sessionDurationLimit) - (DateTime.Now - userSession.SessionStartTime);
            if (remainingTime < TimeSpan.Zero)
                remainingTime = TimeSpan.Zero;
            return remainingTime;
        }

        private UserSessionDetails createSessionResponse(UserSession? userSession)
        {
            UserSessionDetails userSessionDetails = new UserSessionDetails();

            if(userSession != null)
            {
                userSessionDetails.SessionId = userSession.SessionId;
                userSessionDetails.SessionExpired = userSession.SessionExpired;
                userSessionDetails.SessionVersion = userSession.SessionVersion;
                userSessionDetails.RemainingTime = userSession.SessionRemainingTime;
                userSessionDetails.StartedTime = userSession.SessionStartTime;
                userSessionDetails.UserId = userSession.UserId;
                userSessionDetails.ReportGenerated = userSession.ReportGenerated;
            }      
            
            return userSessionDetails;
        }

        private void UpdateUserConfigurations(UserSession userSession)
        {
            if (userSession != null)
            {

                UserConfiguration? userConfiguration =  _context.UserConfiguration.Where(x => x.UserId == userSession.UserId).FirstOrDefault();

                if (userConfiguration == null) {
                    throw new ArgumentException("userConfiguration is null for userid {0}", userSession.UserId);
                }

                var reamingTimes = _context.UserSession.Where(x => x.UserId == userSession.UserId).Select(x => x.SessionRemainingTime).ToList();

                long availableTime = 0;
                long totalusedtime = 0;


                long configuredSessionTime = TimeSpan.FromMinutes(userConfiguration.SessionDurationLimit).Ticks;

                foreach (var reamingTime in reamingTimes)
                {
                    var usedTime = configuredSessionTime - reamingTime.Ticks;
                    totalusedtime = totalusedtime + usedTime;
                }
                if (totalusedtime >= TimeSpan.FromMinutes(userConfiguration.AvailableHours).Ticks)
                {
                    availableTime = 0;
                }
                else
                {
                    availableTime = TimeSpan.FromMinutes(userConfiguration.AvailableHours).Ticks - totalusedtime;
                }
                /// Decrease available time only if it is less than or equal to the session duration limit.
                if (availableTime <= TimeSpan.FromMinutes(userConfiguration.SessionDurationLimit).Ticks)
                {
                    userConfiguration.SessionDurationLimit = availableTime / TimeSpan.TicksPerMinute;
                    userConfiguration.AvailableHours = availableTime / TimeSpan.TicksPerMinute;
                }

                _context.SaveChanges();

            }
        }
        #endregion

    }
}
