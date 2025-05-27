using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace MediAssist.Application.Services
{
    public class PatientService : IPatientService
    {
        #region PRIVATE FIELDS
        private readonly IUserRepository _userRepository;
        private readonly ILogger<PatientService> _logger;

        #endregion

        #region CONSTRUCTOR
        public PatientService(IUserRepository userRepository, ILogger<PatientService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }
        #endregion

        #region PUBLIC METHODS

        public async Task<IPatientResponse> InsertPatient(IPatientDetails patientDetails)
        {
            try
            {
                if (patientDetails == null || string.IsNullOrWhiteSpace(patientDetails.PatientName))
                {
                    return new PatientResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var (firstName, lastName) = SplitFullName(patientDetails.PatientName);

                var patientId = await _userRepository.AddPatientAsync(firstName, lastName, patientDetails);

                if (patientId == null)
                {
                    return new PatientResponse
                    {
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                }

                if (patientDetails.ClinicId != null)
                {
                    await InsertEncounterAsync(patientId.Value, patientDetails.ClinicId.Value);
                }
                else
                {
                    _logger.LogWarning("ClinicId is missing. Cannot insert into Encounter table.");
                }

                return new PatientResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    PatientId = patientId
                };
            }
            catch (Exception ex) {

                _logger.LogError(ex, "An error occurred. See exception details:");
                return new PatientResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = "An unexpected error occurred while inserting the patient."
                };

            }

        }


        public async Task<IPatientResponse> UpdatePatient(IPatientDetails patientDetails)
        {
            try
            {

                if (patientDetails == null)
                {
                    return new PatientResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var existingPatient = await _userRepository.GetPatientByIdAsync(patientDetails.PatientId);

                if (existingPatient == null)
                {
                    return new PatientResponse 
                    {
                        StatusCode = HttpStatusCode.NotFound 
                    };
                }

                var (firstName, lastName) = SplitFullName(patientDetails.PatientName);


                existingPatient.FirstName = firstName;
                existingPatient.LastName = lastName;
                existingPatient.MrnNumber = patientDetails.MrnNumber;
                existingPatient.Gender = patientDetails.Gender;
                existingPatient.ModifiedBy = patientDetails.UserId;
                existingPatient.ModifiedDate = DateTime.Now;


                var updateResult = await _userRepository.UpdatePatientAsync(existingPatient);

                if (updateResult)
                {
                    return new PatientResponse
                    {
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    return new PatientResponse
                    { 
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                }
            }
            catch (Exception ex) {

                _logger.LogError(ex, "An error occurred. See exception details:");
                return new PatientResponse 
                { 
                    StatusCode = HttpStatusCode.InternalServerError 
                };
            }

            
        }

        #endregion

        #region PRIVATE METHODS
        private (string FirstName, string LastName) SplitFullName(string fullName)
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

            string firstName = nameParts[0]; // Always take the first word as FirstName
            string lastName = nameParts.Length > 1 ? nameParts[^1] : ""; // Take the last word if available

            return (firstName, lastName);
        }


        private async Task InsertEncounterAsync(int patientId, int clinicId)
        {
            var userId = await _userRepository.GetUserIdByClinicIdAsync(clinicId);

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("No doctor found");
                return;
            }
            var encounterResult = await _userRepository.AddEncounterAsync(patientId,userId);

            if (!encounterResult)
            {
                _logger.LogWarning("Encounter insertion failed");
            }
        }

        #endregion
    }
}
