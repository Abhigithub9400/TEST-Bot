using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Services
{
    public class ProfileManagementService : IProfileManagementService
    {
        #region PRIVATE FIELDS

        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MediAssistDbContext _context;

        #endregion

        #region CONSTRUCTOR
        public ProfileManagementService(IUserRepository userRepository,
                                        UserManager<ApplicationUser> userManager,
                                        MediAssistDbContext context,
                                        ILogger<ProfileManagementService> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _context = context;
        }

        #endregion

        #region PUBLIC METHODS

        public async Task<ApplicationUser> FindUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                return user;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving user with ID {userId}. See inner exception for details.", ex);
            }
        }


      
        public async Task<DoctorProfile> FindUserDetailsAsync(string userId)
        {
            try
            {
                var userDetails = await _userRepository.GetUserDetailsbyIdAsync(userId);
                return userDetails;
            }
            catch (Exception ex) {
                throw new InvalidOperationException($"An unexpected error occurred while retrieving user details for ID: {userId}.", ex);
            }
        }

        public async Task<List<Feedback>> FindUserFeedBackAsync(string userId)
        {
            try
            {
                var feedbacks = await _userRepository.GetUserFeedBacksByIdAsync(userId);
                return feedbacks;
            }
            catch (Exception ex) {
                throw new InvalidOperationException($"An error occurred while retrieving feedback for user ID: {userId}.", ex);
            }
        }

        public async Task<Clinic> FindUserClinicDetailsAsync(int? clinicId)
        {
            try
            {

                var clinicDetails = await _userRepository.GetUserClinicDetailsByIdAsync(clinicId);              
                return clinicDetails;
            }
            catch (Exception ex) {
                throw new InvalidOperationException($"An error occurred while retrieving clinic details for ID: {clinicId}.", ex);
            }

        }

        public async Task<bool> CheckWhetherUserHistoryIsExist(string emailId)
        {
            var isUserHistoryExist = false;
            if (emailId != null)
            {
                isUserHistoryExist = await _userRepository.CheckWhetherUserHistoryIsExist(emailId);

            }
            return isUserHistoryExist;
        }

        public async Task<(HttpStatusCode HttpStatusCode, UserTitle? UserTitle)> UpdateProfileAsync(string userId, IUpdateUserDetails updateUserDetails)
        {
            var doctorProfile = await GetOrCreateDoctorProfileAsync(userId);

            UpdateProfileDetailsAsync(updateUserDetails, doctorProfile);

            try
            {
                await _context.SaveChangesAsync();
                var userTitle = await _userRepository.GetUserTitlebyIdAsync(doctorProfile?.Title);
                return (HttpStatusCode.OK, userTitle);
            }
            catch (Exception)
            {
                return (HttpStatusCode.InternalServerError, null);
            }
        }

        public async Task<IdentityResult> DeleteUserAccount(ApplicationUser user)
        {
            try
            {
                if (user != null)
                {
                    var userDetails = await FindUserDetailsAsync(user.Id);
                    var feedbacks = await FindUserFeedBackAsync(user.Id);

                    if (feedbacks != null)
                    {
                        foreach (var feedback in feedbacks)
                        {
                            feedback.UserId = null;
                            _context.Feedbacks.Update(feedback);
                        }
                    }

                    if (userDetails != null)
                    {
                        var clinicDetails = await FindUserClinicDetailsAsync(userDetails.ClinicId);

                        if (clinicDetails != null)
                        {
                            _context.Clinics.Remove(clinicDetails);
                        }

                        _context.DoctorProfiles.Remove(userDetails);
                    }

                    var isUserHistoryDetailsExist = await CheckWhetherUserHistoryIsExist(user?.UserName);

                    if (!isUserHistoryDetailsExist)
                    {
                        var userHistory = new UsersHistory()
                        {
                            Name = user.FullName,
                            EmailAddress = user.UserName
                        };

                        _context.UsersHistories.Add(userHistory);
                    }

                    await _context.SaveChangesAsync();

                   
                }
                var result = await _userManager.DeleteAsync(user);

                return result;

            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DeleteUserAccountError",
                    Description = ex.Message
                });
            }
            
        }

        #endregion

        #region PRIVATE METHODS

        private async Task<DoctorProfile> GetOrCreateDoctorProfileAsync(string userId)
        {
            var doctorProfile = await _context.DoctorProfiles.FirstOrDefaultAsync(dp => dp.UserId == userId);
            if (doctorProfile == null)
            {
                doctorProfile = new DoctorProfile
                {
                    UserId = userId
                };
                _context.DoctorProfiles.Add(doctorProfile);
            }

            return doctorProfile;
        }

        private static void UpdateProfileDetailsAsync(IUpdateUserDetails updateUserDetails, DoctorProfile doctorProfile)
        {
            doctorProfile.Title = updateUserDetails.Title;
            doctorProfile.Gender = updateUserDetails.Gender;
            doctorProfile.DOB = updateUserDetails.DOB;

            if (!string.IsNullOrEmpty(updateUserDetails.Image))
            {
                if (updateUserDetails.Image.StartsWith("data:image/png;base64,"))
                {
                    updateUserDetails.Image = updateUserDetails.Image.Substring("data:image/png;base64,".Length);
                }
                else if (updateUserDetails.Image.StartsWith("data:image/jpeg;base64,"))
                {
                    updateUserDetails.Image = updateUserDetails.Image.Substring("data:image/jpeg;base64,".Length);
                }

                doctorProfile.Image = Convert.FromBase64String(updateUserDetails.Image);
            }

            doctorProfile.LicenseNumber = updateUserDetails.LicenseNumber;
            doctorProfile.MedicalCredentials = updateUserDetails.MedicalCredentials;
            doctorProfile.Specialization = updateUserDetails.Specialization;
        }

        #endregion
    }
}
