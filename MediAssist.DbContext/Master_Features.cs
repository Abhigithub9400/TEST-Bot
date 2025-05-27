using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.DbContext
{
    public class Master_Features
    {

        [Key]
        public long Id { get; set; } // Primary Key

        public string FeatureName { get; set; }

        public bool IsActive { get; set; }
        public string Value { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; } // Nullable if not always set

        public string? ModifiedBy { get; set; }
    }
}
