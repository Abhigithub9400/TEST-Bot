using MediAssist.Application.Abstract.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Entities
{
    public class FeedbackData :IFeedbackData
    {
        public int FeedbackID { get; set; }

        [Required]
        public string UserId { get; set; }

        public int Rating { get; set; }

        public  int CategoryID { get; set; }

        public int FeedbackCategory { get; set; }


        [Required]
        public string IssueDescription { get; set; }

        [Required]
        public string SuggestionsImprovement { get; set; }

        public DateTime CreatedDate { get; set; }
       
    }

    
}
