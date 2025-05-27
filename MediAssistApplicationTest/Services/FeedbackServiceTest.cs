using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Entities;
using MediAssist.Application.Services;
using MediAssist.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediAssistApplicationTest.Services
{
    [TestFixture]
    public class FeedbackServiceTest
    {
        #region PRIVATE INSTANCE FIELD
        private FeedbackService _feedbackService;
        private Mock<IUserRepository> _userRepository;
        private Mock<ILogger<FeedbackService>> _logger;
        private MediAssistDbContext _mediAssistDbContext;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;

        #endregion

        #region TEST SETUP
        [SetUp]
        public void Setup()
        {
           
            _userRepository = new Mock<IUserRepository>();
            _logger = new Mock<ILogger<FeedbackService>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _feedbackService = new FeedbackService(_userRepository.Object, _userManagerMock.Object, _logger.Object);
        }
        #endregion

        #region SUBMITFEEDBACK

        [Test]
        public void Constructor_InitializesAllDependencies()
        {
            // Arrange
            // Create the DbContext properly with options
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new MediAssistDbContext(options);

            var userRepo = new Mock<IUserRepository>().Object;

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null).Object;

            var logger = new Mock<ILogger<FeedbackService>>().Object;

            // Act
            var service = new FeedbackService(userRepo, userManager, logger);

            // Assert
            Assert.That(service, Is.Not.Null);

            // Using reflection to verify private fields
            var contextField = typeof(FeedbackService).GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var userRepoField = typeof(FeedbackService).GetField("_userRepository", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var userManagerField = typeof(FeedbackService).GetField("_userManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var loggerField = typeof(FeedbackService).GetField("_logger", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            Assert.That(userRepoField.GetValue(service), Is.EqualTo(userRepo));
            Assert.That(userManagerField.GetValue(service), Is.EqualTo(userManager));
            Assert.That(loggerField.GetValue(service), Is.EqualTo(logger));
        }

        [Test]
        public async Task SubmitFeedback_NullFeedbackData_ReturnsBadRequest()
        {
            // Arrange
            FeedbackViewModel feedbackData = null;

            // Act
            var result = await _feedbackService.SubmitFeedback(feedbackData);

            // Assert
            Assert.That(result, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task SubmitFeedback_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var feedbackData = new FeedbackViewModel
            {
                UserId = "user123",
                Rating = 4,
                IssueDescription = "Test issue",
                SuggestionsImprovement = "Test suggestions",
                CategoryIDs = new List<int>{ 1, 2, 3 }
            };

            _userManagerMock.Setup(x => x.FindByIdAsync(feedbackData.UserId))
                .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _feedbackService.SubmitFeedback(feedbackData);

            // Assert
            Assert.That(result, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task SubmitFeedback_ValidData_ReturnsOK()
        {
            // Arrange
            var feedbackData = new FeedbackViewModel
            {
                UserId = "user123",
                Rating = 4,
                IssueDescription = "Test issue",
                SuggestionsImprovement = "Test suggestions",
                CategoryIDs = new List<int> { 1, 2, 3 },
                CustomCategoryText = "Custom category"
            };

            var user = new ApplicationUser
            {
                Id = "user123",
                Email = "test@example.com"
            };

            _userManagerMock.Setup(x => x.FindByIdAsync(feedbackData.UserId))
                .ReturnsAsync(user);

            var savedFeedback = new Feedback { Id = 1 };
            _userRepository.Setup(x => x.AddFeedbackAsync(It.IsAny<Feedback>()))
                .ReturnsAsync(savedFeedback);

            _userRepository.Setup(x => x.AddCategoryFeedbackAsync(It.IsAny<FeedbackCategoryMapping>()))
                .ReturnsAsync(new FeedbackCategoryMapping());

            // Act
            var result = await _feedbackService.SubmitFeedback(feedbackData);

            // Assert
            Assert.That(result, Is.EqualTo(HttpStatusCode.OK));
            _userRepository.Verify(x => x.AddFeedbackAsync(It.IsAny<Feedback>()), Times.Once);
            _userRepository.Verify(x => x.AddCategoryFeedbackAsync(It.IsAny<FeedbackCategoryMapping>()), Times.Exactly(3));
        }

        [Test]
        public async Task SubmitFeedback_WithCustomCategory_SetsCustomCategoryText()
        {
            // Arrange
            var feedbackData = new FeedbackViewModel
            {
                UserId = "user123",
                Rating = 4,
                IssueDescription = "Test issue",
                SuggestionsImprovement = "Test suggestions",
                CategoryIDs = new List<int> { 5 }, // Category 5 is the custom category
                CustomCategoryText = "Custom category text"
            };

            var user = new ApplicationUser
            {
                Id = "user123",
                Email = "test@example.com"
            };

            _userManagerMock.Setup(x => x.FindByIdAsync(feedbackData.UserId))
                .ReturnsAsync(user);

            var savedFeedback = new Feedback { Id = 1 };
            _userRepository.Setup(x => x.AddFeedbackAsync(It.IsAny<Feedback>()))
                .ReturnsAsync(savedFeedback);

            FeedbackCategoryMapping capturedMapping = null;
            _userRepository.Setup(x => x.AddCategoryFeedbackAsync(It.IsAny<FeedbackCategoryMapping>()))
                .Callback<FeedbackCategoryMapping>(m => capturedMapping = m)
                .ReturnsAsync(new FeedbackCategoryMapping());

            // Act
            var result = await _feedbackService.SubmitFeedback(feedbackData);

            // Assert
            Assert.That(result, Is.EqualTo(HttpStatusCode.OK));
            _userRepository.Verify(x => x.AddCategoryFeedbackAsync(It.IsAny<FeedbackCategoryMapping>()), Times.Once);
            Assert.That(capturedMapping, Is.Not.Null);
            Assert.That(capturedMapping.CategoryID, Is.EqualTo(5));
            Assert.That(capturedMapping.CustomCategoryText, Is.EqualTo("Custom category text"));
        }


        [Test]
        public async Task SubmitFeedback_WhenFeedbackDataIsNull_ReturnsBadRequest()
        {
            FeedbackViewModel feedbackViewModel = null;

            // Act
            var result = await _feedbackService.SubmitFeedback(feedbackViewModel);

            // Assert
            Assert.That(result, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task SubmitFeedback_WhenUserNotFound_ReturnsNotFound()
        {
            // Arrange
            var feedbackData = new FeedbackViewModel
            {
                UserId = "1"
            };

           _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _feedbackService.SubmitFeedback(feedbackData);

            // Assert
            Assert.That(result, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task SubmitFeedback_WhenSuccessful_ReturnsOK()
        {
            // Arrange
            var feedbackData = new FeedbackViewModel
            {
                UserId = "testUserId",
                Rating = 5,
                IssueDescription = "Test Issue",
                SuggestionsImprovement = "Test Suggestion",
                CategoryIDs = new List<int> { 1, 2, 5 },
                CustomCategoryText = "Custom Category"
            };

            var user = new ApplicationUser
            {
                Id = "testUserId",
                Email = "test@example.com"
            };

            var feedback = new Feedback { Id = 1 };

            _userManagerMock.Setup(x => x.FindByIdAsync(feedbackData.UserId))
                .ReturnsAsync(user);

            _userRepository.Setup(x => x.AddFeedbackAsync(It.IsAny<Feedback>()))
                .ReturnsAsync(feedback);

            _userRepository.Setup(x => x.AddCategoryFeedbackAsync(It.IsAny<FeedbackCategoryMapping>()))
                .ReturnsAsync(new FeedbackCategoryMapping());

            // Act
            var result = await _feedbackService.SubmitFeedback(feedbackData);

            // Assert
            Assert.That(result, Is.EqualTo(HttpStatusCode.OK));

            // Verify feedback was created with correct data
            _userRepository.Verify(x => x.AddFeedbackAsync(
                It.Is<Feedback>(f =>
                    f.UserId == user.Id &&
                    f.EmailAddress == user.Email &&
                    f.FeedbackRating == feedbackData.Rating &&
                    f.IssueDescription == feedbackData.IssueDescription &&
                    f.ImprovementSuggestions == feedbackData.SuggestionsImprovement &&
                    f.CreatedDate.Date == DateTime.UtcNow.Date
                )
            ), Times.Once);

            // Verify category mappings were created
            _userRepository.Verify(x => x.AddCategoryFeedbackAsync(
                It.Is<FeedbackCategoryMapping>(m =>
                    m.FeedbackID == feedback.Id &&
                    m.CreatedDate.Date == DateTime.UtcNow.Date
                )
            ), Times.Exactly(feedbackData.CategoryIDs.Count));
        }

      
        [Test]
        public async Task SubmitFeedback_WhenAddFeedbackFails_ReturnsInternalServerError()
        {
            // Arrange
            var feedbackData = new FeedbackViewModel
            {
                UserId = "testUserId"
            };

            var user = new ApplicationUser
            {
                Id = "testUserId",
                Email = "test@example.com"
            };

            _userManagerMock.Setup(x => x.FindByIdAsync(feedbackData.UserId))
                .ReturnsAsync(user);

            _userRepository.Setup(x => x.AddFeedbackAsync(It.IsAny<Feedback>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _feedbackService.SubmitFeedback(feedbackData);

            // Assert
            Assert.That(result, Is.EqualTo(HttpStatusCode.InternalServerError));

            // Verify error was logged
            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }

        [Test]
        public async Task SubmitFeedback_WhenAddCategoryMappingFails_ReturnsInternalServerError()
        {
            // Arrange
            var feedbackData = new FeedbackViewModel
            {
                UserId = "testUserId",
                CategoryIDs = new List<int> { 1 }
            };

            var user = new ApplicationUser
            {
                Id = "testUserId",
                Email = "test@example.com"
            };

            var feedback = new Feedback { Id = 1 };

            _userManagerMock.Setup(x => x.FindByIdAsync(feedbackData.UserId))
                .ReturnsAsync(user);

            _userRepository.Setup(x => x.AddFeedbackAsync(It.IsAny<Feedback>()))
                .ReturnsAsync(feedback);

            _userRepository.Setup(x => x.AddCategoryFeedbackAsync(It.IsAny<FeedbackCategoryMapping>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _feedbackService.SubmitFeedback(feedbackData);

            // Assert
            Assert.That(result, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        #endregion

    }
}
