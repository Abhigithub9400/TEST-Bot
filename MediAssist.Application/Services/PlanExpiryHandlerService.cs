using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Abstract.Services;
using MediAssist.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Services
{
    public class PlanExpiryHandlerService : IPlanExpiryHandlerService
    {
        #region PRIVATE FIELD
        private readonly MediAssistDbContext _context;
        private readonly ILogger<PlanExpiryHandlerService> _logger;
        #endregion

        #region CONSTRUCTOR
        public PlanExpiryHandlerService(MediAssistDbContext context , ILogger<PlanExpiryHandlerService> logger )
        {
            _logger = logger;
            _context = context; 
        }
        #endregion

        #region PUBLIC METHODS 
        public async Task HandleExpiredPlansAsync()
        {
            try
            {
                // Update the logic after completing the payment gateway integration
                var userConfigs = await _context.UserConfiguration.Where(x =>x.Transcriptions > 0 ).ToListAsync();
                foreach (var userConfig in userConfigs) {
                    if(DateTime.Today >= userConfig.CreatedDate.AddDays(30))
                    {
                        userConfig.Transcriptions = 0;
                        userConfig.AvailableHours = 0;
                        userConfig.WatermarkRemoval = false;
                        userConfig.ModifiedDate = DateTime.Now;
                        userConfig.ModifiedBy = "PlanExpiryHandlerService";
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling expired plans.");
            }
        }

        #endregion
    }
}
