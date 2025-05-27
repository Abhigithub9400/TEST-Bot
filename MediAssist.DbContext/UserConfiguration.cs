using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.DbContext
{
    public class UserConfiguration
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }

        public int Transcriptions { get; set; }

        public long AvailableHours { get; set; }

        public long SessionDurationLimit { get; set; }

        public bool RealTimeResults { get; set; }

        public bool PriorityAccessToTheLatestModels { get; set; }

        public bool EarlyAccessToNewAIFeatures { get; set; }

        public bool GenerateDocumentsWithConfidence { get; set; }

        public bool WatermarkRemoval { get; set; }

        public bool TailoredCapabilitiesAndAdvancedSupport { get; set; }

        public int TotalNumberOfDays { get; set; } = 0;

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? ModifiedDate { get; set; }

        [MaxLength(255)] // Adjust as needed
        public string? ModifiedBy { get; set; }

        [ForeignKey(nameof(UserId))]
        public  ApplicationUser User { get; set; }
    }
}
