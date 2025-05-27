using MediAssist.Application.Abstract.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Services
{
    public interface IPatientService
    {
        Task<IPatientResponse> InsertPatient(IPatientDetails patientDetails);

        Task<IPatientResponse> UpdatePatient(IPatientDetails patientDetails);
    }
}
