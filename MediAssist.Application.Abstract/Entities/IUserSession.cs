using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Entities
{
    public interface IUserSessionDetails
    {
        public long SessionId { get; set; }

        public int SessionVersion { get; set; }
        public bool SessionExpired { get; set; }
        public TimeSpan RemainingTime { get; set; }
    }
}
