using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Entities
{
    public interface IUpdateUserDetails
    {
        int Title { get; set; }

        int Gender { get; set; }

        DateTime DOB { get; set; }

        string? Image { get; set; }

        string? LicenseNumber { get; set; }

        int MedicalCredentials { get; set; }

        string Specialization { get; set; }

        string UserId { get; set; }
    }
}
