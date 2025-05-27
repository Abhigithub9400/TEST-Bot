using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Entities
{
    public interface IReportData
    {
        string DoctorName { get; }

        string DoctorSpecialization { get; }

        string DoctorTitle {  get; }

        string DoctorSignature { get; }

        string HospitalName { get; }

        string HospitalAddress { get; }

        string HospitalLogo { get; }

        int ClinicId { get; }

    }
}
