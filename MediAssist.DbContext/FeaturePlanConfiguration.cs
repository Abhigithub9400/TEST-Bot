using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.DbContext
{
    public class FeaturePlanConfiguration
    {
        [Key]
        public int Id { get; set; }
        public int PlanId { get; set; }
        public long FeatureId { get; set; }

        public bool IsActive { get; set; }

        public string Value { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }


        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        // Navigation properties for foreign keys
        [ForeignKey(nameof(PlanId))]
        public  Master_Plans Plan { get; set; }

        [ForeignKey(nameof(FeatureId))]
        public  Master_Features Feature { get; set; }
    }
}
