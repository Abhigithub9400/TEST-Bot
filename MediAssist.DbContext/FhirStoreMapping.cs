using System.ComponentModel.DataAnnotations;

namespace MediAssist.DbContext
{
    public class FHIRStoreMapping
    {
        [Key]
        public int Id { get; set; }

        public string EntityId { get; set; }

        public string FHIRResourceId { get; set; }

        [MaxLength(255)]
        public string CreatedBy { get; set; }

        [MaxLength(255)]
        public string ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        [MaxLength(255)]
        public string ResourceType { get; set; }
    }
}
