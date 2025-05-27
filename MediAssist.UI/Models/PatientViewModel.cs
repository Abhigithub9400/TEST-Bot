using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MediAssist.UI.Models
{
    public class PatientViewModel
    {
        public required string MrnNumber { get; set; }

        public required string PatientName { get; set; }

        public required string Gender { get; set; }

        public required string Age { get; set; }

        public required string DOB { get; set; }

        public string? UserId { get; set; }

        public int? ClinicId { get; set; }

        public int? PatientId { get; set; }

        public bool PatientConsent { get; set; }
    }
}
