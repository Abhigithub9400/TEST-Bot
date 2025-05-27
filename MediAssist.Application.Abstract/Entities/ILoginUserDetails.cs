using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Entities
{
    public interface ILoginUserDetails
    {
        string UserId { get; set; }

        string? FullName { get; set; }

        string? FirstName { get; set; }

        string? Specialization { get; set; }

        string? Image { get; set; }

        string? TitleAbbreviation { get; set; }

        HttpStatusCode HttpStatusCode { get; set; }

        bool IsSettingsUpdated {  get; set; }
    }
}
