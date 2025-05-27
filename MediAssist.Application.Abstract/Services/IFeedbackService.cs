using MediAssist.Application.Abstract.Entities;
using MediAssist.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Services
{
    public interface IFeedbackService
    {
        Task<HttpStatusCode> SubmitFeedback(MediAssist.Application.Abstract.Entities.FeedbackViewModel feedbackData);
    }
}
