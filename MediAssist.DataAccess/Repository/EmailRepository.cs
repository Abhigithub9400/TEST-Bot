using MediAssist.Application.Abstract.Repositories;
using MediAssist.DbContext;
using Microsoft.EntityFrameworkCore;

namespace MediAssist.DataAccess.Repository
{
    public class EmailRepository : IEmailRepository
    {
        #region PRIVATE INSTANCE
        private readonly MediAssistDbContext _context;
        #endregion


        #region CONSTRUCTOR
        public EmailRepository(MediAssistDbContext context)
        {
            _context = context;
        }
        #endregion

        #region PUBLIC METHODS

        public async Task<Master_EmailTemplate ?> Get_EmailTemplate(string emailIdentifier)
        {
            return await _context.Master_EmailTemplates.FirstOrDefaultAsync(x => x.Identifier == emailIdentifier);
        }

        #endregion
    }
}
