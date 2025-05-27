using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.UI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MediAssist.UI.Controllers
{

    [Route("api/Session")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SessionController : ControllerBase
    {
        #region private variables
        private readonly IUserSessionService _userSessionService;
        private readonly ILogger<SessionController> _logger;

        #endregion

        #region constructor 
        public SessionController(IUserSessionService userSessionService, ILogger<SessionController> logger)
        {
            _userSessionService = userSessionService;
            _logger = logger;
        }
        #endregion

        #region Public Methods
        [HttpPost("startusersession")]
        public IActionResult StartOrResumeSession([FromBody] UserSessionViewModel userSessionViewModel)
        {
            try
            {
                if (userSessionViewModel.UserId.IsNullOrEmpty())
                {
                    throw new ArgumentException("userId Or  sessionId is not correct");
                }

                var startSessionDetails = new StartSessionDetails()
                {
                    UserId = userSessionViewModel.UserId,
                    SessionId = userSessionViewModel.SessionId,
                    TotalCost = userSessionViewModel.TotalCost,
                    TotalToken = userSessionViewModel.TotalToken,
                    IsPotentialDiagnosisOn = userSessionViewModel.IsPotentialDiagnosisOn
                };
                var result = _userSessionService.StartOrResumeSession(startSessionDetails);

                return Ok(result.Result);
            }
            catch (Exception ex) {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return BadRequest(new { success = false, message = ex.Message });

            }
        }

        [HttpPost("stopusersession")]
        public IActionResult StopUserSession([FromBody] UserSessionViewModel userSessionViewModel)
        {
            try
            {
                if(userSessionViewModel.UserId.IsNullOrEmpty() || userSessionViewModel.SessionId == 0)
                {
                    throw new ArgumentException("userId Or  sessionId is not correcr");
                }
                var result =    _userSessionService.StopUserSession(userSessionViewModel.SessionId, userSessionViewModel.UserId, userSessionViewModel.TotalToken, userSessionViewModel.TotalCost);

                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return BadRequest(new { success = false, message = ex.Message });

            }

        }
        [HttpPost("updateusersession")]
        public IActionResult UpdateUserSession([FromBody] UserSessionViewModel userSessionViewModel)
        {
            try
            {
                if (userSessionViewModel.UserId.IsNullOrEmpty() || userSessionViewModel.SessionId == 0)
                {
                    throw new ArgumentException("userId Or  sessionId is not correcr");
                }
                var result = _userSessionService.UpdateUserSession(userSessionViewModel.SessionId,  userSessionViewModel.UserId, userSessionViewModel.TotalToken, userSessionViewModel.TotalCost);
                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details: {Exception}", ex.Message, ex);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("reportgenerated")]
        public IActionResult ReportGenerated([FromBody] UserSessionViewModel userSessionViewModel)
        {
            try
            {
                if (userSessionViewModel.UserId.IsNullOrEmpty() || userSessionViewModel.SessionId == 0)
                {
                    throw new ArgumentException("userId Or  sessionId is not correcr");
                }
                var result = _userSessionService.ReportGenerated(userSessionViewModel.SessionId, userSessionViewModel.UserId, userSessionViewModel.TotalToken, userSessionViewModel.TotalCost);
                return Ok(result.Result);
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
