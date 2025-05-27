using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediAssist.Application.Abstract.Entities
{
    public interface IFeedbackData
    {
         int FeedbackID { get; set; }
         string UserId { get; set; }

         int Rating { get; set; }

         int FeedbackCategory { get; set; }

         string IssueDescription { get; set; }

         string SuggestionsImprovement { get; set; }

         DateTime CreatedDate { get; set; }


        public interface ICategory
        {
            int CategoryID { get; set; }

            string CategoryName { get; set; }

            DateTime CreatedDate { get; set; }

        }

        public interface ICategoryMapping
        {
            int MappingID { get; set; }

            int FeedbackId { get; set; }

            int FeedbackCategoryId { get; set; }

            string OtherCategoryText { get; set; }

        }
    }
}
