using MediAssist.Application.Abstract.Services;
using MediAssist.DbContext;
using MediAssist.Infrastructure.HttpProvider.Services.Abstract;
using MediAssist.UI.Models;
using MediAssist.UI.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Encodings.Web;

namespace MediAssist.UI.Controllers
{
    [Route("api/manage")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region PRIVATE FIELD

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IEmailManagementService _emailManagementService;
        private const int MaxFailedAttempts = 7;
        private const int PasswordResetWindowHours = 1;
        private readonly ILogger<UsersController> _logger;

        #endregion

        #region CONSTRUCTOR
        public UsersController(UserManager<ApplicationUser> userManager,
                               IConfiguration configuration, 
                               IMailService mailService,
                               IUserService userService,
                               IEmailManagementService emailManagementService,
                               ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _userService = userService;
            _emailManagementService = emailManagementService;
            _logger = logger;
        }
        #endregion

        #region PUBLIC METHODS

        [HttpPost("forgetpassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgotPasswordRequest model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data." });
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return NotFound(new { success = false, message = "The entered email is not linked to any account. Please try again" });
                }

                if (user.LastForgotPasswordAttemptTimestamp.HasValue &&
                    user.LastForgotPasswordAttemptTimestamp.Value.AddHours(PasswordResetWindowHours) < DateTime.UtcNow)
                {
                    user.ForgotPasswordAttemptCount = 0;
                }

                if (user.ForgotPasswordAttemptCount >= MaxFailedAttempts &&
                    user.LastForgotPasswordAttemptTimestamp.HasValue &&
                    user.LastForgotPasswordAttemptTimestamp.Value.AddHours(PasswordResetWindowHours) > DateTime.UtcNow)
                {
                    return BadRequest(new { success = false, message = "Too many failed attempts. Try again later or contact support." });
                }

                var isPreviousTokenExpired = await _userManager.UpdateSecurityStampAsync(user);

                if (isPreviousTokenExpired.Succeeded)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var success = await _emailManagementService.SendResetPasswordEmail(user.FirstName, model.Email, HtmlEncoder.Default.Encode(code));

