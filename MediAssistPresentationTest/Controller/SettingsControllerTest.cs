using Azure;
using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.DbContext;
using MediAssist.UI.Controllers;
using MediAssist.UI.Validator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssistPresentationTest.Controller
{
    [TestFixture]
    public class SettingsControllerTest
    {
        #region PRIVATE INSTANCE FIELD

        private Mock<ISettingsService> _mockSettingsService;
        private MediAssistDbContext _mockDbContext;
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<ILogger<SettingsController>> _mockLogger;
        private SettingsController _controller;

        #endregion

        #region TEST SETUP

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            _mockDbContext = new MediAssistDbContext(options);

            _mockSettingsService = new Mock<ISettingsService>();
            _mockUserManager = MockUserManager<ApplicationUser>();
            _mockLogger = new Mock<ILogger<SettingsController>>();

            _controller = new SettingsController(
                _mockSettingsService.Object,
                _mockDbContext,
                _mockUserManager.Object,
                _mockLogger.Object
            );

        }

        #endregion

        #region TEAR DOWN

        [TearDown]
        public void TearDown()
        {
            _mockDbContext.Database.EnsureDeleted();
            _mockDbContext.Dispose();
        }

        #endregion

        #region PRIVATEMOCK

        private Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        }

        #endregion

        #region UPDATECLINIC

        [Test]
        public async Task UpdateClinic_ReturnOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var clinicDetails = GetClinicDetails();

            var serviceResponse = new ServiceResponse<Clinic>
            {
                Success = true,
                Message = "Clinic details saved successfully",
                Data = new Clinic { Name = "Test Clinic" }
            };

            _mockSettingsService
                .Setup(s => s.UpdateClinic(It.IsAny<ClinicDetails>()))
                .ReturnsAsync(serviceResponse);

            // Act
            var result = await _controller.UpdateClinic(clinicDetails);

            // Assert
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult, "OkObjectResult is null.");
            Assert.NotNull(objectResult!.Value, "Value in OkObjectResult is null.");

            Assert.That(objectResult.Value!.GetType().GetProperty("success")?.GetValue(objectResult.Value, null), Is.True);

        }

        [Test]
        public async Task UpdateClinic_ReturnBadRequest_WhenUpdateFails()
        {
            // Arrange
            var clinicDetails = GetClinicDetails();

            var serviceResponse = new ServiceResponse<Clinic>
            {
                Success = false,
                Message = "Failed to update clinic details",
                Data = null!
            };

            _mockSettingsService
                .Setup(s => s.UpdateClinic(It.IsAny<ClinicDetails>()))
                .ReturnsAsync(serviceResponse);

            // Act
            var result = await _controller.UpdateClinic(clinicDetails);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var objectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(objectResult);
            var response = objectResult.Value as ServiceResponse<Clinic>;
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
            Assert.That(response.Message, Is.EqualTo("Failed to update clinic details"), "Message property does not match expected value.");

        }

        [Test]
        public async Task UpdateClinic_ReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var clinicDetails = GetClinicDetails();

            _mockSettingsService
                .Setup(s => s.UpdateClinic(It.IsAny<ClinicDetails>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.UpdateClinic(clinicDetails);

           
            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult, "ObjectResult is null.");
            Assert.NotNull(objectResult!.Value, "Value in OkObjectResult is null.");
            Assert.That(objectResult.Value!.GetType().GetProperty("success")?.GetValue(objectResult.Value, null), Is.False);

        }

        [Test]
        public async Task UpdateClinic_ReturnBadRequest_WhenInputIsNull()
        {
            // Act
            var result = await _controller.UpdateClinic(null!);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult, "OkObjectResult is null.");
            Assert.NotNull(objectResult!.Value, "Value in OkObjectResult is null.");
            Assert.That(objectResult.Value!.GetType().GetProperty("success")?.GetValue(objectResult.Value, null), Is.False);
        }

        [Test]
        public async Task UpdateClinic_ReturnUnauthorized_WhenUserNotAuthorized()
        {
            // Arrange
            var clinicDetails = GetClinicDetails();

            _mockSettingsService
                .Setup(s => s.UpdateClinic(It.IsAny<ClinicDetails>()))
                .ThrowsAsync(new UnauthorizedAccessException("User is not authorized"));

            // Act
            var result = await _controller.UpdateClinic(clinicDetails);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.That(objectResult!.StatusCode, Is.EqualTo(500), "Status code does not match expected value.");
            Assert.NotNull(objectResult.Value, "Value in OkObjectResult is null.");
            Assert.That(objectResult.Value!.GetType().GetProperty("success")?.GetValue(objectResult.Value, null), Is.False);

        }

        #endregion

        #region GETCLINIC

        [Test]
        public async Task GetClinic_ReturnNotFound_WhenUserIdIsNull()
        {
            // Arrange
            string userId = null!;

            // Act
            var result = await _controller.GetClinic(userId!);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var objectResult = result as NotFoundObjectResult;         
            Assert.NotNull(objectResult, "ObjectResult is null.");
            Assert.NotNull(objectResult!.Value, "Value in ObjectResult is null.");
            Assert.That(objectResult.Value!.GetType().GetProperty("success")?.GetValue(objectResult.Value, null), Is.False);
        }

        [Test]
        public async Task GetClinic_ReturnBadRequest_WhenServiceCallFails()
        {
            // Arrange
            string userId = "268e9740-ca08-4570-9dba-c5b358c468b0";
            var serviceResponse = new ServiceResponse<Clinic>
            {
                Success = false,
                Message = "Failed to fetch clinic details",
                Data = null!
            };

            _mockSettingsService
                .Setup(s => s.GetClinicByUserIdAsync(userId))
                .ReturnsAsync(serviceResponse);

            // Act
            var result = await _controller.GetClinic(userId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var objectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(objectResult);
            var response = objectResult.Value as ServiceResponse<Clinic>;
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
            Assert.That(response.Message, Is.EqualTo("Failed to fetch clinic details"), "Message property does not match expected value.");
        }

        [Test]
        public async Task GetClinic_ReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            string userId = "268e9740-ca08-4570-9dba-c5b358c468b0";

            _mockSettingsService
                .Setup(s => s.GetClinicByUserIdAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetClinic(userId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult, "ObjectResult is null.");
            Assert.NotNull(objectResult!.Value, "Value in ObjectResult is null.");
            Assert.That(objectResult.Value!.GetType().GetProperty("success")?.GetValue(objectResult.Value, null), Is.False);
        }

        [Test]
        public async Task GetClinic_ReturnOk_WhenClinicDetailsRetrievedSuccessfully()
        {
            // Arrange
            string userId = "268e9740-ca08-4570-9dba-c5b358c468b0";
            var serviceResponse = new ServiceResponse<Clinic>
            {
                Success = true,
                Data = new Clinic
                {
                    Name = "Test Clinic",
                    Address = "123 Test Street",
                    PhoneNumber = "1234567890",
                    Logo = new byte[] { 1, 2, 3, 4 }
                }
            };

            _mockSettingsService
                .Setup(s => s.GetClinicByUserIdAsync(userId))
                .ReturnsAsync(serviceResponse);

            // Act
            var result = await _controller.GetClinic(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var objectResult = result as OkObjectResult;
            Assert.IsNotNull(objectResult);

            var json = JsonConvert.SerializeObject(objectResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.Multiple(() =>
            {
                Assert.That(response!["clinicName"], Is.EqualTo("Test Clinic"), "Clinic name does not match.");
                Assert.That(response["clinicAddress"], Is.EqualTo("123 Test Street"), "Clinic address does not match.");
                Assert.That(response["phoneNumber"], Is.EqualTo("1234567890"), "Phone number does not match.");
                Assert.That(response["logo"], Is.EqualTo(Convert.ToBase64String(new byte[] { 1, 2, 3, 4 })), "Logo does not match.");
            });
        }


        #endregion

        #region UPDATE DOCTOR PROFILE

        [Test]
        public async Task UpdateDoctorProfile_ValidData_ReturnsOkResult()
        {
            // Arrange
            var doctorProfileSettings = new DoctorProfileSettings
            {
                UserId = "1",
                Specialization = "Cardiology",
                MedicalCredentials = 1
            };

            // Add the related MedicalCredentials to the in-memory database
            _mockDbContext.MedicalCredentials.Add(new DoctorMedicalCredentials { Id = 1, MedicalCredentials = "MD" });

            await _mockDbContext.SaveChangesAsync();  // Save changes to the in-memory database

            // Mock the settings service to simulate a successful profile update
            _mockSettingsService.Setup(x => x.UpdateDoctorProfile(It.IsAny<DoctorProfileSettings>()))
                .ReturnsAsync(new ServiceResponse<DoctorProfile> { Success = true, Message = "Doctor's information updated successfully" });

            // Act
            var result = await _controller.UpdateDoctorProfile(doctorProfileSettings);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;

            Assert.IsNotNull(objectResult);
            Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            var json = JsonConvert.SerializeObject(objectResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            // Ensure response is not null and access properties
            Assert.IsNotNull(response); // Ensure response is not null

            Assert.IsTrue((bool)response["success"]);
            Assert.That(response["message"], Is.EqualTo("Doctor's information updated successfully"));
        }

        [Test]
        public async Task UpdateDoctorProfile_ValidationFails_ReturnsBadRequest()
        {
            // Arrange
            var doctorProfileSettings = new DoctorProfileSettings
            {
                UserId = ""
            };

            // Act
            var result = await _controller.UpdateDoctorProfile(doctorProfileSettings);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.That(objectResult!.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            var response = objectResult.Value as ServiceResponse<DoctorProfile>;
            Assert.IsNotNull(response);
            Assert.That(response.Success, Is.EqualTo(false));
            Assert.That(response.Message, Is.EqualTo("An error occurred while updating the doctor's information."));
        }

        [Test]
        public async Task UpdateDoctorProfile_UpdateFails_ReturnsBadRequest()
        {
            // Arrange
            var doctorProfileSettings = new DoctorProfileSettings
            {
                // Populate with valid test data
                UserId = "1",
                Specialization = "Cardiology",
                MedicalCredentials = 1
            };

            // Simulate profile update failure
            _mockSettingsService.Setup(x => x.UpdateDoctorProfile(It.IsAny<DoctorProfileSettings>()))
                .ReturnsAsync(new ServiceResponse<DoctorProfile> { Success = false });

            // Act
            var result = await _controller.UpdateDoctorProfile(doctorProfileSettings);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var badRequestResult = result as ObjectResult;
            Assert.IsNotNull(badRequestResult);

            var response = badRequestResult.Value as ServiceResponse<DoctorProfile>;
            Assert.IsNotNull(response);
           Assert.That(response.Success, Is.EqualTo(false));
        }

        #endregion

        #region GET CLINIC AN DDOCTOR DETAILS

        [Test]
        public async Task GetClinicAndDoctorDetails_ReturnsNotFound_WhenUserIdIsNull()
        {
            // Act
            var response = await _controller.GetClinicAndDoctorDetails(null!);

            // Assert
            Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = (NotFoundObjectResult)response;
            Assert.That(notFoundResult.Value!.GetType().GetProperty("success")?.GetValue(notFoundResult.Value, null), Is.False);
        }

        [Test]
        public async Task GetClinicAndDoctorDetails_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "268e9740-ca08-4570-9dba";
            _mockUserManager
                .Setup(u => u.FindByIdAsync(userId))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _controller.GetClinicAndDoctorDetails(userId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.That(notFoundResult.Value!.GetType().GetProperty("success")?.GetValue(notFoundResult.Value, null), Is.False);
        }

        [Test]
        public async Task GetClinicAndDoctorDetails_ReportDetailsFailure_ReturnsBadRequest()
        {
            // Arrange
            var userId = "268e9740-ca08-4570-9dba-c5b358c468b0";
            var user = new ApplicationUser { Id = userId };

            _mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);

            var reportDetails = new ServiceResponse<IReportData>
            {
                Success = false,
                Message = "Details not found"
            };

            _mockSettingsService.Setup(s => s.GetClinicAndDoctorDetailsAsync(user)).ReturnsAsync(reportDetails);

            // Act
            var result = await _controller.GetClinicAndDoctorDetails(userId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.Value!.GetType().GetProperty("success")?.GetValue(badRequestResult.Value, null), Is.False);
        }

        [Test]
        public async Task GetClinicAndDoctorDetails_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var userId = "268e9740-ca08-4570-9dba-c5b358c468b0";

            _mockUserManager.Setup(m => m.FindByIdAsync(userId)).ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.GetClinicAndDoctorDetails(userId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));
            Assert.That(objectResult.Value!.GetType().GetProperty("success")?.GetValue(objectResult.Value, null), Is.False);
        }

        [Test]
        public async Task GetClinicAndDoctorDetails_ReportDetailsSuccess_ReturnsOk()
        {
            // Arrange
            var userId = "268e9740-ca08-4570-9dba-c5b358c468b0";
            var user = new ApplicationUser { Id = userId };

            _mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);

            var reportData = new ReportData
            {
                    DoctorName = "Dr. John Doe",
                    DoctorSpecialization = "Cardiology",
                    DoctorTitle = "MD",
                    DoctorSignature = "signature.png",
                    HospitalName = "General Hospital",
                    HospitalAddress = "123 Main St",
                    HospitalLogo = "logo.png"
            };

            var reportDetails = new ServiceResponse<IReportData>
            {
                Success = true,
                Message = "Report details fetched successfully",
                Data = reportData
            };
            _mockSettingsService.Setup(s => s.GetClinicAndDoctorDetailsAsync(user)).ReturnsAsync(reportDetails);

            // Act
            var result = await _controller.GetClinicAndDoctorDetails(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var json = JsonConvert.SerializeObject(okResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.Multiple(() =>
            {
                Assert.That(response!["success"], Is.True);
                Assert.That(response["DoctorName"], Is.EqualTo("Dr. John Doe"));
                Assert.That(response["DoctorSpecialization"], Is.EqualTo("Cardiology"));
                Assert.That(response["DoctorTitle"], Is.EqualTo("MD"));
                Assert.That(response["DoctorSignature"], Is.EqualTo("signature.png"));
                Assert.That(response["HospitalName"], Is.EqualTo("General Hospital"));
                Assert.That(response["HospitalAddress"], Is.EqualTo("123 Main St"));
                Assert.That(response["HospitalLogo"], Is.EqualTo("logo.png"));
            });

        }

        #endregion

        #region CHECK WHETHER SETTING SUPDATED

        [Test]
        public async Task CheckWhetherSettingsUpdated_ReturnsOk_WhenSettingsUpdated()
        {
            // Arrange
            var userId = "8469b2ad-75bf-43f1-97d5-6f90dc6b14db";
            var user = new ApplicationUser { Id = userId };
            _mockUserManager
                .Setup(u => u.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _mockSettingsService
                .Setup(s => s.CheckWhetherSettingsUpdated(user))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.CheckWhetherSettingsUpdated(userId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var json = JsonConvert.SerializeObject(okResult.Value);
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Assert.IsTrue((bool)response!["IsSettingsUpdated"]);
        }

        [Test]
        public async Task CheckWhetherSettingsUpdated_UserIdIsNull_ReturnsNotFound()
        {
            // Act
            var result = await _controller.CheckWhetherSettingsUpdated(null!);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.That(notFoundResult.Value!.GetType().GetProperty("success")?.GetValue(notFoundResult.Value, null), Is.False);
        }

        [Test]
        public async Task CheckWhetherSettingsUpdated_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var userId = "8469b2ad-75bf-43f1-97d5-6f90dc6b14";
            _mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _controller.CheckWhetherSettingsUpdated(userId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.That(notFoundResult.Value!.GetType().GetProperty("success")?.GetValue(notFoundResult.Value, null), Is.False);
        }

        [Test]
        public async Task CheckWhetherSettingsUpdated_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var userId = "8469b2ad-75bf-43f1-97d5-6f90dc6b14db";

            _mockUserManager.Setup(m => m.FindByIdAsync(userId)).ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.CheckWhetherSettingsUpdated(userId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));
            Assert.That(objectResult.Value!.GetType().GetProperty("success")?.GetValue(objectResult.Value, null), Is.False);
        }

        #endregion

        #region PRIVATE METHODS
        private ClinicDetails GetClinicDetails()
        {
            return new ClinicDetails
            {
                ClinicName = "Test Clinic",
                ClinicAddress = "123 Test Street",
                UserId = "268e9740-ca08-4570-9dba-c5b358c468b0",
                PhoneNumber = "1234567890",
                Logo = "data:image/png",
                CountryCode = "+91"
            };

        }

        private DoctorProfileSettings GetDoctorProfileSettings()
        {
            return new DoctorProfileSettings
            {
                Signature = "data:image/png",
                Specialization = "Cardio surgen",
                MedicalCredentials = 1,
                UserId = "8469b2ad-75bf-43f1-97d5-6f90dc6b14db"
            };
        }
        #endregion
    }
}
