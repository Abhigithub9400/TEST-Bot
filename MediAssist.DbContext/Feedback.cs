using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.DbContext
{
    public class Feedback
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public int FeedbackRating { get; set; }

        public string IssueDescription { get; set; }

        public string ImprovementSuggestions { get; set; }

        public DateTime CreatedDate { get; set; }
        public string EmailAddress { get; set; }


    }

    public class Category
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public DateTime CreatedDate { get; set; }
    }


    public class FeedbackCategoryMapping
    {
        public int Id { get; set; }

        public int FeedbackID { get; set; }

        public Feedback Feedback { get; set; }
        public int CategoryID { get; set; }

        public Category Category { get; set; }

        public string? CustomCategoryText { get; set; }

        public DateTime CreatedDate { get; set; }

    }

}
