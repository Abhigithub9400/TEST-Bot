using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.DbContext;
using MediAssist.UI.Controllers;
using MediAssist.UI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static MediAssist.UI.Controllers.ProfileManagementController;

namespace MediAssistPresentationTest.Controller
{
    [TestFixture]
    public class ProfileManagementControllerTest
    {
        #region PRIVATE INSTANCE FIELD

        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<MediAssistDbContext> _contextMock;
        private Mock<IProfileManagementService> _mockProfileService;
        private Mock<ILogger<ProfileManagementController>> _mockLogger;
        private ProfileManagementController _controller;
        private MethodInfo _validateDateOfBirthMethod;

        #endregion

        #region TEST SETUP

        [SetUp]
        public void SetUp()
        {
            _mockUserManager = MockHelpers.MockUserManager<ApplicationUser>();
            _mockProfileService = new Mock<IProfileManagementService>();
            _mockLogger = new Mock<ILogger<ProfileManagementController>>();

            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _contextMock = new Mock<MediAssistDbContext>(options);

            _controller = new ProfileManagementController(
                _mockUserManager.Object,
                _contextMock.Object,
                _mockProfileService.Object,
                _mockLogger.Object
            );

            _validateDateOfBirthMethod = typeof(ProfileManagementController)
       .GetMethod("ValidateDateOfBirth", BindingFlags.NonPublic | BindingFlags.Instance)
       ?? throw new InvalidOperationException("Method 'ValidateDateOfBirth' not found in ProfileManagementController.");
        }

        #endregion

        #region MOCK HELPERS

        public static class MockHelpers
        {
            public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
            {
                var store = new Mock<IUserStore<TUser>>();
                var mockUserManager = new Mock<UserManager<TUser>>(
                    store.Object,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!
                );

                return mockUserManager;
            }
        }

        #endregion

        #region MOCK DBSET

        // Example of mocking a DbSet
        private DbSet<DoctorProfile> GetMockedDoctorProfiles()
        {
            var data = new List<DoctorProfile>
            {
                new DoctorProfile
                {
                    UserId = "1",
                    User = new ApplicationUser
                    {
                        FullName = "Test Doctor",
                        Email = "testdoctor@gmail.com"
                    }
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<DoctorProfile>>();
            mockSet.As<IQueryable<DoctorProfile>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DoctorProfile>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DoctorProfile>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DoctorProfile>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet.Object;
        }

        #endregion

        #region TEST TEAR DOWN

        [TearDown]
        public void TearDown()
        {
         
            _contextMock.Object.Dispose();
        }

        #endregion

        #region ERROR RESPONSE

        public class ErrorResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }

        #endregion

        #region GETUSERDETAILS

        [Test]
        public async Task GetUserDetails_UserIdIsNull_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.GetUserDetails(null!);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result, "Expected BadRequestObjectResult but got something else.");

            var badRequest = result as BadRequestObjectResult;
            Assert.NotNull(badRequest, "BadRequestObjectResult is null.");

            // Deserialize the response to the model
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorResponse>(
                Newtonsoft.Json.JsonConvert.SerializeObject(badRequest.Value)
            );

            Assert.NotNull(response, "Response is null.");
            Assert.That(response.Message, Is.EqualTo("Invalid or missing UserId."), "Message property does not match expected value.");
        }

