namespace MediAssist.UI.Models
{
    public class UserConfigurationsViewModel
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
