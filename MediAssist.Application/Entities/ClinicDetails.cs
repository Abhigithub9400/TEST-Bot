using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Entities
{
    public class ClinicDetails : IClinicDetails
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string ClinicName { get; set; }

        [Required]
        public string ClinicAddress { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string CountryCode { get; set; }

        public string? Email { get; set; }

        public string? Website { get; set; }

        [Required]
        public string Logo { get; set; }
    }
}
