using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.DbContext
{
    public class Master_Plans
    {
        [Key]
        public int Id { get; set; }

        public string PlanTitle { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; } // Nullable if not always set

        public string? ModifiedBy { get; set; }
    }
}

