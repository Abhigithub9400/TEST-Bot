namespace MediAssist.UI.Models
{
    public class UserSessionViewModel
    {
        public string UserId { get; set; }

        public int SessionId { get; set; } = 0;

        public int TotalToken { get; set; }

        public decimal TotalCost { get; set; }

        public bool IsPotentialDiagnosisOn { get; set; }
    }
}
