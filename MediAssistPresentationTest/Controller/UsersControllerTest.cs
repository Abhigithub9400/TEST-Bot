using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.DbContext;
using MediAssist.Infrastructure.HttpProvider.Services.Abstract;
using MediAssist.UI.Controllers;
using MediAssist.UI.Models;
using MediAssist.UI.Validator;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssistPresentationTest.Controller
{
    [TestFixture]
    public class UsersControllerTest
    {
        #region PRIVATE INSTANCE FIELD


        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IUserService> _mockUserService;
        private Mock<ILogger<UsersController>> _mockLogger;
        private UsersController _usersController;
        private Mock<IMailService> _mockMailService;
        private Mock<UserManager<ApplicationUser>> _userManager;
        private Mock<IEmailManagementService> _mockMailManagementService;
        private Mock<HttpContext> _mockHttpContext;
        private Mock<IResponseCookies> _mockResponseCookies;
        private const int MaxFailedAttempts = 3;
        
       
        #endregion

        #region TEST SETUP

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockUserService = new Mock<IUserService>();
            _mockMailManagementService = new Mock<IEmailManagementService>();
            _mockMailService = new Mock<IMailService>();
            _mockLogger = new Mock<ILogger<UsersController>>();
            

            

            _mockHttpContext = new Mock<HttpContext>();
            _mockResponseCookies = new Mock<IResponseCookies>();
            _mockHttpContext.Setup(x => x.Response.Cookies).Returns(_mockResponseCookies.Object);
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
            _usersController = new UsersController(_userManager.Object, _mockConfiguration.Object, _mockMailService.Object, _mockUserService.Object, _mockMailManagementService.Object, _mockLogger.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object
                }
            };
           
        }
        #endregion

        #region PUBLIC METHODS

        #region FORGET PASSWORD

        [Test]
        public async Task ForgetPassword_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new ForgotPasswordRequest { Email = "sradha.ks@pitsolutions.com" };
            var user = new ApplicationUser
            {
                Email = "sradha.ks@pitsolutions.com",
                FirstName = "Sradha",
                ForgotPasswordAttemptCount = 0
            };

            _userManager.Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);
            _userManager.Setup(x => x.UpdateSecurityStampAsync(user))
                .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync("test-token");
            _mockMailManagementService.Setup(x => x.SendResetPasswordEmail(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .ReturnsAsync(true);
            _userManager.Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _usersController.ForgetPassword(request);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            if (result is OkObjectResult OkObjectResult && OkObjectResult.Value != null)
            {
                var successProperty = OkObjectResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(OkObjectResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsTrue((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected OkObjectResult but got a different type or null value.");
            }
            
        }

        [Test]
        public async Task ForgetPassword_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var request = new ForgotPasswordRequest { Email = "nonexistent@example.com" };
            _userManager.Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _usersController.ForgetPassword(request);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            if (result is NotFoundObjectResult NotFoundObjectResult && NotFoundObjectResult.Value != null)
            {
                var successProperty = NotFoundObjectResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(NotFoundObjectResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected NotFoundObjectResult but got a different type or null value.");
            }
        }

        [Test]
        public async Task ForgetPassword_TooManyAttempts_ReturnsBadRequest()
        {
            // Arrange
            var request = new ForgotPasswordRequest { Email = "test@example.com" };
            var user = new ApplicationUser
            {
                Email = "test@example.com",
                ForgotPasswordAttemptCount = MaxFailedAttempts,
                LastForgotPasswordAttemptTimestamp = DateTime.UtcNow
            };

            _userManager.Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _usersController.ForgetPassword(request);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>()); if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }

        }

        [Test]
        public async Task ForgetPassword_EmailServiceFails_StillReturnsOk()
        {
            // Arrange
            var request = new ForgotPasswordRequest { Email = "test@example.com" };
            var user = new ApplicationUser
            {
                Email = "test@example.com",
                FirstName = "Test",
                ForgotPasswordAttemptCount = 0
            };

            _userManager.Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);
            _userManager.Setup(x => x.UpdateSecurityStampAsync(user))
                .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync("test-token");
            _mockMailManagementService.Setup(x => x.SendResetPasswordEmail(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _usersController.ForgetPassword(request);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(user.ForgotPasswordAttemptCount, Is.EqualTo(0));

            if (result is OkObjectResult OkObjectResult && OkObjectResult.Value != null)
            {
                var successProperty = OkObjectResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(OkObjectResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsTrue((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected OkObjectResult but got a different type or null value.");
            }
        }

        [Test]
        public async Task ForgetPassword_UpdateSecurityStampFails_ReturnsBadRequest()
        {
            // Arrange
            var request = new ForgotPasswordRequest { Email = "test@example.com" };
            var user = new ApplicationUser
            {
                Email = "test@example.com",
                FirstName = "Test",
                ForgotPasswordAttemptCount = 0
            };

            _userManager.Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);
            _userManager.Setup(x => x.UpdateSecurityStampAsync(user))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to update security stamp" }));

            // Act
            var result = await _usersController.ForgetPassword(request);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }

        }
       
        [Test]
        public async Task ForgetPassword_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _usersController.ModelState.AddModelError("Email", "Email is required");
            var request = new ForgotPasswordRequest { Email = null! };

            // Act
            var result = await _usersController.ForgetPassword(request);
      
            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }

        }
        #endregion

        #region VALIDATE RESET TOKEN
        [Test]
        public async Task ValidateResetToken_ValidToken_ReturnsOkWithEmail()
        {
            // Arrange
            var validToken = "validToken123";
            var email = "test@example.com";
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(validToken));

           
            var testUsers = new List<ApplicationUser>
             {
            new ApplicationUser { UserName = "User1", Email = "user1@example.com" },
            new ApplicationUser { UserName = "User2", Email = email } 
            }.AsQueryable();

            
            _userManager.SetupGet(m => m.Users).Returns(testUsers);

           
            _userManager.Setup(x => x.VerifyUserTokenAsync(It.Is<ApplicationUser>(u => u.Email == email), It.IsAny<string>(), "ResetPassword", validToken )).ReturnsAsync(true);

            // Act
            var result = await _usersController.ValidateResetToken(encodedToken);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            if (result is OkObjectResult OkObjectResult && OkObjectResult.Value != null)
            {
                var successProperty = OkObjectResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(OkObjectResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsTrue((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected OkObjectResult but got a different type or null value.");
            }
        }



        [Test]
        public async Task ValidateResetToken_NullToken_ReturnsBadRequest()
        {
            // Arrange
            string token = null!;

            // Act
            var result = await _usersController.ValidateResetToken(token);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }

        }

        [Test]
        public async Task ValidateResetToken_EmptyToken_ReturnsBadRequest()
        {
            // Arrange
            var token = string.Empty;

            // Act
            var result = await _usersController.ValidateResetToken(token);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }

        [Test]
        public async Task ValidateResetToken_InvalidBase64Token_ReturnsBadRequest()
        {
            // Arrange
            var invalidToken = "invalid@token@format";

            // Act
            var result = await _usersController.ValidateResetToken(invalidToken);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }

        [Test]
        public async Task ValidateResetToken_NoValidUserFound_ReturnsBadRequest()
        {
            // Arrange
            var token = "validToken123";
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            _userManager.Setup(x => x.VerifyUserTokenAsync(
                It.IsAny<ApplicationUser>(),
                It.IsAny<string>(),
                "ResetPassword",
                token))
                .ReturnsAsync(false);

            // Act
            var result = await _usersController.ValidateResetToken(encodedToken);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }

        [Test]
        public async Task ValidateResetToken_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var token = "validToken123";
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var exceptionMessage = "Unexpected error occurred";

            _userManager.Setup(x => x.VerifyUserTokenAsync(
                It.IsAny<ApplicationUser>(),
                It.IsAny<string>(),
                "ResetPassword",
                token))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _usersController.ValidateResetToken(encodedToken);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }
        #endregion

        #region RESET PASSWORD

        [Test]
        public async Task ResetPassword_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var token = "validToken123";
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var email = "test@example.com";
            var password = "NewPassword123!";

            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                Email = email,
                Token = encodedToken,
                Password = password,
                ConfirmPassword= password
            };

            var user = new ApplicationUser {Email=email};

            _userManager.Setup(x => x.FindByEmailAsync(resetPasswordViewModel.Email))
                .ReturnsAsync(user);

            _userManager.Setup(x => x.ResetPasswordAsync(user, token, password))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _usersController.ResetPassword(resetPasswordViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            if (result is OkObjectResult okResult && okResult.Value != null)
            {
                var successProperty = okResult.Value.GetType().GetProperty("success");
                var redirectUrlProperty = okResult.Value.GetType().GetProperty("redirectUrl");

                Assert.That(successProperty, Is.Not.Null, "The 'success' property was not found in the response.");
                Assert.That(redirectUrlProperty, Is.Not.Null, "The 'redirectUrl' property was not found in the response.");

                var successValue = successProperty?.GetValue(okResult.Value, null);
                var redirectUrlValue = redirectUrlProperty?.GetValue(okResult.Value, null);

                Assert.That(successValue, Is.Not.Null, "The 'success' property is null.");
                Assert.That(successValue, Is.InstanceOf<bool>(), "The 'success' property is not a boolean.");
                Assert.That((bool)successValue, Is.True, "The 'success' property should be true.");

                Assert.That(redirectUrlValue, Is.Not.Null, "The 'redirectUrl' property is null.");
                Assert.That(redirectUrlValue, Is.InstanceOf<string>(), "The 'redirectUrl' property is not a string.");
                Assert.That(redirectUrlValue, Is.EqualTo("/login"), "The 'redirectUrl' property has an unexpected value.");
            }
            else
            {
                Assert.Fail("Expected OkObjectResult but got a different type or null value.");
            }

        }


        [Test]
        public async Task ResetPassword_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var resetPasswordViewModel = new ResetPasswordViewModel();
            _usersController.ModelState.AddModelError("Password", "Password is required");

            // Act
            var result = await _usersController.ResetPassword(resetPasswordViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }

        [Test]
        public async Task ResetPassword_UserNotFound_ReturnsBadRequest()
        {
            // Arrange
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                Email = "nonexistent@example.com",
                Token = "someToken",
                Password = "NewPassword123!"
            };

            _userManager.Setup(x => x.FindByEmailAsync(resetPasswordViewModel.Email))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _usersController.ResetPassword(resetPasswordViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected a BadRequestObjectResult.");

            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                // Check the type of the response value
                if (badRequestResult.Value is string errorMessage)
                {
                    Assert.That(errorMessage, Is.EqualTo("Invalid Request"), "Unexpected error message.");
                }
                else
                {
                    // Use dynamic for flexibility in property checking
                    dynamic response = badRequestResult.Value;

                    Assert.That(response, Is.Not.Null, "Expected a response object but got null.");
                    Assert.That(response.success, Is.False, "Expected success to be false.");
                    Assert.That((string)response.message, Is.EqualTo("Invalid Request"), "Unexpected message.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult with a valid response.");
            }
        }


        [Test]
        public async Task ResetPassword_ResetPasswordFails_ReturnsBadRequest()
        {
            // Arrange
            var token = "validToken123";
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var email = "test@example.com";
            var password = "NewPassword123!";

            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                Email = email,
                Token = encodedToken,
                Password = password
            };

            var user = new ApplicationUser{ Email = email };
            var errors = new List<IdentityError>
            {
             new IdentityError { Description = "Password reset failed" }
            };

            _userManager.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManager.Setup(x => x.ResetPasswordAsync(user, token, password))
                .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

            // Act
            var result = await _usersController.ResetPassword(resetPasswordViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }

        [Test]
        public async Task ResetPassword_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                Email = "test@example.com",
                Token = "someToken",
                Password = "NewPassword123!"
            };

            var exceptionMessage = "An unexpected error occurred";
            _userManager.Setup(x => x.FindByEmailAsync(resetPasswordViewModel.Email))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _usersController.ResetPassword(resetPasswordViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }

        #endregion

        #region CheckPasswordIfExist

        [Test]
        public async Task CheckPasswordIfExist_ReturnsBadRequest_WhenUserIdIsNull()
        {
            // Act
            var result = await _usersController.CheckPasswordIfExist(null!, "password123");

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }

        [Test]
        public async Task CheckPasswordIfExist_ReturnsBadRequest_WhenPasswordValidationFails()
        {
            // Arrange
            var userId = "12345";
            var password = ""; // Invalid password

            // Act
            var result = await _usersController.CheckPasswordIfExist(userId, password);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            var json = JsonConvert.SerializeObject(badRequestResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Assert.That(response!["success"], Is.False);
            Assert.That(response["message"], Is.EqualTo("Password is required."));
        }

        [Test]
        public async Task CheckPasswordIfExist_ReturnsOk_WithSuccessFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var userId = "8469b2ad-75bf-43f1-97d5-6f90dc6b14db";
            var password = "wrongpassword";
            _mockUserService
                .Setup(s => s.CheckWhetherCurrentPasswordCorrectOrNot(userId, password))
                .ReturnsAsync(false);

            // Act
            var result = await _usersController.CheckPasswordIfExist(userId, password);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var json = JsonConvert.SerializeObject(okResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.That(response!["success"], Is.False);
            Assert.That(response["IfPasswordCorrect"], Is.False);
            Assert.That(response["message"], Is.EqualTo("Please enter the correct password to proceed."));
        }

        [Test]
        public async Task CheckPasswordIfExist_ReturnsOk_WithSuccessTrue_WhenPasswordIsCorrect()
        {
            // Arrange
            var userId = "8469b2ad-75bf-43f1-97d5-6f90dc6b14db";
            var password = "Pits@123";
            _mockUserService
                .Setup(s => s.CheckWhetherCurrentPasswordCorrectOrNot(userId, password))
                .ReturnsAsync(true);

            // Act
            var result = await _usersController.CheckPasswordIfExist(userId, password);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            if (result is OkObjectResult OkObjectResult && OkObjectResult.Value != null)
            {
                var successProperty = OkObjectResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(OkObjectResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsTrue((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected OkObjectResult but got a different type or null value.");
            }
        }

        [Test]
        public async Task CheckPasswordIfExist_ReturnsBadRequest_OnException()
        {
            // Arrange
            var userId = "8469b2ad-75bf-43f1-97d5-6f90dc6b14db";
            var password = "anyPassword";
            _mockUserService
                .Setup(s => s.CheckWhetherCurrentPasswordCorrectOrNot(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Something went wrong."));

            // Act
            var result = await _usersController.CheckPasswordIfExist(userId, password);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            var json = JsonConvert.SerializeObject(badRequestResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Assert.That(response!["success"], Is.False);
            Assert.That(response["message"], Is.EqualTo("Something went wrong."));
        }

        #endregion

        #region ChangePassword

        [Test]
        public async Task ChangePassword_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _usersController.ModelState.AddModelError("UserId", "Required");

            // Act
            var result = await _usersController.ChangePassword(new ChangePasswordViewModel());

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            var json = JsonConvert.SerializeObject(badRequestResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Assert.That(response!["success"], Is.False);
            Assert.That(response["message"], Is.EqualTo("Invalid input."));
        }

        [Test]
        public async Task ChangePassword_ReturnsBadRequest_WhenUserNotFound()
        {
            // Arrange
            var changePasswordModel = GetPasswordDetails();

            _userManager
                .Setup(m => m.FindByIdAsync(changePasswordModel.UserId))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _usersController.ChangePassword(changePasswordModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.Value, Is.EqualTo("Invalid Request"));
        }

        [Test]
        public async Task ChangePassword_ReturnsBadRequest_WhenPasswordChangeFails()
        {
            // Arrange
            var changePasswordModel = GetPasswordDetails();

            var user = new ApplicationUser { Id = changePasswordModel.UserId };

            _userManager
                .Setup(m => m.FindByIdAsync(changePasswordModel.UserId))
                .ReturnsAsync(user);

            _userManager
                .Setup(m => m.ChangePasswordAsync(user, changePasswordModel.Password, changePasswordModel.ConfirmPassword))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password does not meet complexity requirements." }));

            // Act
            var result = await _usersController.ChangePassword(changePasswordModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }

        [Test]
        public async Task ChangePassword_ReturnsOk_WhenPasswordChangeSucceeds()
        {
            // Arrange
            var changePasswordModel = GetPasswordDetails();

            var user = new ApplicationUser { Id = changePasswordModel.UserId };

            _userManager
                .Setup(m => m.FindByIdAsync(changePasswordModel.UserId))
                .ReturnsAsync(user);

            _userManager
                .Setup(m => m.ChangePasswordAsync(user, changePasswordModel.Password, changePasswordModel.ConfirmPassword))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _usersController.ChangePassword(changePasswordModel);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var json = JsonConvert.SerializeObject(okResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Assert.That(response!["success"], Is.True);
            Assert.That(response["message"], Is.EqualTo("Your password has been successfully changed. Please sign in using your new password"));
            Assert.That(response["redirectUrl"], Is.EqualTo("/login"));
        }

        [Test]
        public async Task ChangePassword_ReturnsBadRequest_OnException()
        {
            // Arrange
            var changePasswordModel = GetPasswordDetails();

            _userManager
                .Setup(m => m.FindByIdAsync(changePasswordModel.UserId))
                .ThrowsAsync(new Exception("Something went wrong."));

            // Act
            var result = await _usersController.ChangePassword(changePasswordModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }

        #endregion

        #region UPDATE COUNTERS
        [Test]
        public async Task UpdateCounters_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var userId = "test-user-id";
            var user = new ApplicationUser { Id = userId };
            _userManager.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _mockUserService.Setup(x => x.UpdateCounterAsync(userId, user))
             .Returns(Task.FromResult((5, true)));

            // Act
            var result = await _usersController.UpdateCounters(userId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            if (result is OkObjectResult okResult && okResult.Value != null)
            {
                var resultType = okResult.Value.GetType();

                var successProperty = resultType.GetProperty("success");
                var messageProperty = resultType.GetProperty("message");
                var countProperty = resultType.GetProperty("count");

                Assert.That(successProperty, Is.Not.Null, "The 'success' property was not found in the response.");
                Assert.That(messageProperty, Is.Not.Null, "The 'message' property was not found in the response.");
                Assert.That(countProperty, Is.Not.Null, "The 'count' property was not found in the response.");

                var successValue = successProperty?.GetValue(okResult.Value, null);
                var messageValue = messageProperty?.GetValue(okResult.Value, null);
                var countValue = countProperty?.GetValue(okResult.Value, null);

                Assert.That(successValue, Is.Not.Null, "The 'success' property is null.");
                Assert.That(successValue, Is.InstanceOf<bool>(), "The 'success' property is not a boolean.");
                Assert.That((bool)successValue, Is.True, "The 'success' property should be true.");

                Assert.That(messageValue, Is.Not.Null, "The 'message' property is null.");
                Assert.That(messageValue, Is.InstanceOf<string>(), "The 'message' property is not a string.");
                Assert.That(messageValue, Is.EqualTo("Counters updated successfully"), "The 'message' property has an unexpected value.");

                Assert.That(countValue, Is.Not.Null, "The 'count' property is null.");
                Assert.That(countValue, Is.InstanceOf<int>(), "The 'count' property is not an integer.");
                Assert.That((int)countValue, Is.EqualTo(5), "The 'count' property has an unexpected value.");
            }
            else
            {
                Assert.Fail("Expected OkObjectResult but got a different type or null value.");
            }

        }

        [Test]
        public async Task UpdateCounters_NullUserId_ReturnsBadRequest()
        {
            // Arrange
            string userId = null!;

            // Act
            var result = await _usersController.UpdateCounters(userId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var resultType = badRequestResult.Value.GetType();

                var successProperty = resultType.GetProperty("success");
                var messageProperty = resultType.GetProperty("message");

                Assert.That(successProperty, Is.Not.Null, "The 'success' property was not found in the response.");
                Assert.That(messageProperty, Is.Not.Null, "The 'message' property was not found in the response.");

                var successValue = successProperty?.GetValue(badRequestResult.Value, null);
                var messageValue = messageProperty?.GetValue(badRequestResult.Value, null);

                Assert.That(successValue, Is.Not.Null, "The 'success' property is null.");
                Assert.That(successValue, Is.InstanceOf<bool>(), "The 'success' property is not a boolean.");
                Assert.That((bool)successValue, Is.False, "The 'success' property should be false.");

                Assert.That(messageValue, Is.Not.Null, "The 'message' property is null.");
                Assert.That(messageValue, Is.InstanceOf<string>(), "The 'message' property is not a string.");
                Assert.That(messageValue, Is.EqualTo("Invalid input."), "The 'message' property has an unexpected value.");
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }

        }
        [Test]
        public async Task UpdateCounters_UserNotFound_ReturnsNotFound()
        {
           
            // Act
            var result = await _usersController.UpdateCounters(null!);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }

        [Test]
        public async Task UpdateCounters_UpdateFails_ReturnsBadRequest()
        {
            // Arrange
            var userId = "test-user-id";
            var user = new ApplicationUser { Id = userId };
            _userManager.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _mockUserService.Setup(x => x.UpdateCounterAsync(userId, user))
                .ReturnsAsync((0, false));

            // Act
            var result = await _usersController.UpdateCounters(userId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var resultType = badRequestResult.Value.GetType();

                var successProperty = resultType.GetProperty("success");
                var messageProperty = resultType.GetProperty("message");
                var countProperty = resultType.GetProperty("count");

                Assert.That(successProperty, Is.Not.Null, "The 'success' property was not found in the response.");
                Assert.That(messageProperty, Is.Not.Null, "The 'message' property was not found in the response.");
                Assert.That(countProperty, Is.Not.Null, "The 'count' property was not found in the response.");

                var successValue = successProperty?.GetValue(badRequestResult.Value, null);
                var messageValue = messageProperty?.GetValue(badRequestResult.Value, null);
                var countValue = countProperty?.GetValue(badRequestResult.Value, null);

                Assert.That(successValue, Is.Not.Null, "The 'success' property is null.");
                Assert.That(successValue, Is.InstanceOf<bool>(), "The 'success' property is not a boolean.");
                Assert.That((bool)successValue, Is.False, "The 'success' property should be false.");

                Assert.That(messageValue, Is.Not.Null, "The 'message' property is null.");
                Assert.That(messageValue, Is.InstanceOf<string>(), "The 'message' property is not a string.");
                Assert.That(messageValue, Is.EqualTo("Counters updated failed"), "The 'message' property has an unexpected value.");

                Assert.That(countValue, Is.Not.Null, "The 'count' property is null.");
                Assert.That(countValue, Is.InstanceOf<int>(), "The 'count' property is not an integer.");
                Assert.That((int)countValue, Is.EqualTo(0), "The 'count' property has an unexpected value.");
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }

        [Test]
        public async Task UpdateCounters_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var userId = "test-user-id";
            var exceptionMessage = "Test exception";
            _userManager.Setup(x => x.FindByIdAsync(userId))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _usersController.UpdateCounters(userId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var resultType = badRequestResult.Value.GetType();

                var successProperty = resultType.GetProperty("success");
                var messageProperty = resultType.GetProperty("message");

                Assert.That(successProperty, Is.Not.Null, "The 'success' property was not found in the response.");
                Assert.That(messageProperty, Is.Not.Null, "The 'message' property was not found in the response.");

                var successValue = successProperty?.GetValue(badRequestResult.Value, null);
                var messageValue = messageProperty?.GetValue(badRequestResult.Value, null);

                Assert.That(successValue, Is.Not.Null, "The 'success' property is null.");
                Assert.That(successValue, Is.InstanceOf<bool>(), "The 'success' property is not a boolean.");
                Assert.That((bool)successValue, Is.False, "The 'success' property should be false.");

                Assert.That(messageValue, Is.Not.Null, "The 'message' property is null.");
                Assert.That(messageValue, Is.InstanceOf<string>(), "The 'message' property is not a string.");
                Assert.That(messageValue, Is.EqualTo(exceptionMessage), "The 'message' property has an unexpected value.");
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }

            // Verify that the error was logged
            _mockLogger.Verify(
             x => x.Log(
                 LogLevel.Error,
                 It.IsAny<EventId>(),
                 It.Is<It.IsAnyType>((v, t) => true),
                 It.IsAny<Exception>(),
                 It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), // Fixed nullability
             Times.Once);

        }

        #endregion

        #region RequestDemo

        [Test]
        public async Task RequestDemo_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _usersController.ModelState.AddModelError("Name", "Required");
            var model = new ScheduleDemoViewModel();

            // Act
            var result = await _usersController.RequestDemo(model);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            var json = JsonConvert.SerializeObject(badRequestResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Assert.That(response!["success"], Is.False);
            Assert.That(response["message"], Is.EqualTo("Invalid data."));
        }


        [Test]
        public async Task RequestDemo_ReturnsOk_WhenRequestIsSuccessful()
        {
            // Arrange
            var model = GetDemoDetails();

            _mockMailManagementService
                .Setup(e => e.SendRequestDemoEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _usersController.RequestDemo(model);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var json = JsonConvert.SerializeObject(okResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Assert.That(response!["success"], Is.True);
            Assert.That(response["message"], Is.EqualTo("Thank you for scheduling a demo! We will get in touch with you shortly."));
        }

        [Test]
        public async Task RequestDemo_ReturnsBadRequest_WhenEmailServiceFails()
        {
            // Arrange
            var model = GetDemoDetails();

            _mockMailManagementService
                .Setup(e => e.SendRequestDemoEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _usersController.RequestDemo(model);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            var json = JsonConvert.SerializeObject(badRequestResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Assert.That(response!["success"], Is.False);
            Assert.That(response["message"], Is.EqualTo("Unable to process your demo request. Please try again later."));
        }


        [Test]
        public async Task RequestDemo_ReturnsBadRequest_OnException()
        {
            // Arrange
            var model = GetDemoDetails();

            _mockMailManagementService
                .Setup(e => e.SendRequestDemoEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Service failure"));

            // Act
            var result = await _usersController.RequestDemo(model);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }
        #endregion

        #region ContactUsForSubscription

        [Test]
        public async Task ContactUsForSubscription_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _usersController.ModelState.AddModelError("Name", "Required");
            var model = new ContactUsVieModel();

            // Act
            var result = await _usersController.ContactUsForSubscription(model);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            var json = JsonConvert.SerializeObject(badRequestResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Assert.That(response!["success"], Is.False);
            Assert.That(response["message"], Is.EqualTo("Invalid data."));
        }

        [Test]
        public async Task ContactUsForSubscription_ReturnsOk_WhenRequestIsSuccessful()
        {
            // Arrange
            var model = GetContactUsDetails();

            _mockMailManagementService
                .Setup(e => e.ContactUsForSubscription(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _usersController.ContactUsForSubscription(model);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var json = JsonConvert.SerializeObject(okResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Assert.That(response!["success"], Is.True);
            Assert.That(response["message"], Is.EqualTo("Your request has been received. We will contact you shortly."));
        }

        [Test]
        public async Task ContactUsForSubscription_ReturnsBadRequest_WhenEmailServiceFails()
        {
            // Arrange
            var model = GetContactUsDetails();

            _mockMailManagementService
                .Setup(e => e.ContactUsForSubscription(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _usersController.ContactUsForSubscription(model);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            var json = JsonConvert.SerializeObject(badRequestResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Assert.That(response!["success"], Is.False);
            Assert.That(response["message"], Is.EqualTo("Failed to submit your request. Please try again later."));
        }

        [Test]
        public async Task ContactUsForSubscription_ReturnsBadRequest_OnException()
        {
            // Arrange
            var model = GetContactUsDetails();

            _mockMailManagementService
                .Setup(e => e.ContactUsForSubscription(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Service failure"));

            // Act
            var result = await _usersController.ContactUsForSubscription(model);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }

        #endregion

        #region GET USER CONFIGURATION

        [Test]
        public async Task GetUserConfigurations_ValidUserId_ReturnsOkResultWithUserConfigurations()
        {
            // Arrange
            var userId = "123";
            var expectedUserConfigurationMock = new Mock<IUserConfigurations>();
            expectedUserConfigurationMock.SetupGet(u => u.Transcriptions).Returns(1);
            expectedUserConfigurationMock.SetupGet(u => u.SessionDurationLimit).Returns(60);
            expectedUserConfigurationMock.SetupGet(u => u.AvailableHours).Returns(10);
            expectedUserConfigurationMock.SetupGet(u => u.RealTimeResults).Returns(true);
            expectedUserConfigurationMock.SetupGet(u => u.PriorityAccessToTheLatestModels).Returns(true);
            expectedUserConfigurationMock.SetupGet(u => u.EarlyAccessToNewAIFeatures).Returns(true);
            expectedUserConfigurationMock.SetupGet(u => u.GenerateDocumentsWithConfidence).Returns(true);
            expectedUserConfigurationMock.SetupGet(u => u.WatermarkRemoval).Returns(true);
            expectedUserConfigurationMock.SetupGet(u => u.TailoredCapabilitiesAndAdvancedSupport).Returns(true);

            _mockUserService
                .Setup(s => s.SetUserConfiguration(userId, 1))
                .ReturnsAsync(true);

            _mockUserService
                .Setup(s => s.GetUserConfigurationAsync(userId))
                .ReturnsAsync(expectedUserConfigurationMock.Object);

            // Act
            var result = await _usersController.GetUserConfigurations(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            if (result is OkObjectResult okResult && okResult.Value is UserConfigurationsViewModel userConfigurations)
            {
                Assert.That(userConfigurations, Is.Not.Null);
                Assert.That(userConfigurations.Transcriptions, Is.EqualTo(expectedUserConfigurationMock.Object.Transcriptions));
                Assert.That(userConfigurations.SessionDurationLimit, Is.EqualTo(expectedUserConfigurationMock.Object.SessionDurationLimit));
                Assert.That(userConfigurations.AvailableHours, Is.EqualTo(expectedUserConfigurationMock.Object.AvailableHours));
                Assert.That(userConfigurations.RealTimeResults, Is.EqualTo(expectedUserConfigurationMock.Object.RealTimeResults));
            }
            else
            {
                Assert.Fail("Expected OkObjectResult with UserConfigurationsViewModel, but got null or unexpected type.");
            }

        }

        [Test]
        public async Task GetUserConfigurations_NullOrEmptyUserId_ReturnsBadRequest()
        {
            // Arrange
            string userId = string.Empty;

            // Act
            var result = await _usersController.GetUserConfigurations(userId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }

        }

        [Test]
        public async Task GetUserConfigurations_UserConfigNotFound_ReturnsOkWithEmptyConfiguration()
        {
            // Arrange
            string userId = "user123";
            _mockUserService.Setup(s => s.SetUserConfiguration(userId, 1)).ReturnsAsync(true);
            _mockUserService .Setup(s => s.GetUserConfigurationAsync(userId)).ReturnsAsync((IUserConfigurations?)null);

            // Act
            var result = await _usersController.GetUserConfigurations(userId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>(), "Expected result to be an OkObjectResult.");

            if (result is OkObjectResult okResult && okResult.Value is UserConfigurationsViewModel userConfigurations)
            {
                Assert.That(userConfigurations, Is.Not.Null, "Expected userConfigurations to be not null.");
            }
            else
            {
                Assert.Fail("Expected OkObjectResult with UserConfigurationsViewModel, but got null or an unexpected type.");
            }
        }

        [Test]
        public async Task GetUserConfigurations_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            string userId = "user123";
            var expectedException = new Exception("Test exception");

            _mockUserService.Setup(s => s.SetUserConfiguration(userId,1)).ThrowsAsync(expectedException);

            // Act
            var result = await _usersController.GetUserConfigurations(userId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }

        #endregion

        #region ShareReport

        [Test]
        public async Task ShareReport_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _usersController.ModelState.AddModelError("RecipientName", "Required");
            var report = new ReportSendViewModel();

            // Act
            var result = await _usersController.ShareReport(report);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            var json = JsonConvert.SerializeObject(badRequestResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Assert.That(response!["success"], Is.False);
            Assert.That(response["message"], Is.EqualTo("Invalid data."));
        }

        [Test]
        public async Task ShareReport_ReturnsOk_WhenEmailSentSuccessfully()
        {
            // Arrange
            var report = GetReportDetails();

            _mockMailManagementService
                .Setup(e => e.SendReportRequestEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _usersController.ShareReport(report);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var json = JsonConvert.SerializeObject(okResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Assert.That(response!["success"], Is.True);
            Assert.That(response["message"], Is.EqualTo("The Report han been send successfully"));
        }


        [Test]
        public async Task ShareReport_ReturnsBadRequest_WhenEmailServiceFails()
        {
            // Arrange
            var report = GetReportDetails();

            _mockMailManagementService
                .Setup(e => e.SendReportRequestEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _usersController.ShareReport(report);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            var json = JsonConvert.SerializeObject(badRequestResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Assert.That(response!["success"], Is.False);
            Assert.That(response["message"], Is.EqualTo("Unable to process. Please try again later."));
        }

        [Test]
        public async Task ShareReport_ReturnsBadRequest_OnException()
        {
            // Arrange
            var report = GetReportDetails();

            _mockMailManagementService
                .Setup(e => e.SendReportRequestEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Service failure"));

            // Act
            var result = await _usersController.ShareReport(report);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value != null)
            {
                var successProperty = badRequestResult.Value.GetType().GetProperty("success");
                if (successProperty != null)
                {
                    var successValue = successProperty.GetValue(badRequestResult.Value, null);
                    Assert.IsNotNull(successValue, "The 'success' property is null.");
                    Assert.IsInstanceOf<bool>(successValue, "The 'success' property is not a boolean.");
                    Assert.IsFalse((bool)successValue);
                }
                else
                {
                    Assert.Fail("The 'success' property was not found in the response.");
                }
            }
            else
            {
                Assert.Fail("Expected BadRequestObjectResult but got a different type or null value.");
            }
        }
        #endregion

        #endregion

        #region PRIVATE METHODS

        private ChangePasswordViewModel GetPasswordDetails()
        {
            return new ChangePasswordViewModel
            {
                UserId = "12345",
                Password = "OldPassword123!",
                NewPassword = "NewPassword123!",
                ConfirmPassword = "NewPassword123!"

            };

        }

        private ScheduleDemoViewModel GetDemoDetails()
        {
            return new ScheduleDemoViewModel
            {
                Name = "Test User",
                Email = "test123@gmail.com",
                Phone = "1234567890",
                CountryCode = "+91",
                Requirements = "Demo requirements"
            };
        }

        private ContactUsVieModel GetContactUsDetails()
        {
            return new ContactUsVieModel
            {
                Name = "John Doe",
                Email = "john.doe@gmail.com",
                Phone = "1234567890",
                CountryCode = "+91",
                AdditionalNotes = "Additional notes",
                SelectedPlan = "Premium"
            };
        }

        private ReportSendViewModel GetReportDetails()
        {
            return new ReportSendViewModel
            {
                RecipientName = "John Doe",
                Email = "john.doe@gmail.com",
                HospitalName = "Test Hospital",
                HospitalAddress = "123 Street",
                ReportName = "Summery Report",
                PatientName = "Jane Smith",
                DoctorName = "Dr. Watson",
                ConsultationDate = "12/02/24",
                file = "report.pdf"
            };
        }
        #endregion
    }

}
