using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.DbContext
{
    [Table("DoctorProfiles")]
    public class DoctorProfile
    {
        public int Id { get; set; }

        public int Title { get; set; } // Foreign Key from UserTitle
        public UserTitle UserTitle { get; set; }

        public DateTime DOB { get; set; }

        public int Gender { get; set; } // Foreign Key from Gender table
        public UserGender UserGender { get; set; }

        public byte[]? Image { get; set; }

        public string? LicenseNumber { get; set; }

        public int MedicalCredentials { get; set; } // Foreign Key from MedicalCredentials table
        public DoctorMedicalCredentials MedicalCredential { get; set; }

        public string Specialization { get; set; }

        public string UserId { get; set; } // Foreign Key from User table
        public ApplicationUser User { get; set; } // Navigation Property

        public int? ClinicId { get; set; }
        public virtual Clinic? Clinic { get; set; }

        public string? PrivacyNotice { get; set; }

        public string? TermsOfUse { get; set; }

        public string? CopyRightInformation { get; set; }

        public string? ConsultationDisclaimer { get; set; }
        public byte[]? Signature { get; set; }

    }

    public class UserTitle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Abbreviations { get; set; }
    }

    public class UserGender
    {
        public int Id { get; set; }
        public string Gender { get; set; }
    }

    public class DoctorMedicalCredentials
    {
        public int Id { get; set; }
        public string MedicalCredentials { get; set; }
    }
}