using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Entities
{
    public interface IClinicDetails
    {
        string UserId { get; set; }

        string ClinicName { get; set; }

        string? ClinicAddress { get; set; }

        string PhoneNumber { get; set; }

        string CountryCode { get; set; }

        string? Email { get; set; }

        string? Website { get; set; }

        string Logo { get; set; }

    }
}
