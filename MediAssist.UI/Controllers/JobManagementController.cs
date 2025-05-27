using Azure;
using Hangfire;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediAssist.UI.Controllers
{
    [Route("api/JobManagement")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class JobManagementController : ControllerBase
    {
        #region PRIVATE FIELD

        private readonly IPlanExpiryHandlerService _planExpiryHandlerService;
        private readonly ILogger<JobManagementController> _logger;
        private readonly IFHIRServiceExecutor _FHIRServiceExecutor;

        #endregion

        #region CONSTRUCTOR
        public JobManagementController(IPlanExpiryHandlerService planExpiryHandlerService , ILogger<JobManagementController> logger,  IFHIRServiceExecutor FHIRServiceExecutor)
        {
            _planExpiryHandlerService = planExpiryHandlerService;   
            _logger = logger;
            _FHIRServiceExecutor = FHIRServiceExecutor; 
        }
        #endregion

        #region PUBLIC METHODS 
        [Route("CreatePlanExpiryRecurringJob")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePlanExpiryRecurringJob()
        {
            try
            {
                RecurringJob.AddOrUpdate("PlanExpiryRecurringJob",() => _planExpiryHandlerService.HandleExpiredPlansAsync(), Cron.Daily);                
                return Ok();
            }
            catch (Exception ex) {
                _logger.LogError("somthing went wrong");
                throw;
            }
        }

       

        [Route("createFhirDataSyncJob")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CreateFhirDataSyncJob()
        {
            try
            {
                RecurringJob.AddOrUpdate("FhirDataSyncJob", () => _FHIRServiceExecutor.ExecuteAsync(), Cron.Daily);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("somthing went wrong");
                throw;
            }
        }


        
        #endregion  
    }
}
