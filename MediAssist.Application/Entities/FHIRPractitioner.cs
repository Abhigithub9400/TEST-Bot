using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Entities
{
    public class FHIRPractitioner
    {
        public string resourceType { get; set; }
        public string Id { get; set; }
        public Name[] Name { get; set; }
    }
}
