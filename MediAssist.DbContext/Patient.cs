using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.DbContext
{
    public class Patient
    {
        public int PatientId { get; set; }

        public required string MrnNumber { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Gender { get; set; }

        public required string Age { get; set; }

        public required string DOB { get; set; }

        public int? ClinicId { get; set; }
       

        public bool IsUpdatedtoFHIR { get; set; }

        public required string CreatedBy { get; set; }

        public required string ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool PatientConsent { get; set; }



    }
}
