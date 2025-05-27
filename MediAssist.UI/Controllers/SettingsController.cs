using Azure;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.DbContext;
using MediAssist.UI.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediAssist.UI.Controllers
{
    [Route("api/settings")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;
        private readonly MediAssistDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<SettingsController> _logger;


        public SettingsController(ISettingsService settingsService, MediAssistDbContext context, UserManager<ApplicationUser> userManager, ILogger<SettingsController> logger)
        {
            _settingsService = settingsService;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("update-clinic")]
        public async Task<IActionResult> UpdateClinic([FromBody] ClinicDetails clinicDetails)
        {
            try
            {
                await FormValidator.ValidateClinicDetails(clinicDetails);

                var updateClinicDetails = await _settingsService.UpdateClinic(clinicDetails);

                if (!updateClinicDetails.Success)
                {
                    return BadRequest(updateClinicDetails);
                }

                var result = new
                {
                    success = true,
                    message = "Clinic details updated successfully"
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return StatusCode(500, new { success = false, message = "An unexpected error occurred while updating the clinic details.", details = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-clinic")]
        public async Task<IActionResult> GetClinic([FromQuery] string userId)
        {
            try
            {
                if (userId is null)
                {
                    return NotFound(new { success = false, message = "User not found." });
                }

                var clinicDetails = await _settingsService.GetClinicByUserIdAsync(userId);

                string logoBase64 = null;
                if (clinicDetails?.Data?.Logo != null && clinicDetails?.Data?.Logo?.Length > 0)
                {
                    logoBase64 = Convert.ToBase64String(clinicDetails.Data.Logo);
                }

                if (!clinicDetails.Success)
                {
                    return BadRequest(clinicDetails);
                }

                var result = new
                {
                    success = true,
                    clinicName = clinicDetails.Data?.Name,
                    clinicAddress = clinicDetails.Data?.Address,
                    phoneNumber = clinicDetails.Data?.PhoneNumber,
                    countryCode= clinicDetails.Data?.CountryCode,
                    email = clinicDetails.Data?.Email,
                    website = clinicDetails.Data?.Website,
                    logo = logoBase64
                };
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return StatusCode(500, new { success = false, message = "An unexpected error occurred while fetching the clinic.", details = ex.Message });
            }
        }

        [HttpPost]
        [Route("update-doctor")]

        public async Task<IActionResult> UpdateDoctorProfile([FromBody] DoctorProfileSettings doctorProfileSettings)
        {
            try
            {
                await FormValidator.ValidateDoctorProfileSettings(doctorProfileSettings, _context);

                var updateDoctorProfile = await _settingsService.UpdateDoctorProfile(doctorProfileSettings);

                if (!updateDoctorProfile.Success)
                {
                    return BadRequest(updateDoctorProfile);
                }

                var result = new
                {
                    success = true,
                    message = "Doctor's information updated successfully"
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                new ServiceResponse<DoctorProfile>
                {
                    Success = false,
                    Message = "An error occurred while updating the doctor's information."
                });
            }
        }

        [HttpGet]
        [Route("get-report-details")]
        public async Task<IActionResult> GetClinicAndDoctorDetails([FromQuery] string userId)
        {
            try
            {
                if (userId is null)
                {
                    return NotFound(new { success = false, message = "Invalid input" });
                }


                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new { success = false, message = "User not found." });
                }

                var reportDetails = await _settingsService.GetClinicAndDoctorDetailsAsync(user);

                if (reportDetails.Success)
                {
                    var result = new
                    {
                        success = true,
                        DoctorName = reportDetails.Data.DoctorName,
                        DoctorSpecialization = reportDetails.Data.DoctorSpecialization,
                        DoctorTitle = reportDetails.Data.DoctorTitle,
                        DoctorSignature = reportDetails.Data.DoctorSignature,
                        HospitalName = reportDetails.Data.HospitalName,
                        HospitalAddress = reportDetails.Data.HospitalAddress,
                        HospitalLogo = reportDetails.Data.HospitalLogo,
                        ClinicId = reportDetails.Data.ClinicId
                    };

                    return Ok(result);
                }

                return BadRequest(new { success = false, message = "Failed to fetch the doctor's or hospital's details.", details = reportDetails.Message});

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return StatusCode(500, new { success = false, message = "An unexpected error occurred while fetching the doctor's or hospital's details.", details = ex.Message });
            }
        }

        [HttpGet("check-settings-updated")]
        public async Task<IActionResult> CheckWhetherSettingsUpdated([FromQuery] string userId)
        {
            try
            {
                if (userId is null)
                {
                    return NotFound(new { success = false, message = "Invalid input" });
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new { success = false, message = "User not found." });
                }

                var isSettingsUpdated = await _settingsService.CheckWhetherSettingsUpdated(user);

                var result = new
                {
                    IsSettingsUpdated = isSettingsUpdated,
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
