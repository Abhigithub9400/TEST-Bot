using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Entities
{
    public interface IPatientDetails
    {
         string MrnNumber { get; set; }

         string PatientName { get; set; }

         string Gender { get; set; }

         string Age { get; set; }

         string DOB { get; set; }

         string UserId { get; set; }

         int? ClinicId { get; set; }

         int? PatientId { get; set; }

         bool PatientConsent { get; set; }
    }
}
