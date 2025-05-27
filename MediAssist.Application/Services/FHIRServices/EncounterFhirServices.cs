using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.Configurations.Exceptions;
using MediAssist.DbContext;
using MediAssist.Infrastructure.Abstract.Configurations;
using MediAssist.Infrastructure.Extensions.Common;
using MediAssist.Infrastructure.HttpProvider.Services.Abstract;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediAssist.Configurations.GlobalEnums;

namespace MediAssist.Application.Services.FHIRServices
{
    class EncounterFhirServices : IFHIRService
    {

        #region PRIVATE FIELDS
        private readonly MediAssistDbContext _context;
        private readonly IFHIRMappingRepository _fhirMappingRepository;
        private readonly ILogger<IFHIRService> _logger;
        private readonly IAppSettings _appSettings;
        private readonly IFHIRHttpProvider _fhirHttpProvider;

        #endregion

        #region  CONSTRUCTOR
        public EncounterFhirServices(MediAssistDbContext context, IFHIRMappingRepository fhirMappingRepository,
            ILogger<IFHIRService> logger, IAppSettings appSettings, IFHIRHttpProvider fhirHttpProvider)
        {
            _context = context;
            _fhirMappingRepository = fhirMappingRepository;
            _logger = logger;
            _appSettings = appSettings;
            _fhirHttpProvider = fhirHttpProvider;
        }
        #endregion

        #region PUBLIC METHODS
        public async Task<HttpResponseMessage> GetDataFromFHIRStoreByResourceId(string authToken, string resourceId)
        {
            try
            {
                var url = _appSettings.FHIRBaseUrl + FHIRResourceTypes.Encounter;

                var response = await _fhirHttpProvider.GetAsync(resourceId, authToken, url);

                return response;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }

        public async Task InsertResourceAsync(string authToken)
        {
            try
            {
                var records = MapRecords(_context.DoctorPatientMapping.Where(x => !x.IsUpdatedtoFHIR).ToList());

                foreach (var record in records)
                {

                    var jsonRecord = JsonConvert.SerializeObject(record);

                    var url = _appSettings.FHIRBaseUrl + FHIRResourceTypes.Encounter;

                    var response = await _fhirHttpProvider.AddAsync(jsonRecord, authToken, url);

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        await _fhirMappingRepository.SaveDataToFHIRMapping(response, record.Id, FHIRResourceTypes.Encounter);

                        await UpdateEncounter(record.Id);
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong. Please refer to the exception details.");
                throw;
            }
        }
        #endregion

        #region PRIVATE METHODS
        private List<FHIREncounter> MapRecords(List<DoctorPatientMapping> sourceRecords)
        {
            List<FHIREncounter> patients = new List<FHIREncounter>();

            foreach (var record in sourceRecords)
            {
                var patientId = _context.FHIRStoreMapping.Where(x => x.EntityId == record.Patient_id.ToString())
                                  .Select(x => x.FHIRResourceId).FirstOrDefault();

                var practiotionerId = _context.FHIRStoreMapping.Where(x => x.EntityId == record.Practitioner_id)
                                  .Select(x => x.FHIRResourceId).FirstOrDefault();

                var doctorName = _context.Users.Where(x => x.Id == record.Practitioner_id).Select(x => x.FullName).FirstOrDefault();

                if (doctorName.IsNullOrEmpty() || patientId.IsNullOrEmpty() || practiotionerId.IsNullOrEmpty()) {
                    throw new BadRequestException("some of the data is not found ");
                }

                var encounterClass = new EncounterClass()
                {
                    System = FHIRResourceTypes.encounterClassSystem,
                    Code = FHIRResourceTypes.encounterClassCode,
                    Display = FHIRResourceTypes.encounterClassDisplay,
                };

                var subject = new Subject()
                {
                    Reference = FHIRResourceTypes.Patient + "/" + patientId,
                };

                var individual = new Individual()
                {
                    Reference = FHIRResourceTypes.Doctor + "/" + practiotionerId,
                    Display = doctorName
                };

                List<Participant> participants = new List<Participant>();
                var participant = new Participant()
                {
                    Individual = individual,
                };

                participants.Add(participant);


                var FHIREncounter = new FHIREncounter() {
                    Status = FHIRResourceTypes.encounterStatus,
                    resourceType = FHIRResourceTypes.Encounter,
                    Id = record.Id.ToString(),
                    Class = encounterClass,
                    Subject = subject,
                    Participant = participants,                   

                };

                patients.Add(FHIREncounter);
            }

            return patients;
        }

        private async Task UpdateEncounter(string entityId)
        {
            var currentEncounter = _context.DoctorPatientMapping.Where(x => x.Id == Convert.ToInt32(entityId) && !x.IsUpdatedtoFHIR).FirstOrDefault();

            if (currentEncounter != null)
            {
                currentEncounter.IsUpdatedtoFHIR = true;
                await _context.SaveChangesAsync();
            }
        }
        #endregion
    }
}
