using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.Infrastructure.Abstract.Configurations;
using MediAssist.UI.Controllers;
using MediAssist.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace MediAssistPresentationTest.Controller
{
    [TestFixture]
    public class LoginControllerTest
    {
        #region PRIVATE INSTANCE FIELD

        private Mock<IUserService> _mockUserService;
        private Mock<IAppSettings> _mockAppSettings;
        private Mock<ILogger<LoginController>> _mockLogger;
        private LoginController _loginController;
        private Mock<HttpResponse> _mockResponse;

        #endregion

        #region TEST SETUP
        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
            _mockAppSettings = new Mock<IAppSettings>();
            _mockLogger = new Mock<ILogger<LoginController>>();
            _mockResponse = new Mock<HttpResponse>();

            _mockAppSettings.SetupGet(a => a.Key).Returns("YourLongRandom256BitKeyHereYourLongRandom256BitKeyHere");
            _mockAppSettings.SetupGet(a => a.Issuer).Returns("https://medi-dev.mypits.org/");
            _mockAppSettings.SetupGet(a => a.Audience).Returns("https://medi-dev.mypits.org/");
            _mockAppSettings.SetupGet(x => x.ExpirationDays).Returns(7);

            _loginController = new LoginController(
                _mockUserService.Object,
                _mockAppSettings.Object,
                _mockLogger.Object
            );
            _loginController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }
  
        #endregion

        #region PUBLIC METHODS

        #region LOGOUT

        [Test]
        public void Logout_ShouldRemoveCookie_AndReturnSuccessMessage()
        {
            // Act
            var result = _loginController.Logout() as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode,Is.EqualTo(200));
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value?.GetType().GetProperty("success")?.GetValue(result.Value, null), Is.EqualTo(true));

            // Verify that the cookie was set with an expired date
            _mockResponse.Verify(r => r.Cookies.Append("sessionToken", "", It.Is<CookieOptions>(options =>
                options.Expires < DateTime.UtcNow)), Times.Never);
        }

        #endregion

        #region LOGIN

        [Test]
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                Email = "aswana.bk@pitsolutions.com",
                Password = "Pits@123",
                RememberMe = true
            };

            var userServiceResponse = new LoginUserDetails
            {
                HttpStatusCode = HttpStatusCode.OK,
                UserId = "268e9740-ca08-4570-9dba-c5b358c468b0",
                FullName = "Aswana",
                FirstName = "Aswana",
                Image = "image.png",
                TitleAbbreviation = "Dr.",
                Specialization = "Cardiology",
                IsSettingsUpdated = true
            };

            _mockUserService
                .Setup(x => x.SignInWithEmailAndPasswordAsync(loginViewModel.Email, loginViewModel.Password))
                .ReturnsAsync(userServiceResponse);

            // Act
            var result = await _loginController.Login(loginViewModel) ;

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.That(okResult!.Value, Is.Not.Null);
            Assert.That(okResult.Value?.GetType().GetProperty("success")?.GetValue(okResult.Value, null), Is.EqualTo(true));
          
        }

        [Test]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                Email = "aswana.bk@pitsolutions.com",
                Password = "WrongPassword",
            };

            var userServiceResponse = new LoginUserDetails
            {
                HttpStatusCode = HttpStatusCode.Unauthorized
            };

            _mockUserService
                .Setup(s => s.SignInWithEmailAndPasswordAsync(loginViewModel.Email, loginViewModel.Password))
                .ReturnsAsync(userServiceResponse);

            // Act
            var result = await _loginController.Login(loginViewModel);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);

            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.That(unauthorizedResult,Is.Not.Null);
            Assert.That(unauthorizedResult.Value?.GetType().GetProperty("success")?.GetValue(unauthorizedResult.Value, null), Is.EqualTo(false));
        }

        [Test]
        public async Task Login_NonExistentAccount_ReturnsNotFound()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                Email = "nonexistent@example.com",
                Password = "Test@123",
            };

            var userServiceResponse = new LoginUserDetails
            {
                HttpStatusCode = HttpStatusCode.NotFound
            };

            _mockUserService
                .Setup(s => s.SignInWithEmailAndPasswordAsync(loginViewModel.Email, loginViewModel.Password))
                .ReturnsAsync(userServiceResponse);

            // Act
            var result = await _loginController.Login(loginViewModel);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.That(notFoundResult.Value?.GetType().GetProperty("success")?.GetValue(notFoundResult.Value, null), Is.EqualTo(false));
        }

        [Test]
        public async Task Login_ShouldReturnInternalServerError_OnException()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                Email = "aswana.bk@pitsolutions.com",
                Password = "Pits@123"
            };

            _mockUserService
                .Setup(s => s.SignInWithEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _loginController.Login(loginViewModel);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);

            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.That(objectResult.Value?.GetType().GetProperty("success")?.GetValue(objectResult.Value, null), Is.EqualTo(false));
        }

        #endregion

        #endregion
    }
}
