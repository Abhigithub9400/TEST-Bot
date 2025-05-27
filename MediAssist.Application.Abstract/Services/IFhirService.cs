namespace MediAssist.Application.Abstract.Services
{
    public  interface IFHIRService
    {
        Task InsertResourceAsync(string authToken);

        Task<HttpResponseMessage> GetDataFromFHIRStoreByResourceId(string authToken, string resourceId);
    }
}
