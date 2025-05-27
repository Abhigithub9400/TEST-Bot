using MediAssist.Application.Abstract.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Entities
{
    public class UpdateUserDetails : IUpdateUserDetails
    {
        public int Title { get; set; }

        public int Gender { get; set; }

        public DateTime DOB { get; set; }

        public string? Image { get; set; }

        public string? LicenseNumber { get; set; }

        public int MedicalCredentials { get; set; }

        [Required]
        public string Specialization { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
