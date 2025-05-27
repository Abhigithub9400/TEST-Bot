using Google.Apis.Auth.OAuth2;
using MediAssist.Application.Abstract.Services;
using MediAssist.Infrastructure.HttpProvider.Services.Abstract;
using Microsoft.Extensions.Logging;
using static MediAssist.Configurations.GlobalEnums;

namespace MediAssist.Application.Services
{
    public class FHIRServiceExecutor : IFHIRServiceExecutor
    {
        #region PRIVATE 
        private readonly IFHIRServiceFactory _FHIRServiceFactory;
        private readonly ILogger<FHIRServiceExecutor> _logger;
        private readonly IAzureKeyVaultService _keyVaultService;
        #endregion

        #region  CONSTRUCTOR
        public FHIRServiceExecutor(
            IFHIRServiceFactory FHIRServiceFactory,
            ILogger<FHIRServiceExecutor> logger,
            IAzureKeyVaultService keyVaultService)
        {
            _FHIRServiceFactory = FHIRServiceFactory;
            _logger = logger;
            _keyVaultService = keyVaultService;
        }
        #endregion

        #region PUBLIC METHODS
        public async Task ExecuteAsync()
        {
            try
            {
                var authToken = await GetGoogleAuthToken();

                await SyncResourceAsync(authToken);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
            
        }

        public async Task<HttpResponseMessage> RetriveDataFromFHIRStore(string resoureceType,string resoureceId)
        {
            try
            {
                var authToken = await GetGoogleAuthToken();
                var service   = _FHIRServiceFactory.GetFHIRService(resoureceType);
                var result    = await   service.GetDataFromFHIRStoreByResourceId(authToken, resoureceId);               
               
                return result;
               
                
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }
        #endregion

        #region PRIVATE METHODS
        private async Task<string> GetGoogleAuthToken()
        {
            try
            {
                var googleCredentialsJson = await _keyVaultService.GetSecretAsync("GoogleCredentials");

                var token = await GetAccessTokenFromJSONKeyAsync(googleCredentialsJson,
                    "https://www.googleapis.com/auth/userinfo.profile");
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong. Please refer to the exception details.");
                throw;
            }
        }
        public  async Task<string> GetAccessTokenFromJSONKeyAsync(string jsonKeyFilePath, params string[] scopes)
        {

            var credentialPath = Path.Combine(AppContext.BaseDirectory, jsonKeyFilePath);
            using (var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
            {
                return await GoogleCredential
                    .FromStream(stream) // Loads key file
                    .CreateScoped(new[]{
                              "https://www.googleapis.com/auth/cloud-healthcare",
                               "https://www.googleapis.com/auth/cloud-platform"
        }) // Gathers scopes requested
                    .UnderlyingCredential // Gets the credentials
                    .GetAccessTokenForRequestAsync(); // Gets the Access Token
            }
        }

        private async Task SyncResourceAsync(string authToken) 
        {
            try
            {
                foreach (var resource in FHIRResourceTypes.FhirResourceTypesDict)
                {
                    var fhirService = _FHIRServiceFactory.GetFHIRService(resource.Value);

                    await fhirService.InsertResourceAsync(authToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }  
        }
        #endregion
    }

}
   

   




