using MediAssist.Application.Abstract.Entities;
using System.Net;

namespace MediAssist.Application.Entities
{
    public class PatientResponse : IPatientResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public  int? PatientId { get; set; }

        public string ErrorMessage { get; set; }

    }
}