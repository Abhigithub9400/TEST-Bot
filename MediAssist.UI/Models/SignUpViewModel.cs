using System.ComponentModel.DataAnnotations;

namespace MediAssist.UI.Models
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Agree the Terms and Privacy.")]
        public bool TermsAndPrivacy { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public int Title { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Medical Credentials is required.")]
        public int MedicalCredentials { get; set; }

        [Required(ErrorMessage = "Specialization is required.")]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "Agree the License Agreement.")]
        public bool LicenseAgreement { get; set; }

    }
}
