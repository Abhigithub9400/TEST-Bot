using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.DbContext;
using MediAssist.UI.Models;
using MediAssist.UI.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.SecurityTokenService;
using System.Net;

namespace MediAssist.UI.Controllers
{
    [Route("api/manage")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        #region PRIVATE FIELD
        private readonly IUserService _userService;
        private readonly MediAssistDbContext _context;
        private readonly ILogger<SignUpController> _logger;
        #endregion

        #region CONSTRUCTOR
        public SignUpController(IUserService userService, MediAssistDbContext context, ILogger<SignUpController> logger)
        {
            _userService = userService;
            _context = context;
            _logger = logger;   
        }
        #endregion

        #region PUBLIC METHODS
        [HttpGet("emailcheck")]
        public async Task<IActionResult> CheckEmailIfExist(string emailId)
        {
            if (emailId == null) 
            {
                return BadRequest(new { success = false, message = "Invalid input." });
            }
            try
            {
                FormValidator.ValidateEmail(emailId);
                var ifEmailExist = await _userService.CheckWhetherEmailExistOrNot(emailId);

                return Ok(new { IfEmailExist = ifEmailExist });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid input." });
            }

            try
            {
                FormValidator.ValidateName(signUpViewModel.Name);
                FormValidator.ValidateEmail(signUpViewModel.Email);
                FormValidator.ValidatePassword(signUpViewModel.Password);
                FormValidator.ValidateConfirmPassword(signUpViewModel.Password, signUpViewModel.ConfirmPassword);
                FormValidator.ValidateSpecialization(signUpViewModel.Specialization);
                await FormValidator.ValidateTitleAsync(signUpViewModel.Title, _context);
                await FormValidator.ValidateGenderAsync(signUpViewModel.Gender, _context);
                await FormValidator.ValidateMedicalCredentialsAsync(signUpViewModel.MedicalCredentials, _context);

                var validationResult = ValidateDateOfBirth(signUpViewModel.DateOfBirth);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new { success = false, message = validationResult.ErrorMessage });
                }


                var signUpUserDetails = new SignUpUserDetails()
                {
                    FullName = signUpViewModel.Name,
                    Email = signUpViewModel.Email,
                    Password = signUpViewModel.Password,
                    TermsAndPrivacy = signUpViewModel.TermsAndPrivacy,
                    Title = signUpViewModel.Title,
                    Gender = signUpViewModel.Gender,
                    DateOfBirth = signUpViewModel.DateOfBirth,
                    MedicalCredentials = signUpViewModel.MedicalCredentials,
                    Specialization = signUpViewModel.Specialization,
                    LicenseAgreement = signUpViewModel.LicenseAgreement,
                    IsActive = true
                };
                var response = await _userService.SignUpWithEmailAndPasswordAsync(signUpUserDetails);

                if (response == HttpStatusCode.OK || response == HttpStatusCode.Created)
                {
                    return Ok(new { success = true, redirectUrl = "/login" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Invalid SignUp attempt." });
                }
            }
            catch (BadRequestException ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { success = false, message = $"{ex.Message}" });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { success = false, message = "An unexpected error occurred." });
            }
        }


        #endregion

        #region PRIVATE METHODS
        private (bool IsValid, string ErrorMessage) ValidateDateOfBirth(DateTime dob)
        {
            // Check if the date is in the past
            if (dob >= DateTime.Today)
            {
                return (false, "Date of birth cannot be a future date.");
            }

            // Check minimum age (18 years)
            if ((DateTime.Today.Year - dob.Year) < 18 || (dob > DateTime.Today.AddYears(-18)))
            {
                return (false, "The age must be above 18 years.");
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
