using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Entities
{
    public interface IPatientResponse
    {
        HttpStatusCode StatusCode { get; set; }

        int? PatientId { get; set; }

        string ErrorMessage { get; set; }
    }
}
