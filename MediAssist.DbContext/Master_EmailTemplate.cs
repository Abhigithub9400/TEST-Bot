using System.ComponentModel.DataAnnotations;

namespace MediAssist.DbContext
{
    public class Master_EmailTemplate
    {
        [Key]
        public long Id { get; set; } // Primary Key

        public string Identifier { get; set; }

        public string Subject { get; set; }

        public string PlainTextBody { get; set; }

        public string HTMLBody { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; } 

        public string? ModifiedBy { get; set; }
    }
}
