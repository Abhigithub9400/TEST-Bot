using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Entities
{
    public interface IDoctorProfileSettings
    {
        string UserId { get; set; }

        [Required(ErrorMessage = "MedicalCredentials is required.")]
        int MedicalCredentials { get; set; }

        [Required(ErrorMessage = "Specialization is required.")]
        string Specialization { get; set; }

        string Signature { get; set; }
    }
}
