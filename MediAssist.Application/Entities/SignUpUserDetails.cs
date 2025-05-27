using MediAssist.Application.Abstract.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Entities
{
    public class SignUpUserDetails : ISignUpUserDetails
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool TermsAndPrivacy { get; set; }
        public int Title { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int MedicalCredentials { get; set; }
        [Required]
        public string Specialization { get; set; }
        public bool LicenseAgreement { get; set; }
        public bool IsActive { get; set; }

    }
}
