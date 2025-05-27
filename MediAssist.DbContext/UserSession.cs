using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.DbContext
{
    public class UserSession
    {
        [Key]
        public long Id { get; set; }

        public long SessionId { get; set; }

        public string UserId { get; set; }

        public int FeaturePlanId { get; set; }

        public DateTime SessionStartTime { get; set; }

        public int SessionVersion { get; set; }

        public bool SessionExpired { get; set; }

        public bool ReportGenerated { get; set; }

        public TimeSpan SessionRemainingTime { get; set; }

        public DateTime CreatedDate { get; set; }

        [MaxLength(255)]
        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [MaxLength(255)]
        public string ModifiedBy { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(FeaturePlanId))]
        public FeaturePlanConfiguration FeaturePlanConfiguration { get; set; }

        public int TotalToken { get; set; }

        [Column(TypeName = "decimal(10,5)")]
        public decimal TotalCost { get; set; } 

        public bool IsPotentialDiagnosisOn { get; set; }
    }
}
