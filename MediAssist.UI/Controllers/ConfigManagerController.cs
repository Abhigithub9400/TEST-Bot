using MediAssist.Infrastructure.Abstract.Configurations;
using MediAssist.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MediAssist.UI.Controllers
{
    [Route("api/Config")]
    [ApiController]
    public class ConfigManagerController : ControllerBase
    {
        #region PRIVATE FIELD
        private readonly IAppSettings _appSettings;
        private readonly ILogger<UsersController> _logger;
        #endregion

        #region CONSTRUCTOR

        public ConfigManagerController(IAppSettings appSettings, ILogger<UsersController> logger)
        {
            _appSettings = appSettings;
            _logger = logger;
        }
        #endregion

        #region PUBLIC METHODS
        [HttpGet("get")]
        public async Task<IActionResult> GetMediAssistConfigurations()
        {
            try
            {
                var mdiAssistConfigManager = new MediAssistConfigManager();

                mdiAssistConfigManager.DomainName = _appSettings.MediAssistDomainName;
                mdiAssistConfigManager.SupportEmail = _appSettings.MediAssistSupportEmail;
                return Ok(mdiAssistConfigManager);

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        #endregion
    }
}
