using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.Application.Services;
using MediAssist.DbContext;
using MediAssist.UI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MediAssist.UI.Controllers
{
    [Route ("api/feedback")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FeedbackController : ControllerBase
    {
        #region PRIVATE FIELD

        private readonly IFeedbackService _feedbackService;
        private readonly ILogger<FeedbackController> _logger;


        #endregion

        #region CONSTRUCTOR

        public FeedbackController(IFeedbackService feedbackService, ILogger<FeedbackController> logger)
        {
            _feedbackService = feedbackService;
            _logger = logger;
        }

        #endregion

        #region PUBLIC METHODS

        [HttpPost("addfeedback")]
        public async Task<IActionResult> AddFeedback(FeedbackViewModel feedbackViewModel)
        {
            try
            {
                var result = await _feedbackService.SubmitFeedback(feedbackViewModel);
                
                if (result == HttpStatusCode.OK)
                {
                    return Ok(new { success = true });
                }

                return BadRequest(new {success = false, message = "Error submitting feedback"});
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        #endregion
    }
}
