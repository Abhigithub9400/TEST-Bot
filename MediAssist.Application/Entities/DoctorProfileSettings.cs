using MediAssist.Application.Abstract.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Entities
{
    public class DoctorProfileSettings : IDoctorProfileSettings
    {
        [Required]
        public string UserId{ get; set; }

        public int MedicalCredentials { get; set; }

        [Required]
        public string Specialization { get; set; }

        [Required]
        public string Signature { get; set; }
    }
}
