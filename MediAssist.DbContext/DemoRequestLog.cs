namespace MediAssist.DbContext
{
    public class DemoRequestLog
    {
        public int Id { get; set; }

        public string RequesterName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string? Requirements { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
