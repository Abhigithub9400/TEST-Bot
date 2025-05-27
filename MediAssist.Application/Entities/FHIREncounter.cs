using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Entities
{

    public class FHIREncounter
    {
        public string resourceType { get; set; }
        public string Id { get; set; }
        public string Status { get; set; }
        public EncounterClass Class { get; set; }
        public Subject Subject { get; set; }
        public List<Participant> Participant { get; set; }
    }

    public class EncounterClass
    {
        public string System { get; set; }
        public string Code { get; set; }
        public string Display { get; set; }
    }

    public class Subject
    {
        public string Reference { get; set; }
    }

    public class Participant
    {
        public Individual Individual { get; set; }
    }

    public class Individual
    {
        public string Reference { get; set; }
        public string Display { get; set; }
    }

}
