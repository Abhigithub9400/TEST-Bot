using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediAssist.DbContext
{
    public class Clinic
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(255)]
        [Url]
        public string? Website { get; set; }

        [Required]
        public byte[]? Logo { get; set; }

        [ForeignKey("ClinicId")]
        public ICollection<DoctorProfile> DoctorProfiles { get; set; }
    }
}