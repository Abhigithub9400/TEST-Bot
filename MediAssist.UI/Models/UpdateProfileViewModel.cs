using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Reflection.Metadata;
using System.Threading;

namespace MediAssist.UI.Models
{
    public class UpdateProfileViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        public int Title { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DOB { get; set; }

        public string? Image { get; set; }

        public string? LicenseNumber { get; set; }

        [Required(ErrorMessage = "MedicalCredentials is required.")]
        public int MedicalCredentials { get; set; }

        [Required(ErrorMessage = "Specialization is required.")]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }
    }
}
