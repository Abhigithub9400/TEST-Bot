using MediAssist.Application.Abstract.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Entities
{
    public class UserSessionDetails : IUserSessionDetails
    {
        public long SessionId { get; set; }

        [Required]
        public string UserId { get; set; }

        public int FeaturePlanId { get; set; }

        public DateTime StartedTime { get; set; }

        public int SessionVersion { get; set; }

        public bool SessionExpired { get; set; }

        public bool ReportGenerated { get; set; }

        public TimeSpan RemainingTime { get; set; }
    }
}
