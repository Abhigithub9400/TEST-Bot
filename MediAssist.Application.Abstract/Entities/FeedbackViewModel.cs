using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Entities
{
    public class FeedbackViewModel
    {

        public string UserId { get; set; }

        public List<int> CategoryIDs { get; set; }

        public int Rating { get; set; }

        public string CustomCategoryText { get; set; }

        public string IssueDescription { get; set; }

        public string SuggestionsImprovement { get; set; }

    }
}
