using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Entities
{
    public interface IServiceResponse<T>
    {
        bool Success { get; set; }
        string Message { get; set; }
        T Data { get; set; }
    }
}
