using MediAssist.Application.Abstract.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MediAssist.Configurations.GlobalEnums;

namespace MediAssist.API.Controllers
{
    [Route("api/FHIRResource")]
    [ApiController]
    public class FHIRResourceController : ControllerBase
    {
        #region PRIVATE FIELD
        private readonly ILogger<FHIRResourceController> _logger;
        private readonly IFHIRServiceExecutor _FHIRServiceExecutor;
        #endregion

        #region CONSTRUCTOR
        public FHIRResourceController(ILogger<FHIRResourceController> logger, IFHIRServiceExecutor FHIRServiceExecutor)
        {
            _logger = logger;
            _FHIRServiceExecutor = FHIRServiceExecutor;
        }
        #endregion

        #region PUBLIC METHODS 
        [Route("GetResource/{resourcetype}/{resourceid}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetResource(string resourcetype, string resourceid)
        {
            try
            {
                resourcetype = resourcetype.ToLower();
                
                bool isValidateResourseType = ValidateResourseType(resourcetype);
                if (isValidateResourseType && !string.IsNullOrEmpty(resourceid))
                {
                    //add the validation here for resourcetype
                    var result = await _FHIRServiceExecutor.RetriveDataFromFHIRStore(resourcetype.ToLower(), resourceid);
                    var content = await result.Content.ReadAsStringAsync();
                    return Ok(content);
                }
                else if (!isValidateResourseType)
                {
                    return BadRequest("Please provide a valid resource type.");
                }
                else
                {
                    return BadRequest("Somthing went wrong...");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }

        }
        #endregion

        #region Private Methods
        private bool ValidateResourseType(string resourcetype)
        {
            if (!string.IsNullOrEmpty(resourcetype) && FHIRResourceTypes.FhirResourceTypesDict.ContainsKey(resourcetype))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
