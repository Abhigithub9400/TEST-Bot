using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediAssist.UI.Controllers
{
    [Route("api/manage")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentController : ControllerBase
    {
        #region constructor
        public PaymentController()
        {
            
        }
        #endregion

        #region public methods
        [HttpPost("processpayment")]
        public IActionResult ProcessPayment(int planId, string userId)
        {
            return Ok();
        }
        #endregion
    }
}
