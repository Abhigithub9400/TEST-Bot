using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.Configurations;
using MediAssist.DbContext;
using MediAssist.Infrastructure.Abstract.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Services
{
    public class UserService : IUserService
    {
        #region PRIVATE FIELDS
        private readonly IUserRepository _userRepository;
        private readonly MediAssistDbContext _context;
        private const int MaxReportCount = 3;
        private const int ResetReportCount = 1;
        private const int InitialReportCount = 0;
        private readonly ILogger<UserService> _logger;

        #endregion

        #region CONSTRUCTOR
        public UserService(IUserRepository userRepository, MediAssistDbContext context,ILogger<UserService> logger)

        {
            _userRepository = userRepository;
            _context = context;
            _logger = logger;
        }
        #endregion

        #region PUBLIC METHODS

        public async Task<ILoginUserDetails> SignInWithEmailAndPasswordAsync(string email, string password)
        {
            try
            {


                var user = await _userRepository.GetUserByEmailAsync(email);
                var isSettingsUpdated = true;

                if (user == null)
                {
                    var notFoundResult = new LoginUserDetails()
                    {
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                    return (notFoundResult);
                }

                var userDetails = await _userRepository.GetUserDetailsbyIdAsync(user?.Id);

                var userTitle = await _userRepository.GetUserTitlebyIdAsync(userDetails?.Title);


                if (userDetails?.Signature == null || userDetails?.ClinicId == null)
                {
                    isSettingsUpdated = false;
                }
                string? imageBase64 = null;
                if (userDetails?.Image != null && userDetails?.Image?.Length > 0)
                {
                    imageBase64 = Convert.ToBase64String(userDetails.Image);
                }



                bool isPasswordValid = await _userRepository.VerifyPasswordAsync(user, password);
                if (!isPasswordValid)
                {
                    var unauthorizedResult = new LoginUserDetails()
                    {
                        HttpStatusCode = HttpStatusCode.Unauthorized
                    };
                    return (unauthorizedResult);
                }
                var result = new LoginUserDetails()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    FullName = user.FullName,
                    FirstName = user.FirstName,
                    UserId = user.Id,
                    Image = imageBase64,
                    TitleAbbreviation = userTitle?.Abbreviations,
                    Specialization = userDetails?.Specialization,
                    IsSettingsUpdated = isSettingsUpdated
                };
                return (result);
            }catch(Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                throw;
            }
        }

        public async Task<HttpStatusCode> SignUpWithEmailAndPasswordAsync(ISignUpUserDetails signUpUserDetails)
        {
            try
            {
                var (firstName, middleName, lastName) = SplitFullName(signUpUserDetails.FullName);
                var result = await _userRepository.CreateUserAsync(firstName, middleName, lastName, signUpUserDetails);

                if (result == null)
                {
                    return HttpStatusCode.NotFound;
                }
                else
                {
                    return HttpStatusCode.OK;
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                throw;
            }
        }
        public async Task<bool> CheckWhetherCurrentPasswordCorrectOrNot(string userId, string password)
        {
            try
            {
                var user = await _userRepository.GetUserByUserIdAsync(userId);
                bool isPasswordValid = await _userRepository.VerifyPasswordAsync(user, password);
                return isPasswordValid;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                throw;
            }
        }

        public async Task<bool> CheckWhetherEmailExistOrNot(string emailId)
        {
            try
            {

                var user = await _userRepository.GetUserByEmailAsync(emailId);
                if (user?.Email == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex) {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                throw;
            }
        }

        public async Task<(int count, bool succeeded)> UpdateCounterAsync(string userId, ApplicationUser user)
        {
            try
            {
                if (user.GenerateReportCount != InitialReportCount && user.GenerateReportCount < MaxReportCount)
                {
                    user.GenerateReportCount++;
                }
                else
                {
                    user.GenerateReportCount = ResetReportCount;
                }
                await _context.SaveChangesAsync();
           
                return (user.GenerateReportCount, true);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return (0, false);
                throw ex;
            }
        }

        public async Task<bool> SetUserConfiguration(string userId,int planid = 1)
        {            
            try
            {
                bool userConfigurationEXist = await _context.UserConfiguration.AnyAsync(x => x.UserId == userId).ConfigureAwait(false);

                if (!userConfigurationEXist)
                {
                    AddUserConfiguration(userId, planid);
                }
               
                return true;

            }
            catch (Exception ex) 
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                throw;
            }
        }

        public async Task<IUserConfigurations> GetUserConfigurationAsync(string userId)
        {
                 var userConfigurations = new UserConfigurations();                 
            try
            {
                 var result =  await _context.UserConfiguration.Where(x => x.UserId == userId).FirstOrDefaultAsync().ConfigureAwait(false);

                if (result != null) {

                    userConfigurations.Transcriptions                         = result.Transcriptions;
                    userConfigurations.SessionDurationLimit                   = result.SessionDurationLimit;
                    userConfigurations.AvailableHours                         = result.AvailableHours;
                    userConfigurations.RealTimeResults                        = result.RealTimeResults;
                    userConfigurations.PriorityAccessToTheLatestModels        = result.PriorityAccessToTheLatestModels;
                    userConfigurations.EarlyAccessToNewAIFeatures             = result.EarlyAccessToNewAIFeatures;
                    userConfigurations.GenerateDocumentsWithConfidence        = result.GenerateDocumentsWithConfidence;
                    userConfigurations.WatermarkRemoval                       = result.WatermarkRemoval;
                    userConfigurations.TailoredCapabilitiesAndAdvancedSupport = result.TailoredCapabilitiesAndAdvancedSupport;

                }

                int totalUserSessions = await _context.UserSession
                                              .Where(s => s.UserId == userId)
                                              .CountAsync()
                                              .ConfigureAwait(false);

                userConfigurations.UserSessionsCount = totalUserSessions;

                return userConfigurations;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                throw;
            }            
         }      

        #endregion

        #region PRIVATE METHODS
        private (string FirstName,string MiddleName, string LastName) SplitFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Full name cannot be null or empty", nameof(fullName));
            }

            string[] nameParts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length == 0)
            {
                throw new ArgumentException("Full name cannot be empty or contain only spaces", nameof(fullName));
            }

            string firstName = string.Empty;
            string lastName = string.Empty;
            string middleName = string.Empty;

            if (nameParts.Length == 1)
            {
                firstName = nameParts[0];
            }
            else if (nameParts.Length == 2)
            {
                firstName = nameParts[0];
                lastName = nameParts[1];
            }
            else
            {
                firstName = nameParts[0];
                middleName = string.Join(" ", nameParts.Skip(1).Take(nameParts.Length - 2));
                lastName = nameParts[nameParts.Length - 1];
            }


            return (FirstName: firstName,MiddleName:middleName, LastName: lastName);
        }

        private void AddUserConfiguration(string userId, int planid)
        {            

            UserConfiguration userConfiguration = new UserConfiguration();

            SetFeatureValues(userConfiguration, planid);

            userConfiguration.UserId = userId;
            userConfiguration.CreatedBy = userId;
            userConfiguration.CreatedDate = DateTime.Now;

            _context.UserConfiguration.Add(userConfiguration);
            _context.SaveChanges();           
            
        }
        
        private UserConfiguration SetFeatureValues(UserConfiguration userConfiguration,int planid)
        {
            try
            {
                var featurePlanConfiguration = _context.FeaturePlanConfiguration.Where(x => x.PlanId == planid).ToList();

                if (!featurePlanConfiguration.Any()) {
                    throw new ArgumentException("featurePlanConfiguration is empty for the planId {0}", planid.ToString());
                }

                foreach (var item in featurePlanConfiguration)
                {
                    switch (item.FeatureId)
                    {
                        case (long)GlobalEnums.ConfigurableFeatures.Transcriptions:
                            if (item.IsActive)
                                userConfiguration.Transcriptions = Convert.ToInt32(item.Value);
                            break;
                        case (long)GlobalEnums.ConfigurableFeatures.AvailableHours:
                            if (item.IsActive)
                                userConfiguration.AvailableHours = (long)Convert.ToInt32(item.Value);
                            break;
                        case (long)GlobalEnums.ConfigurableFeatures.SessionDurationLimit:
                            if (item.IsActive)
                                userConfiguration.SessionDurationLimit = (long)Convert.ToInt32(item.Value);
                            break;
                        case (long)GlobalEnums.ConfigurableFeatures.RealtimeResults:
                            userConfiguration.RealTimeResults = item.IsActive;
                            break;
                        case (long)GlobalEnums.ConfigurableFeatures.PriorityAccessToTheLatestModels:
                            userConfiguration.PriorityAccessToTheLatestModels = item.IsActive;
                            break;
                        case (long)GlobalEnums.ConfigurableFeatures.EarlyAccessToNewAIFeatures:
                            userConfiguration.EarlyAccessToNewAIFeatures = item.IsActive;
                            break;
                        case (long)GlobalEnums.ConfigurableFeatures.GeneratedocumentsWithConfidence:
                            userConfiguration.GenerateDocumentsWithConfidence = item.IsActive;
                            break;
                        case (long)GlobalEnums.ConfigurableFeatures.WatermarkRemoval:
                            userConfiguration.WatermarkRemoval = item.IsActive;
                            break;
                        case (long)GlobalEnums.ConfigurableFeatures.TailoredcapabilitiesAndAdvancedsupport:
                            userConfiguration.TailoredCapabilitiesAndAdvancedSupport = item.IsActive;
                            break;
                        default:
                            break;
                    }
                }
                return userConfiguration;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


    }

}
