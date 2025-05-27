using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.DbContext
{
    public class DoctorPatientMapping
    {
        public int Id { get; set; }

        public int Patient_id { get; set; }

        public string Practitioner_id { get; set; }

        public bool IsUpdatedtoFHIR { get; set; }

        public required string CreatedBy { get; set; }

        public required string ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
