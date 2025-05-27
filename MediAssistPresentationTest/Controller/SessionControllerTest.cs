using Azure;
using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.Application.Services;
using MediAssist.DbContext;
using MediAssist.UI.Controllers;
using MediAssist.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssistPresentationTest.Controller
{
    [TestFixture]
    public class SessionControllerTest
    {
        #region PRIVATE INSTANCE FIELD

        private Mock<ILogger<SessionController>> _mockLogger;
        private SessionController _sessionController;
        private Mock<IUserSessionService> _mockUserSessionService;

        #endregion

        #region TEST SETUP
        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<SessionController>>();
            _mockUserSessionService = new Mock<IUserSessionService>();
            _sessionController = new SessionController(_mockUserSessionService.Object,_mockLogger.Object);
        }

        #endregion

        #region PUBLIC METHODS

        #region START OR RESUME SESSION

        [Test]
        public void StartOrResumeSession_WithValidInput_ReturnsOkResult()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = "testUser123",
                SessionId = 1
            };
            // Act
            var result = _sessionController.StartOrResumeSession(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void StartOrResumeSession_WithNullUserId_ReturnsBadRequest()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = null!,
                SessionId = 1
            };

            // Act
            var result = _sessionController.StartOrResumeSession(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value?.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }

        [Test]
        public void StartOrResumeSession_WhenServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = "testUser123",
                SessionId = 1,
                TotalToken = 100,
                TotalCost = 10,
                IsPotentialDiagnosisOn = true
            };
            var expectedException = new Exception("Service error");

            var startSessionDetails = new StartSessionDetails
            {
                UserId = userSessionViewModel.UserId,
                SessionId = userSessionViewModel.SessionId,
                TotalToken = userSessionViewModel.TotalToken,
                TotalCost = userSessionViewModel.TotalCost,
                IsPotentialDiagnosisOn = userSessionViewModel.IsPotentialDiagnosisOn
            };

            _mockUserSessionService
                .Setup(s => s.StartOrResumeSession(It.IsAny<StartSessionDetails>()))
                .Throws(expectedException);

            // Act
            var result = _sessionController.StartOrResumeSession(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value?.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }

        [Test]
        public void StartOrResumeSession_WithEmptyUserId_ReturnsBadRequest()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = string.Empty,
                SessionId = 1
            };

            // Act
            var result = _sessionController.StartOrResumeSession(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value?.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }
        #endregion

        #region STOP USER SESSION

        [Test]
        public void StopUserSession_WithValidInput_ReturnsOkResult()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = "testUser123",
                SessionId = 123
            };
           
            // Act
            var result = _sessionController.StopUserSession(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void StopUserSession_WithNullUserId_ReturnsBadRequest()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = null,
                SessionId = 123
            };

            // Act
            var result = _sessionController.StopUserSession(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value?.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }

        [Test]
        public void StopUserSession_WithZeroSessionId_ReturnsBadRequest()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = "testUser123",
                SessionId = 0
            };

            // Act
            var result = _sessionController.StopUserSession(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value?.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }

        [Test]
        public void StopUserSession_WhenServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = "testUser123",
                SessionId = 123,
                TotalToken = 100,
                TotalCost = 10,
            };
            var expectedException = new Exception("Service error");

            _mockUserSessionService
                .Setup(s => s.StopUserSession(userSessionViewModel.SessionId, userSessionViewModel.UserId, userSessionViewModel.TotalToken, userSessionViewModel.TotalCost))
                .Throws(expectedException);

            // Act
            var result = _sessionController.StopUserSession(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value?.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }

        [Test]
        public void StopUserSession_WithEmptyUserId_ReturnsBadRequest()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = string.Empty,
                SessionId = 123
            };

            // Act
            var result = _sessionController.StopUserSession(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value?.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }
        #endregion

        #region UPDATE USER SESSION

        [Test]
        public void UpdateUserSession_WithValidInput_ReturnsOkResult()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = "testUser123",
                SessionId = 123
            };

            // Act
            var result = _sessionController.UpdateUserSession(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void UpdateUserSession_WithNullUserId_ReturnsBadRequest()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = null!,
                SessionId = 123
            };

            // Act
            var result = _sessionController.UpdateUserSession(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value?.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }

        [Test]
        public void UpdateUserSession_WithZeroSessionId_ReturnsBadRequest()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = "testUser123",
                SessionId = 0
            };

            // Act
            var result = _sessionController.UpdateUserSession(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value?.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }

        [Test]
        public void UpdateUserSession_WhenServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = "testUser123",
                SessionId = 123,
                TotalToken = 100,
                TotalCost = 10,
            };
            var expectedException = new Exception("Service error");

            _mockUserSessionService
                .Setup(s => s.UpdateUserSession(userSessionViewModel.SessionId, userSessionViewModel.UserId, userSessionViewModel.TotalToken, userSessionViewModel.TotalCost))
                .Throws(expectedException);

            // Act
            var result = _sessionController.UpdateUserSession(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value?.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }

        [Test]
        public void UpdateUserSession_WithEmptyUserId_ReturnsBadRequest()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = string.Empty,
                SessionId = 123
            };

            // Act
            var result = _sessionController.UpdateUserSession(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value?.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }

        #endregion

        #region REPORT GENERATED

        [Test]
        public void ReportGenerated_WithValidInput_ReturnsOkResult()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = "testUser123",
                SessionId = 123
            };
           
            // Act
            var result = _sessionController.ReportGenerated(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

        }

        [Test]
        public void ReportGenerated_WithNullUserId_ReturnsBadRequest()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = null!,
                SessionId = 123
            };

            // Act
            var result = _sessionController.ReportGenerated(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value?.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }

        [Test]
        public void ReportGenerated_WithZeroSessionId_ReturnsBadRequest()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = "testUser123",
                SessionId = 0
            };

            // Act
            var result = _sessionController.ReportGenerated(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value?.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }

        [Test]
        public void ReportGenerated_WhenServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = "testUser123",
                SessionId = 123,
                TotalToken = 100,
                TotalCost = 10,
            };
            var expectedException = new Exception("Service error");

            _mockUserSessionService
                .Setup(s => s.ReportGenerated(userSessionViewModel.SessionId, userSessionViewModel.UserId, userSessionViewModel.TotalToken, userSessionViewModel.TotalCost))
                .Throws(expectedException);

            // Act
            var result = _sessionController.ReportGenerated(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value?.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }

        [Test]
        public void ReportGenerated_WithEmptyUserId_ReturnsBadRequest()
        {
            // Arrange
            var userSessionViewModel = new UserSessionViewModel
            {
                UserId = string.Empty,
                SessionId = 123
            };

            // Act
            var result = _sessionController.ReportGenerated(userSessionViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value?.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }
        #endregion

        #endregion

    }
}
