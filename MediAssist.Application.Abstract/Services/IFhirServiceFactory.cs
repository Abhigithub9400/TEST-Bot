namespace MediAssist.Application.Abstract.Services
{
    public interface IFHIRServiceFactory
    {
        IFHIRService GetFHIRService(string resourceType);
    }
}
