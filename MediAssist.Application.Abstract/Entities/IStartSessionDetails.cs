using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Entities
{
    public interface IStartSessionDetails
    {
        public string UserId { get; set; }

        public int SessionId { get; set; }

        public int TotalToken { get; set; }

        public decimal TotalCost { get; set; }

        public bool IsPotentialDiagnosisOn { get; set; }
    }
}
