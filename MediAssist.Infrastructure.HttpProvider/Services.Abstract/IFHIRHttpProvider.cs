using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Infrastructure.HttpProvider.Services.Abstract
{
    public interface IFHIRHttpProvider
    {
        Task<HttpResponseMessage> GetAsync(string record, string authToken, string url);
        Task<HttpResponseMessage> AddAsync(string record, string authToken, string url);
        Task<HttpResponseMessage> DeleteAsync(string record, string authToken, string url);
        Task<HttpResponseMessage> UpdateAsync(string record, string authToken, string url);
    }
}
