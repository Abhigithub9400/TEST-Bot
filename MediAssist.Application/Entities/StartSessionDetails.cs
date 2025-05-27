using MediAssist.Application.Abstract.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Entities
{
    public class StartSessionDetails : IStartSessionDetails
    {
        public string UserId { get; set; }

        public int SessionId { get; set; }

        public int TotalToken { get; set; }

        public decimal TotalCost { get; set; }

        public bool IsPotentialDiagnosisOn { get; set; }
    }
}
