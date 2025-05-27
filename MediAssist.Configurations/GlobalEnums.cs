namespace MediAssist.Configurations
{
    public static class GlobalEnums
    {
        public enum ConfigurableFeatures
        {
            Transcriptions = 1,
            AvailableHours = 2,
            SessionDurationLimit = 3,
            RealtimeResults = 4,
            PriorityAccessToTheLatestModels = 5,
            EarlyAccessToNewAIFeatures = 6,
            GeneratedocumentsWithConfidence = 7,
            WatermarkRemoval = 8,
            TailoredcapabilitiesAndAdvancedsupport = 9
        }

        public static class EmailIdentifiers
        {
            public const string ResetRequest = "MEDI_1_1_E1";
            public const string RequestDemo = "MEDI_2_1_E1";
            public const string GenericEnquiry = "MEDI_2_1_E2";
            public const string ContactUsForSubscriptionForRegisteredUserTemplate = "MEDI_3_1_E1";
            public const string ContactUsForSubscriptionForGuestUserTemplate = "MEDI_3_1_E2";
            public const string ThankyouMailToUserOnPricingEnquiry = "MEDI_3_1_E3";
            public const string ShareReport = "MEDI_4_1_E1";
        }
       

        public static class FHIRResourceTypes {
            public const string Patient = "Patient";
            public const string Doctor = "Practitioner";
            public const string Encounter = "Encounter";

            public const string encounterClassSystem = "http://terminology.hl7.org/CodeSystem/v3-ActCode";
            public const string encounterClassCode = "AMB";
            public const string encounterClassDisplay = "ambulatory";
            public const string encounterStatus = "finished";

            public static  Dictionary<string, string> FhirResourceTypesDict = new Dictionary<string, string>
            {
                    { "patient", "Patient" },
                    {"practitioner", "Practitioner"},
                    {"encounter", "Encounter" }
            };
        }

        //public static class Encryption
        //{
        //    public static string Key { get; set; } = string.Empty;
        //    public static string IV { get; set; } = string.Empty;

        //    public const string KeyName = "";
        //    public const string IVName = "";
        //}


        public static class Gender {
            public const string Male = "Male";
            public const string Female = "Female";
            public const string Transgender = "Transgender";
            public const string Nonbinary = "Non-binary";
            public const string PreferNotToSay = "Prefer Not to Say";
            public const string Other = "other";
            public const string Unknown = "unknown";
        }
    }
}
