using MediAssist.Application.Abstract.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Entities
{
    public class PatientDetails : IPatientDetails
    {
       
        public required string MrnNumber { get; set; }

        public required string PatientName { get; set; }

        public required string Gender { get; set; }

        public required string Age { get; set; }

        public required string DOB {  get; set; }

        public  required string UserId { get; set; }

        public int? ClinicId { get; set; }

        public int? PatientId { get; set; }

        public bool PatientConsent { get; set; }



    }
}
