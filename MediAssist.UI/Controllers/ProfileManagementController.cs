using Azure;
using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.DbContext;
using MediAssist.UI.Models;
using MediAssist.UI.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace MediAssist.UI.Controllers
{
    [Route("api/profile")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileManagementController : ControllerBase
    {
        #region PRIVATE FIELD
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MediAssistDbContext _context;
        private readonly IProfileManagementService _profileManagementService;
        private readonly ILogger<ProfileManagementController> _logger;

        #endregion

        #region CONSTRUCTOR
        public ProfileManagementController(UserManager<ApplicationUser> userManager, 
                                            MediAssistDbContext context,
                                            IProfileManagementService profileManagementService,
                                            ILogger<ProfileManagementController> logger)
        {
            _userManager = userManager;
            _context = context;
            _profileManagementService = profileManagementService;
            _logger = logger;
        }

        #endregion

        #region PUBLIC METHODS
        [HttpGet("get")]
        public async Task<IActionResult> GetUserDetails([FromQuery] string UserId)
        {
            try
            {


                if (string.IsNullOrEmpty(UserId))
                {
                    return BadRequest(new { success = false, message = "Invalid or missing UserId." });
                }

                var user = await _userManager.FindByIdAsync(UserId);

                if (user == null)
                {
                    return NotFound(new { success = false, message = "User not found." });
                }

                var userDetails = await _context.DoctorProfiles.FirstOrDefaultAsync(dp => dp.UserId == UserId);

                string imageBase64 = null;
                if (userDetails?.Image != null && userDetails?.Image?.Length > 0)
                {
                    imageBase64 = Convert.ToBase64String(userDetails.Image);
                }

                string signBase64 = null;
                if (userDetails?.Signature != null && userDetails?.Signature?.Length > 0)
                {
                    signBase64 = Convert.ToBase64String(userDetails.Signature);
                }

                var result = new
                {
                    success = true,
                    email = user.Email,
                    fullName = user.FullName,
                    title = userDetails?.Title,
                    gender = userDetails?.Gender,
                    image = imageBase64,
                    dob = userDetails?.DOB,
                    licenseNumber = userDetails?.LicenseNumber,
                    medicalCredentials = userDetails?.MedicalCredentials,
                    specialization = userDetails?.Specialization,
                    signature = signBase64
                };

                return Ok(result);
            }catch(Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                throw;
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel updateProfileViewModel)
        {
            try
            {
                if (updateProfileViewModel == null)
                {
                return BadRequest(new { success = false, message = "Profile data is required." });
                }

            var updateUserProfile = new UpdateUserDetails()
            {
                UserId = updateProfileViewModel.UserId,
                Title = updateProfileViewModel.Title,
                Gender = updateProfileViewModel.Gender,
                Image = updateProfileViewModel.Image,
                DOB = updateProfileViewModel.DOB,
                LicenseNumber = updateProfileViewModel.LicenseNumber,
                MedicalCredentials = updateProfileViewModel.MedicalCredentials,
                Specialization = updateProfileViewModel.Specialization
            };

            var user = await _profileManagementService.FindUserAsync(updateUserProfile.UserId);

            if (user == null)
            {
                return NotFound(new { success = false, message = "User not found." });
            }

            await ValidateFields(updateProfileViewModel);

            var validationResult = ValidateDateOfBirth(updateProfileViewModel.DOB);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { success = false, message = validationResult.ErrorMessage });
            }
            
                var response = _profileManagementService.UpdateProfileAsync(user.Id, updateUserProfile);
                if (response.Result.HttpStatusCode == HttpStatusCode.OK)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Profile updated successfully.",
                        title = response.Result.UserTitle?.Abbreviations
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "An error occurred while updating the profile."
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return StatusCode(500, new { success = false, message = "An error occurred while updating the profile.", details = ex.Message });
            }

        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteProfile([FromQuery] string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return BadRequest(new { success = false, message = "Invalid or missing UserId." });
            }
            try
            {
                var user = await _profileManagementService.FindUserAsync(UserId);

                if (user == null)
                {
                    return NotFound(new { success = false, message = "User not found." });
                }

            
                var result = await _profileManagementService.DeleteUserAccount(user);

                if (result.Succeeded)
                {
                    return Ok(new { success = true, message = "Your account has been successfully deleted. Thank you for using MediAssist AI.", redirectUrl = "/" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed to delete the account." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return StatusCode(500, new { success = false, message = "An error occurred while deleting the account.", details = ex.Message });
            }
        }
        #endregion

        #region PRIVATE METHODS
        private async Task ValidateFields(UpdateProfileViewModel updateProfileViewModel)
        {
            await FormValidator.ValidateTitleAsync(updateProfileViewModel.Title, _context);
            await FormValidator.ValidateGenderAsync(updateProfileViewModel.Gender, _context);
            await FormValidator.ValidateMedicalCredentialsAsync(updateProfileViewModel.MedicalCredentials, _context);
        }

        private (bool IsValid, string ErrorMessage) ValidateDateOfBirth(DateTime dob)
        {
            // Check if the date is in the past
            if (dob >= DateTime.Today)
            {
                return (false, "Date of birth must be in the past.");
            }

            // Check minimum age (18 years)
            if ((DateTime.Today.Year - dob.Year) < 18 || (dob > DateTime.Today.AddYears(-18)))
            {
                return (false, "You must be at least 18 years old.");
            }

            // Check for leap year validation for February 29
            if (dob.Month == 2 && dob.Day == 29 && !DateTime.IsLeapYear(dob.Year))
            {
                return (false, "February 29 is only valid in leap years.");
            }
            return (true, string.Empty);
        }

        #endregion
    }
}
