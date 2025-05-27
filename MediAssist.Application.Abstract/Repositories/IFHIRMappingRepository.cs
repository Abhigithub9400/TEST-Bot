using MediAssist.DbContext;

namespace MediAssist.Application.Abstract.Repositories
{
    public interface IFHIRMappingRepository
    {
        Task SaveDataToFHIRMapping(HttpResponseMessage response, string recordId, string resourceType);
    }
}
