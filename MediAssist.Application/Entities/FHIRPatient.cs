using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Entities
{
    public class FHIRPatient
    {
        public string resourceType { get; set; }
        public string Id { get; set; }
        public Name[] Name { get; set; }
        public string Gender { get; set; }
        public ManagingOrganization ManagingOrganization { get; set; }

    }

    public class Name
    {
        public string Family { get; set; }
        public string[] Given { get; set; }
    }

    public class ManagingOrganization
    {
        public string Reference { get; set; }
    }
}
