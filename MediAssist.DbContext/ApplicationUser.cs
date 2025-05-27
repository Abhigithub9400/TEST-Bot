using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.DbContext
{
    public class ApplicationUser :IdentityUser
    {
        public string  FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public bool TermsAndPrivacy { get; set; }

        public int ForgotPasswordAttemptCount { get; set; }

        public DateTime? LastForgotPasswordAttemptTimestamp { get; set; }

        public override string Id { get; set; }

        public bool LicenseAgreement { get; set; }


        public int GenerateReportCount { get; set; }

        public bool IsUpdatedtoFHIR { get; set; }
    }
}
