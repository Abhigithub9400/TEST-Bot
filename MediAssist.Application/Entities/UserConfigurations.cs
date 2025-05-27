using MediAssist.Application.Abstract.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Entities
{
    public class UserConfigurations : IUserConfigurations
    {
        public int Transcriptions { get; set; }

        public long AvailableHours { get; set; }

        public long SessionDurationLimit { get; set; }

        public bool RealTimeResults { get; set; }

        public bool PriorityAccessToTheLatestModels { get; set; }

        public bool EarlyAccessToNewAIFeatures { get; set; }

        public bool GenerateDocumentsWithConfidence { get; set; }

        public bool WatermarkRemoval { get; set; }

        public bool TailoredCapabilitiesAndAdvancedSupport { get; set; }

        public int UserSessionsCount { get; set; }
    }
}
