using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.DbContext;
using MediAssist.Infrastructure.Abstract.Configurations;
using MediAssist.Infrastructure.HttpProvider.Services.Abstract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static MediAssist.Configurations.GlobalEnums;

namespace MediAssist.Application.Services.FHIRServices
{
    public class PractitionerFHIRService : IFHIRService
    {
        #region PRIVATE FIELDS
        private readonly MediAssistDbContext _context;
        private readonly IFHIRMappingRepository _fhirMappingRepository;
        private readonly ILogger<IFHIRService> _logger;
        private readonly IAppSettings _appSettings;
        private readonly IFHIRHttpProvider _fhirHttpProvider;
        #endregion

        #region  CONSTRUCTOR
        public PractitionerFHIRService(MediAssistDbContext context, IFHIRMappingRepository fhirMappingRepository, 
                                        ILogger<IFHIRService> logger, IAppSettings appSettings,IFHIRHttpProvider fhirHttpProvider)
        {
            _context = context;
            _fhirMappingRepository = fhirMappingRepository;
            _logger = logger;
            _appSettings = appSettings;
            _fhirHttpProvider = fhirHttpProvider;
        }
        #endregion
        #region Public Methods
        public async Task<HttpResponseMessage> GetDataFromFHIRStoreByResourceId(string authToken, string resourceId)
        {
            try
            {
                var url = _appSettings.FHIRBaseUrl + FHIRResourceTypes.Doctor;           

                var response = await _fhirHttpProvider.GetAsync(resourceId, authToken, url);
                                
                return response;
                
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Something went wrong. Please refer to the exception details.");
                throw;
            }
        }

        public async Task InsertResourceAsync(string authToken)
        {
            try
            {

                var records = MapRecords(_context.Users.Where(x => !x.IsUpdatedtoFHIR).ToList());

                foreach (var record in records)
                {
                    
                    var jsonRecord = JsonConvert.SerializeObject(record);                
                                      
                    var url = _appSettings.FHIRBaseUrl + FHIRResourceTypes.Doctor;

                    var response =  await _fhirHttpProvider.AddAsync(jsonRecord, authToken, url);

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        await _fhirMappingRepository.SaveDataToFHIRMapping(response, record.Id, FHIRResourceTypes.Doctor);                        

                        await UpdateUser(record.Id);
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
        #region Private Methods        

        private List<FHIRPractitioner> MapRecords(List<DbContext.ApplicationUser> sourceRecords)
        {
            List<FHIRPractitioner> practitioners = new List<FHIRPractitioner>();

            foreach (var record in sourceRecords)
            {
                Name name = new Name
                {
                    Given = new[] { record.FirstName},
                    Family = record.LastName
                };

                FHIRPractitioner practitioner = new FHIRPractitioner
                {
                    resourceType = FHIRResourceTypes.Doctor,
                    Id = record.Id.ToString(),
                    Name = new[] { name },
                   
                };

                practitioners.Add(practitioner);
            }

            return practitioners;
        }

        private async Task UpdateUser(string entityId)
        {
            var currentPatientData = _context.Users.Where(x => x.Id == entityId && !x.IsUpdatedtoFHIR).FirstOrDefault();

            if (currentPatientData != null)
            {
                currentPatientData.IsUpdatedtoFHIR = true;
                await _context.SaveChangesAsync();
            }
        }
        #endregion
    }
}
