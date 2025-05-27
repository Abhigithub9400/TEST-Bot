namespace MediAssist.Application.Abstract.Services
{
    public interface IFHIRServiceExecutor
    {
        Task ExecuteAsync();

        Task<HttpResponseMessage> RetriveDataFromFHIRStore(string resoureceType, string resoureceTypeId);
    }
}
