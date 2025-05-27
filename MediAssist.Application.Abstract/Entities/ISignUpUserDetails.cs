using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Entities
{
    public interface ISignUpUserDetails
    {
        string FullName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        bool TermsAndPrivacy { get; set; }
        int Title { get; set; }
        int Gender { get; set; }
        DateTime DateOfBirth { get; set; }
        int MedicalCredentials { get; set; }
        string Specialization { get; set; }
        bool LicenseAgreement { get; set; }
        bool IsActive { get; set; }
    }
}
