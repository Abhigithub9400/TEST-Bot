using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.UI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MediAssist.UI.Controllers
{
    [Route("api/patient")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PatientController : ControllerBase
    {
        #region PRIVATE INSTANCE
        private readonly IPatientService  _patientService;
        private readonly ILogger<PatientController> _logger;

        #endregion

        #region CONSTRUCTOR
        public PatientController(IPatientService patientService, ILogger<PatientController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }

        #endregion

        #region PUBLIC METHODS

        [HttpPost("insert-update-patient")]
        public async Task<IActionResult> InsertOrUpdatePatientDetails([FromBody] PatientViewModel patientViewModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid input." });
            }
            try
            {

                var patientDetails = new PatientDetails()
                {
                    MrnNumber = patientViewModel.MrnNumber,
                    PatientName = patientViewModel.PatientName,
                    Age = patientViewModel.Age,
                    DOB =patientViewModel.DOB,
                    Gender = patientViewModel.Gender,
                    ClinicId = patientViewModel.ClinicId,
                    UserId = patientViewModel.UserId,
                    PatientId = patientViewModel.PatientId,
                    PatientConsent = patientViewModel.PatientConsent
                };

                if (patientViewModel.PatientId == null || patientViewModel.PatientId == 0) 
                {
                    var insertPatientDetails = await _patientService.InsertPatient(patientDetails);

                    if (insertPatientDetails.StatusCode == HttpStatusCode.OK)
                    {
                        return Ok(new { success = true, patientId = insertPatientDetails.PatientId });
                    }

                    return BadRequest(new { success = false, message = "Error inserting patient details" });
                }
                else
                {
                    var updatePatientDetails = await _patientService.UpdatePatient(patientDetails);

                    if (updatePatientDetails.StatusCode == HttpStatusCode.OK)
                    {
                        return Ok(new { success = true });
                    }

                    return BadRequest(new { success = false, message = "Error updating patient details" });
                }

            }

            catch (Exception ex)

            {
                _logger.LogError(ex, "An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        #endregion

    }
}
