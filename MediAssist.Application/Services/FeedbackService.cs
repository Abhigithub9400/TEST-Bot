using Mapster;
using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static MediAssist.Application.Abstract.Entities.IFeedbackData;

namespace MediAssist.Application.Services
{
    public class FeedbackService : IFeedbackService
    {
        #region PRIVATE FIELD

        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<FeedbackService> _logger;


        #endregion

        #region CONSTRUCTOR

        public FeedbackService(IUserRepository userRepository, UserManager<ApplicationUser> userManager,ILogger<FeedbackService> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _logger = logger;
        }

        #endregion

        #region PUBLIC METHODS

        public async Task<HttpStatusCode>SubmitFeedback(FeedbackViewModel feedbackData)
        {
            if (feedbackData == null)
            {
                return HttpStatusCode.BadRequest;
            }

            var user = await _userManager.FindByIdAsync(feedbackData.UserId);
            if (user == null)
            {
                return HttpStatusCode.NotFound;
            }

            try
            {
                var feedback = CreateFeedbackEntity(user, feedbackData);
                var result = await _userRepository.AddFeedbackAsync(feedback);

                if (result.Id == 0)
                {
                    return HttpStatusCode.InternalServerError;
                }

                await CreateFeedbackCategoryMappingsAsync(result.Id, feedbackData);
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred. See exception details:");
                return HttpStatusCode.InternalServerError;
            }
        }

        private Feedback CreateFeedbackEntity(ApplicationUser user, FeedbackViewModel feedbackData)
        {
            return new Feedback
            {
                UserId = user.Id,
                EmailAddress = user.Email ?? string.Empty,
                CreatedDate = DateTime.UtcNow,
                FeedbackRating = feedbackData.Rating,
                IssueDescription = feedbackData.IssueDescription,
                ImprovementSuggestions = feedbackData.SuggestionsImprovement
            };
        }

        private async Task CreateFeedbackCategoryMappingsAsync(int feedbackId, FeedbackViewModel feedbackData)
        {
            string categoryText = feedbackData.CustomCategoryText;

            var mappingTasks = feedbackData.CategoryIDs.Select(categoryId =>
                CreateAndSaveCategoryMappingAsync(feedbackId, categoryId, categoryText));

            await Task.WhenAll(mappingTasks);
        }

        private async Task CreateAndSaveCategoryMappingAsync(int feedbackId, int categoryId, string categoryText)
        {
            var mappingData = new FeedbackCategoryMapping
            {
                FeedbackID = feedbackId,
                CreatedDate = DateTime.UtcNow,
                CategoryID = categoryId,
                CustomCategoryText = categoryId == 5 ? categoryText : null
            };

            var mappingDetails = await _userRepository.AddCategoryFeedbackAsync(mappingData);

            if (mappingDetails == null)
            {
                throw new InvalidOperationException("Failed to create category mapping");
            }
        }

        #endregion
    }
}