                if (success)
                {
                    user.ForgotPasswordAttemptCount++;
                    user.LastForgotPasswordAttemptTimestamp = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);
                }

                    return Ok(new { success = true, message = "A password reset link has been sent to your email." });
                }

                return BadRequest(new { success = false, message = "Unable to generate a password reset link. Please try again later or contact support." });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("validate-reset-token")]
        public async Task<IActionResult> ValidateResetToken([FromQuery] string token)
        {
            try
            {

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { success = false, message = "Invalid or missing token." });
                }

                string decodedToken;
                try
                {
                    decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
                }
                catch (FormatException)
                {
                    return BadRequest(new { success = false, message = "The reset link has expired or is invalid. Please request a new password reset." });
                }

                var users = _userManager.Users.ToList();
                IdentityUser foundUser = null;

                foreach (var user in users)
                {
                    var isValidToken = await _userManager.VerifyUserTokenAsync(user,
                        _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", decodedToken);

                    if (isValidToken)
                    {
                        foundUser = user;
                        break;
                    }
                }

                if (foundUser == null)
                {
                    return BadRequest(new { success = false, message = "The reset link has expired or is invalid. Please request a new password reset." });
                }

                return Ok(new { success = true, email = foundUser.Email });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            try
            {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid input." });
            }
            var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);

            if (user == null)
            {
                return BadRequest("Invalid Request");
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordViewModel.Token));

            var result = await _userManager.ResetPasswordAsync(user, decodedToken!, resetPasswordViewModel.Password!);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description);
                return BadRequest(new { success = false, message = $"{errors}" });
            }

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("sessionToken", "", cookieOptions);

            return Ok(new { success = true, message = "Your password has been reset successfully. Please sign in using your new password", redirectUrl = "/login" });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("passwordcheck")]
        public async Task<IActionResult> CheckPasswordIfExist(string userId, string password)
        {
            if (userId == null)
            {
                return BadRequest(new { success = false, message = "Invalid input." });
            }
            try
            {
                FormValidator.ValidatePasswordForLogin(password);
                var ifPasswordCorrect = await _userService.CheckWhetherCurrentPasswordCorrectOrNot(userId, password);
                if (!ifPasswordCorrect)
                {
                    return Ok(new { success = false, IfPasswordCorrect = false, message = "Please enter the correct password to proceed." });
                }
                return Ok(new { success = true, IfPasswordCorrect = true });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordModal)
        {
            try
            {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid input." });
            }
            var user = await _userManager.FindByIdAsync(changePasswordModal.UserId);

            if (user == null)
            {
                return BadRequest("Invalid Request");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordModal.Password, changePasswordModal.ConfirmPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description);
                return BadRequest(new { success = false, message = $"{errors}" });
            }
            return Ok(new { success = true, message = "Your password has been successfully changed. Please sign in using your new password", redirectUrl = "/login" });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("update-counter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateCounters([FromQuery] string userId)
        {
            try
            {
                if (userId == null)
                {
                    return BadRequest(new { success = false, message = "Invalid input." });
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var result = await _userService.UpdateCounterAsync(userId, user);

                if (result.succeeded)
                {
                    return Ok(new { success = true, message = "Counters updated successfully", count = result.count });
                }

                return BadRequest(new { success = false, message = "Counters updated failed", count = result.count });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }

        }

        [HttpPost("requestdemo")]
        public async Task<IActionResult> RequestDemo([FromBody] ScheduleDemoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data." });
            }

            try
            {
                FormValidator.ValidateName(model.Name);
                FormValidator.ValidateEmail(model.Email);
                FormValidator.ValidatePhoneNumber(model.Phone, model.CountryCode);
                FormValidator.ValidateRequirements(model.Requirements);

                var phoneNumber = $"{model.CountryCode} {model.Phone}";

                var response =  await _emailManagementService.SendRequestDemoEmail(model.Name, model.Email, phoneNumber, model.Requirements);

                if (response)
                {
                    return Ok(new { success = true, message = "Thank you for scheduling a demo! We will get in touch with you shortly." });
                }

                return BadRequest(new { success = false, message = "Unable to process your demo request. Please try again later." });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return BadRequest(new { success = false, message = $"An error occurred while processing your request, with Exception {ex.Message}." });
            }
        }

        [HttpGet("userconfig")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserConfigurations(string userId)
        {
            try
            {
                if (userId.IsNullOrEmpty())
                {
                    return BadRequest(new { success = false, message = "Invalid input." });
                }
                UserConfigurationsViewModel userConfigurations = new UserConfigurationsViewModel();
                //change to  the login area
                 await _userService.SetUserConfiguration(userId);

                var userconfig = await _userService.GetUserConfigurationAsync(userId).ConfigureAwait(false);

                if (userconfig != null)
                {
                    userConfigurations.Transcriptions = userconfig.Transcriptions;
                    userConfigurations.SessionDurationLimit = userconfig.SessionDurationLimit;
                    userConfigurations.AvailableHours = userconfig.AvailableHours;
                    userConfigurations.RealTimeResults = userconfig.RealTimeResults;
                    userConfigurations.PriorityAccessToTheLatestModels = userconfig.PriorityAccessToTheLatestModels;
                    userConfigurations.EarlyAccessToNewAIFeatures = userconfig.EarlyAccessToNewAIFeatures;
                    userConfigurations.GenerateDocumentsWithConfidence = userconfig.GenerateDocumentsWithConfidence;
                    userConfigurations.WatermarkRemoval = userconfig.WatermarkRemoval;
                    userConfigurations.TailoredCapabilitiesAndAdvancedSupport = userconfig.TailoredCapabilitiesAndAdvancedSupport;
                    userConfigurations.UserSessionsCount = userconfig.UserSessionsCount;
                }

                return Ok(userConfigurations);

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("contactUsForSubscription")]
        public async Task<IActionResult> ContactUsForSubscription([FromBody] ContactUsVieModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data." });
            }

            try
            {
                FormValidator.ValidateName(model.Name);
                FormValidator.ValidateEmail(model.Email);
                FormValidator.ValidatePhoneNumber(model.Phone, model.CountryCode);
                FormValidator.ValidateRequirements(model.AdditionalNotes);

                var phoneNumber = $"{model.CountryCode} {model.Phone}";

                var response =  await _emailManagementService.ContactUsForSubscription(model.Name, model.Email, phoneNumber, model.AdditionalNotes, model.SelectedPlan);

                if (response)
                {
                    return Ok(new { success = true, message = "Your request has been received. We will contact you shortly." });
                }

                return BadRequest(new { success = false, message = "Failed to submit your request. Please try again later." });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return BadRequest(new { success = false, message = $"An error occurred while processing your request, with Exception {ex.Message}." });
            }
        }

        [HttpPost("shareRreport")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ShareReport([FromForm] ReportSendViewModel report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data." });
            }
            try
            {
                FormValidator.ValidateName(report.RecipientName);
                FormValidator.ValidateEmail(report.Email);

                var response = await _emailManagementService.SendReportRequestEmail(report.file, report.Email, report.RecipientName, report.HospitalName, report.HospitalAddress, report.ReportName, report.PatientName, report.DoctorName, report.ConsultationDate);
                if (response)
                {
                    return Ok(new { success = true, message = "The Report han been send successfully" });
                }

                return BadRequest(new { success = false, message = "Unable to process. Please try again later." });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return BadRequest(new { success = false, message = $"An error occurred while processing your request, with Exception {ex.Message}." });
            }
            return Ok(report);
        }

        [HttpPost("generic-enquiry")]
        public async Task<IActionResult> GenericEnquiry([FromBody] ScheduleDemoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data." });
            }

            try
            {
                FormValidator.ValidateName(model.Name);
                FormValidator.ValidateEmail(model.Email);
                FormValidator.ValidatePhoneNumber(model.Phone, model.CountryCode);
                FormValidator.ValidateRequirements(model.Requirements);

                var phoneNumber = $"{model.CountryCode} {model.Phone}";

                var response = await _emailManagementService.SendGenericEnquiryEmail(model.Name, model.Email, phoneNumber, model.Requirements);

                if (response)
                {
                    return Ok(new { success = true, message = "Your request has been received. We will contact you shortly." });
                }

                return BadRequest(new { success = false, message = "Unable to process your request. Please try again later." });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return BadRequest(new { success = false, message = $"An error occurred while processing your request, with Exception {ex.Message}." });
            }
        }
        #endregion

        #region PRIVATE METHODS
        #endregion
    }
}
