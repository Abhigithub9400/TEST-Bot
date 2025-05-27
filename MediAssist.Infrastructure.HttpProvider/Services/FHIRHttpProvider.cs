using MediAssist.Infrastructure.HttpProvider.Services.Abstract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace MediAssist.Infrastructure.HttpProvider.Services
{
    class FHIRHttpProvider : IFHIRHttpProvider
    {
        #region   PRIVATE FIELDS
        private readonly ILogger<FHIRHttpProvider> _logger;
        private readonly HttpClient _httpClient;

        #endregion

        #region  CONSTRUCTOR
        public FHIRHttpProvider(ILogger<FHIRHttpProvider>  logger, HttpClient httpClient)
        {
            _logger = logger;   
            _httpClient = httpClient;
        }
        #endregion

        #region  PUBLIC METHODS
        public async Task<HttpResponseMessage> AddAsync(string record, string authToken, string url)
        {
            try
            {
                
                var content = new StringContent(record, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);                

                var response = await _httpClient.PostAsync(url, content);

                if (response.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(errorContent);

                    var fhirError = json["issue"]?[0]?["diagnostics"]?.ToString();

                    var item = string.IsNullOrEmpty(fhirError) ? "Detailsare not Provided" : fhirError;

                    _logger.LogError("Something went wrong. Please refer to the exception details. {0} ", errorContent);

                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(item, Encoding.UTF8, "text/plain")
                    };
                }

                return response;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Something went wrong. Please refer to the exception details.");
                throw;
            }
        }
        public async Task<HttpResponseMessage> GetAsync(string record, string authToken, string url)
        {
            try
            {

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                var requestUrl = $"{url}/{record}";

                var response = await _httpClient.GetAsync(requestUrl);


                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(errorContent);

                    // Safely extract diagnostics
                    var fhirError = json["issue"]?[0]?["diagnostics"]?.ToString();

                    var item = string.IsNullOrEmpty(fhirError) ? "Detailsare not Provided" : fhirError;                    

                    _logger.LogError( "Something went wrong. Please refer to the exception details. {0} ", errorContent);

                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(item, Encoding.UTF8, "text/plain")
                    };
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong. Please refer to the exception details.");
                throw;
            }
        }
        public Task<HttpResponseMessage> DeleteAsync(string record, string authToken, string url)
        {
            throw new NotImplementedException();
        }

       

        public Task<HttpResponseMessage> UpdateAsync(string record, string authToken, string url)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
  



}
