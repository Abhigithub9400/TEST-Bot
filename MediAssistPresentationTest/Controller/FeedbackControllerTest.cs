using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Services;
using MediAssist.UI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MediAssistPresentationTest.Controller
{
    [TestFixture]
    public class FeedbackControllerTest
    {
        #region PRIVATE INSTANCE FIELD

        private FeedbackController _feedbackController;
        private Mock<IFeedbackService> _mockFeedbackService;
        private Mock<ILogger<FeedbackController>> _mockLogger;

        #endregion

        #region TEST SETUP

        [SetUp]
        public void Setup()
        {
            _mockFeedbackService = new Mock<IFeedbackService>();
            _mockLogger = new Mock<ILogger<FeedbackController>>();
            _feedbackController = new FeedbackController(_mockFeedbackService.Object, _mockLogger.Object);
        }

        #endregion

      
        #region PUBLIC METHODS

        [Test]
        public async Task AddFeedback_WhenSuccessful_ReturnsOkResult()
        {
            // Arrange
            var feedbackViewModel = GetFeedbackdata();
           

            _mockFeedbackService.Setup(x => x.SubmitFeedback(It.IsAny<FeedbackViewModel>()))
                .ReturnsAsync(HttpStatusCode.OK);

            // Act
            var result = await _feedbackController.AddFeedback(feedbackViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult!.Value, Is.Not.Null);
            Assert.That(okResult.Value?.GetType().GetProperty("success")?.GetValue(okResult.Value, null), Is.EqualTo(true));
        }

        [Test]
        public async Task AddFeedback_WhenServiceFails_ReturnsBadRequest()
        {
            //Arrange 
            var feedbackViewModel = GetFeedbackdata();

            _mockFeedbackService.Setup(x => x.SubmitFeedback(It.IsAny<FeedbackViewModel>()))
                .ReturnsAsync(HttpStatusCode.BadRequest);

            // Act
            var result = await _feedbackController.AddFeedback(feedbackViewModel);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var error = result as BadRequestObjectResult;
            Assert.That(error!.Value, Is.Not.Null);
            Assert.That(error.Value?.GetType().GetProperty("success")?.GetValue(error.Value, null), Is.EqualTo(false));
        }

        [Test]
        public async Task AddFeedback_WhenExceptionOccurs_ReturnsBadRequest()
        {
            //Arrange 
            var feedbackViewModel = GetFeedbackdata();

            _mockFeedbackService.Setup(x => x.SubmitFeedback(It.IsAny<FeedbackViewModel>()))
                .ThrowsAsync(new Exception("Test exception"));

            //Act

            var result = await _feedbackController.AddFeedback(feedbackViewModel);
            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var error = result as BadRequestObjectResult;
            Assert.That(error!.Value?.GetType().GetProperty("success")?.GetValue(error.Value, null), Is.EqualTo(false));

        }

        #endregion

        #region PRIVATE METHODS
        private FeedbackViewModel GetFeedbackdata()
        {
            return new FeedbackViewModel
            { 
                UserId = "1",
                CategoryIDs =new List<int> {1},
                Rating =3,
                CustomCategoryText = "CustomCategoryText",
                IssueDescription = "IssueDescription",
                SuggestionsImprovement = "Test"
            };
        }

        #endregion
    }
}
