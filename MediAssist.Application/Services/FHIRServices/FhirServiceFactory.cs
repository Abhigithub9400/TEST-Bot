using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Abstract.Services;
using MediAssist.DbContext;
using MediAssist.Infrastructure.Abstract.Configurations;
using MediAssist.Infrastructure.HttpProvider.Services.Abstract;
using Microsoft.Extensions.Logging;
using static MediAssist.Configurations.GlobalEnums;

namespace MediAssist.Application.Services.FHIRServices
{
    public class FHIRServiceFactory : IFHIRServiceFactory
    {
        private readonly MediAssistDbContext _context;
        private readonly IFHIRMappingRepository _fHIRMappingRepository;
        private readonly ILoggerFactory _logger;
        private readonly IAppSettings _appSettings;
        private readonly IFHIRHttpProvider _fHIRHttpProvider;
        private readonly IAzureKeyVaultService _keyVault; // Add this field

        public FHIRServiceFactory(MediAssistDbContext context, IFHIRMappingRepository FHIRMappingRepository,
                                  ILoggerFactory logger, IAppSettings appSettings, IFHIRHttpProvider fHIRHttpProvider,
                                  IAzureKeyVaultService keyVault) // Add keyVault parameter
        {
            _context = context;
            _fHIRMappingRepository = FHIRMappingRepository;
            _logger = logger;
            _appSettings = appSettings;
            _fHIRHttpProvider = fHIRHttpProvider;
            _keyVault = keyVault; // Initialize keyVault
        }

        public IFHIRService GetFHIRService(string resourceType)
        {
            try
            {
                switch (resourceType.ToLower())
                {
                    case var rt when rt == FHIRResourceTypes.Patient.ToLowerInvariant():
                        return new PatientFHIRServices(
                            _context, _fHIRMappingRepository,
                            _logger.CreateLogger<PatientFHIRServices>(),
                            _appSettings, _fHIRHttpProvider, _keyVault); // Pass keyVault
                    case var rt when rt == FHIRResourceTypes.Doctor.ToLowerInvariant():
                        return new PractitionerFHIRService(
                            _context, _fHIRMappingRepository,
                            _logger.CreateLogger<PractitionerFHIRService>(),
                            _appSettings, _fHIRHttpProvider);
                    case var rt when rt == FHIRResourceTypes.Encounter.ToLowerInvariant():
                        return new EncounterFhirServices(
                            _context, _fHIRMappingRepository,
                            _logger.CreateLogger<EncounterFhirServices>(),
                            _appSettings, _fHIRHttpProvider);
                }
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
