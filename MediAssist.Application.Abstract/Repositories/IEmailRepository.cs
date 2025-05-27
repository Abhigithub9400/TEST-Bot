using MediAssist.DbContext;

namespace MediAssist.Application.Abstract.Repositories
{
    public interface IEmailRepository
    {
        Task<Master_EmailTemplate> Get_EmailTemplate(string emailIdentifier);
    }
}
