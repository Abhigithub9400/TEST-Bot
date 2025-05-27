using MediAssist.Application.Abstract.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Entities
{
    public class LoginUserDetails : ILoginUserDetails
    {
        [Required]
        public string UserId { get; set; }

        public string? FullName { get; set; }

        public string? FirstName { get; set; }

        public string? Specialization { get; set; }

        public string? Image { get; set; }

        public string? TitleAbbreviation { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public bool IsSettingsUpdated { get; set; }
    }
}