        [Test]
        public async Task GetUserDetails_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _controller.GetUserDetails("12345");

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult, "NotFoundObjectResult is null.");

            // Deserialize the response to the model
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorResponse>(
                Newtonsoft.Json.JsonConvert.SerializeObject(notFoundResult.Value)
            );

            Assert.NotNull(response, "Response is null.");
            Assert.That(response.Message, Is.EqualTo("User not found."), "Message property does not match expected value.");
        }

        [Test]
        public async Task GetUserDetails_UserFound_ReturnsOkWithDetails()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "12345",
                Email = "test@example.com",
                FullName = "John Doe",
                FirstName = "John", 
                LastName = "Doe",    
                MiddleName = "M"  
            };

            var doctorProfile = new DoctorProfile
            {
                UserId = "12345",
                Title = 1,  
                Gender = 1,
                DOB = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc), 
                LicenseNumber = "LN12345",
                MedicalCredentials = 1,
                Specialization = "Cardiology",
                Image = null,
                Signature = null
            };

            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase("TestDatabase")  
                .EnableSensitiveDataLogging() 
                .Options;

            using var context = new MediAssistDbContext(options);

            context.Users.Add(user);
            context.DoctorProfiles.Add(doctorProfile);
            await context.SaveChangesAsync();

            var mockProfileManagementService = new Mock<IProfileManagementService>();
            var mockLogger = new Mock<ILogger<ProfileManagementController>>();

            _mockUserManager.Setup(m => m.FindByIdAsync("12345")).ReturnsAsync(user);

            var controller = new ProfileManagementController(
                _mockUserManager.Object,  
                context,  
                mockProfileManagementService.Object,
                mockLogger.Object
            );

            // Act
            var result = await controller.GetUserDetails("12345");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);

            var json = JsonConvert.SerializeObject(okResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.That(response!["success"], Is.EqualTo(true));
        }

        #endregion

        #region UPDATEPROFILE

        [Test]
        public async Task UpdateProfile_InvalidModel_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.UpdateProfile(null!);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequest = result as BadRequestObjectResult;

            Assert.NotNull(badRequest, "BadRequestObjectResult is null.");

            // Deserialize the response to the model
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorResponse>(
                Newtonsoft.Json.JsonConvert.SerializeObject(badRequest.Value)
            );

            Assert.NotNull(response, "Response is null.");
            Assert.That(response.Message, Is.EqualTo("Profile data is required."), "Message property does not match expected value.");
        }

        [Test]
        public async Task UpdateProfile_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var mockProfileService = new Mock<IProfileManagementService>();
            var mockLogger = new Mock<ILogger<ProfileManagementController>>();

            var updateProfileViewModel = new UpdateProfileViewModel
            {
                UserId = "12345",
                Title = 1,
                Gender = 1,
                Image = "imageUrl",
                DOB = DateTime.Now,
                LicenseNumber = "ABC123",
                MedicalCredentials = 1,
                Specialization = "Cardiology"
            };

            mockProfileService.Setup(service => service.FindUserAsync("1")).ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _controller.UpdateProfile(updateProfileViewModel);

            // Assert

            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult, "NotFoundObjectResult is null.");

            // Deserialize the response to the model
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorResponse>(
                Newtonsoft.Json.JsonConvert.SerializeObject(notFoundResult.Value)
            );

            Assert.NotNull(response, "Response is null.");
            Assert.That(response.Message, Is.EqualTo("User not found."), "Message property does not match expected value.");
        }

        [Test]
        public async Task UpdateProfile_ValidInput_ReturnsOk()
        {
            // Arrange

            var updateProfileViewModel = new UpdateProfileViewModel
            {
                UserId = "12345",
                Title = 1,
                Gender = 1,
                Image = "imageUrl",
                DOB = DateTime.Now,
                LicenseNumber = "ABC123",
                MedicalCredentials = 1,
                Specialization = "Cardiology"
            };

            var user = new ApplicationUser { Id = updateProfileViewModel.UserId, FullName = "John Deo" };

            _mockProfileService.Setup(service => service.FindUserAsync(updateProfileViewModel.UserId)).ReturnsAsync(user);
            _mockProfileService.Setup(service => service.UpdateProfileAsync(It.IsAny<string>(), It.IsAny<UpdateUserDetails>()))
                   .ReturnsAsync((HttpStatusCode.OK, new UserTitle { Abbreviations = "Dr." }));

            // Act
            var result = await _controller.UpdateProfile(updateProfileViewModel);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var okResult = result as ObjectResult;

            Assert.IsNotNull(okResult);

            var json = JsonConvert.SerializeObject(okResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.IsFalse((bool)response!["success"]);
            Assert.That(response["message"], Is.EqualTo("An error occurred while updating the profile."));
        }


        [TestCase("1990-01-01", true, "")] // Valid date
        [TestCase("2010-01-01", false, "You must be at least 18 years old.")] // Under 18
        [TestCase("2004-02-29", true, "")] // Valid leap year
        public void ValidateDateOfBirth_ReturnsExpectedResult(string dobString, bool expectedIsValid, string expectedError)
        {
            // Arrange
            bool isValidDate = DateTime.TryParseExact(
                dobString,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime dob
            );

            object param = isValidDate ? dob : dobString; // Assign correct parameter type

            // Act
            var result = _validateDateOfBirthMethod?.Invoke(_controller, new object[] { param });

            // Ensure result is not null before unboxing
            Assert.That(result, Is.Not.Null, "Method invocation returned null.");

            // Convert result safely using pattern matching
            if (result is ValueTuple<bool, string> tupleResult)
            {
                var (isValid, errorMessage) = tupleResult;

                // Assert
                Assert.That(isValid, Is.EqualTo(expectedIsValid));
                Assert.That(errorMessage, Is.EqualTo(expectedError));
            }
            else
            {
                Assert.Fail("Unexpected result type.");
            }
        }


        #endregion

        #region DELETEPROFILE

        [Test]
        public async Task DeleteProfile_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockProfileService.Setup(x => x.FindUserAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _controller.DeleteProfile("12345");

            // Assert

            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult, "NotFoundObjectResult is null.");

            // Deserialize the response to the model
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorResponse>(
                Newtonsoft.Json.JsonConvert.SerializeObject(notFoundResult.Value)
            );

            Assert.NotNull(response, "Response is null.");
            Assert.That(response.Success, Is.False, "Success property does not match expected value.");
            Assert.That(response.Message, Is.EqualTo("User not found."), "Message property does not match expected value.");
        }

        [Test]
        public async Task DeleteProfile_UserDeletedSuccessfully_ReturnsOk()
        {
            // Arrange
            var user = new ApplicationUser { Id = "12345" };
            _mockProfileService.Setup(x => x.FindUserAsync(user.Id)).ReturnsAsync(user);
            _mockProfileService.Setup(x => x.DeleteUserAccount(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.DeleteProfile(user.Id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);

            var json = JsonConvert.SerializeObject(okResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.IsTrue((bool)response!["success"]);
            Assert.That(response["message"], Is.EqualTo("Your account has been successfully deleted. Thank you for using MediAssist AI."));
        }

        #endregion

    }
}
