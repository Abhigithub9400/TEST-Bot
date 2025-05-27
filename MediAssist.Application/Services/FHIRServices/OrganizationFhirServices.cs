using MediAssist.Application.Abstract.Services;

namespace MediAssist.Application.Services.FHIRServices
{
    public class OrganizationFHIRServices : IFHIRService
    {

        public Task<HttpResponseMessage> GetDataFromFHIRStoreByResourceId(string authToken, string resourceId)
        {
            throw new NotImplementedException();
        }


        public Task InsertResourceAsync(string authToken)
        {
            throw new NotImplementedException();
        }
    }
}
