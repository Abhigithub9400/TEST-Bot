using MediAssist.Application.Abstract.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Entities
{
    public class ReportData : IReportData
    {
        [Required]
        public string DoctorName { get; set; }

        [Required]
        public string DoctorSpecialization { get; set; }

        [Required]
        public string DoctorTitle { get; set; }

        [Required]
        public string DoctorSignature { get; set; }

        [Required]
        public string HospitalName { get; set; }

        [Required]
        public string HospitalAddress { get; set; }

        [Required]
        public string HospitalLogo { get; set; }

        public int ClinicId { get; set; }
    }
}
