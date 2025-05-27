using MediAssist.Application.Abstract.Repositories;
using MediAssist.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MediAssist.DataAccess.Repository
{
    public class FHIRMappingRepository : IFHIRMappingRepository    {

        #region   PRIVATE FIELDS
        private readonly MediAssistDbContext _context;
        private readonly ILogger<FHIRMappingRepository> _logger;
        private const string FHIRSyncJobName = "FHIRSyncJob";
        #endregion
        #region  CONSTRUCTOR
        public FHIRMappingRepository(MediAssistDbContext context, ILogger<FHIRMappingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        #region PUBLIC METHODS

        public async Task   SaveDataToFHIRMapping(HttpResponseMessage response, string recordId, string resourceType)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jsonContent = JsonConvert.DeserializeObject<dynamic>(content);


                if (jsonContent != null)
                {
                    var id = jsonContent.id;

                    FHIRStoreMapping fhirStoreMapping = new FHIRStoreMapping
                    {
                        EntityId = recordId,
                        ResourceType = resourceType,
                        FHIRResourceId = id,
                        CreatedBy = FHIRSyncJobName
                    };

                    await AddOrUpdateFHIRMapping(fhirStoreMapping);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }
        private async Task<FHIRStoreMapping> AddOrUpdateFHIRMapping(FHIRStoreMapping fhirdata)
        {
            try
            {
                FHIRStoreMapping fhirStoreMappedData = new FHIRStoreMapping();
                var isFHIRDataMapped = await IsFHIRDataMapped(fhirdata.EntityId, fhirdata.ResourceType);
                if (!isFHIRDataMapped)
                {
                    fhirStoreMappedData = await AddFHIRMappingAsync(fhirdata);
                }
                else
                {
                    fhirStoreMappedData = await UpdateFHIRMappingAsync(fhirdata);
                }

                return fhirStoreMappedData;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Something went wrong. Please refer to the exception details.");
                throw;
            }
        }
        #endregion
        #region PRIVATE METHODS
        public async Task<FHIRStoreMapping> AddFHIRMappingAsync(FHIRStoreMapping fhirdata)
        {
            try
            {
                fhirdata.CreatedAt = DateTime.Now;
                fhirdata.ModifiedBy = " ";

                _context.FHIRStoreMapping.Add(fhirdata);
                await _context.SaveChangesAsync();

                return fhirdata;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Something went wrong. Please refer to the exception details.");
                throw;
            }
          
        }

        public async Task<bool> IsFHIRDataMapped(string entityId,string resourceType)
        {
            return  await _context.FHIRStoreMapping.Where(x => x.EntityId == entityId && x.ResourceType == resourceType).AnyAsync();
           
        }

        public async Task<FHIRStoreMapping> UpdateFHIRMappingAsync(FHIRStoreMapping fhirdata)
        {
            try
            {
                var existingData = await  _context.FHIRStoreMapping
                                         .Where(x => x.EntityId == fhirdata.EntityId && x.ResourceType == fhirdata.ResourceType)
                                         .FirstOrDefaultAsync();

                if (existingData == null)
                {
                    throw new KeyNotFoundException($"FHIR mapping not found for EntityId: {fhirdata.EntityId}, ResourceType: {fhirdata.ResourceType}");
                }
                else
                {
                    // Update the fields
                    existingData.FHIRResourceId = fhirdata.FHIRResourceId;
                    existingData.ModifiedBy = FHIRSyncJobName;
                    existingData.ModifiedAt = DateTime.UtcNow; // Updating modification timestamp

                    await  _context.SaveChangesAsync(); // Save changes to DB
                }
                return existingData;
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong. Please refer to the exception details.");
                throw;
            }

        }

        #endregion
    }
}