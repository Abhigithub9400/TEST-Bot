using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.DbContext;
using MediAssist.Infrastructure.Abstract.Configurations;
using MediAssist.Infrastructure.Extensions.Common;
using MediAssist.Infrastructure.HttpProvider.Services.Abstract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static MediAssist.Configurations.GlobalEnums;

namespace MediAssist.Application.Services.FHIRServices
{
    public class PatientFHIRServices : IFHIRService
    {
        #region PRIVATE FIELDS
        private readonly MediAssistDbContext _context;
        private readonly IFHIRMappingRepository _fhirMappingRepository;
        private readonly ILogger<IFHIRService> _logger;
        private readonly IAppSettings _appSettings;
        private readonly IFHIRHttpProvider _fhirHttpProvider;
        private readonly IAzureKeyVaultService _keyVault;

        #endregion

        #region  CONSTRUCTOR
        public PatientFHIRServices(MediAssistDbContext context, IFHIRMappingRepository fhirMappingRepository, 
                                   ILogger<IFHIRService> logger, IAppSettings appSettings, 
                                   IFHIRHttpProvider fhirHttpProvider, IAzureKeyVaultService keyVault)
        {
            _context = context;
            _fhirMappingRepository = fhirMappingRepository;
            _logger = logger;
            _appSettings = appSettings;
            _fhirHttpProvider = fhirHttpProvider;
            _keyVault = keyVault;
        }
        #endregion

        #region PUBLIC METHODS
        public async Task InsertResourceAsync(string authToken)
        {
            try
            {
                var records = await MapRecords(_context.Patients.Where(x => !x.IsUpdatedtoFHIR).ToList());

                foreach (var record in records)
                {
                    var jsonRecord = JsonConvert.SerializeObject(record);

                    var url = _appSettings.FHIRBaseUrl + FHIRResourceTypes.Patient;

                    var response = await _fhirHttpProvider.AddAsync(jsonRecord, authToken, url);

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        await _fhirMappingRepository.SaveDataToFHIRMapping(response, record.Id, FHIRResourceTypes.Patient);

                        await UpdatePatientTable(record.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong. Please refer to the exception details.");
                throw;
            }
        }
        public async Task<HttpResponseMessage> GetDataFromFHIRStoreByResourceId(string authToken,string resourceId)
        {
            try
            {                        
                var url = _appSettings.FHIRBaseUrl + FHIRResourceTypes.Patient;

                var response = await _fhirHttpProvider.GetAsync(resourceId, authToken, url);

                return response;
                 

            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }
        #endregion

        #region PRIVATE METHODS


        private async Task<List<FHIRPatient>> MapRecords(List<DbContext.Patient> sourceRecords)
        {
            string key = await _keyVault.GetSecretAsync("Encryption-Key");
            string iv = await _keyVault.GetSecretAsync("Encryption-IV");
            List<FHIRPatient> patients = new List<FHIRPatient>();

            foreach (var record in sourceRecords)
            {
                Name name = new Name
                {
                    Given = new[] { EncryptionHelper.Decrypt(record.FirstName, key, iv) },
                    Family = EncryptionHelper.Decrypt(record.LastName, key, iv)
                };

                FHIRPatient patient = new FHIRPatient
                {
                    resourceType = FHIRResourceTypes.Patient,
                    Id = record.PatientId.ToString(),
                    Name = new[] { name },
                    Gender = await MapToFhirGender(record.Gender)
                };

                patients.Add(patient);
            }

            return patients;
        }

        private async Task<string> MapToFhirGender(string encryptedGender)
        {
            string key = await _keyVault.GetSecretAsync("Encryption-Key");
            string iv = await _keyVault.GetSecretAsync("Encryption-IV");

            var gender = EncryptionHelper.Decrypt(encryptedGender, key, iv);
            return gender switch
            {
                Gender.Transgender or Gender.Nonbinary => Gender.Other,
                Gender.PreferNotToSay => Gender.Unknown,
                Gender.Male => Gender.Male,
                Gender.Female => Gender.Female,
                _ => Gender.Unknown // fallback for unexpected values  
            };
        }

        private async Task UpdatePatientTable(string entityId)
        {
            var currentPatientData = _context.Patients.Where(x => x.PatientId == Convert.ToInt32(entityId) && !x.IsUpdatedtoFHIR).FirstOrDefault();

            if (currentPatientData != null)
            {
                currentPatientData.IsUpdatedtoFHIR = true;
                await  _context.SaveChangesAsync();
            }
        }
      
        #endregion
    }
}
