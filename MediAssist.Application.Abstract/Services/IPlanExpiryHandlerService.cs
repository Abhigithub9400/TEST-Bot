using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Services
{
    public interface IPlanExpiryHandlerService
    {
        public Task HandleExpiredPlansAsync();
    }
}
