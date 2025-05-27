using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Services;
using MediAssist.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MediAssistApplicationTest.Services
{
    [TestFixture]
    public class SettingsServiceTest
    {
        #region PRIVATE INSTANCE FIELD

        private MediAssistDbContext _dbContext;
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<ILogger<SettingsService>> _mockLogger;
        private SettingsService _settingsService;

        #endregion

        #region TEST SETUP

        [SetUp]
        public void Setup()
        {
            // Use InMemoryDatabase for testing
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb") // Give the in-memory database a name
                .Options;

            _dbContext = new MediAssistDbContext(options);

            // Initialize Mock dependencies
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _mockUserRepository = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILogger<SettingsService>>();

            _settingsService = new SettingsService(_dbContext, _mockUserManager.Object, _mockUserRepository.Object, _mockLogger.Object);
        }

        #endregion

        #region TEST TEAR DOWN

        [TearDown]
        public void TearDown()
        {
            // Dispose of the DbContext to avoid memory leaks
            _dbContext.Dispose();
        }

        #endregion

        #region UPDATECLINIC

        [Test]
        public void SettingsService_Constructor_ShouldInitializeSuccessfully()
        {
            // Act
            var settingsService = new SettingsService(
                _dbContext,
                _mockUserManager.Object,
                _mockUserRepository.Object,
                _mockLogger.Object
            );

            // Assert
            Assert.NotNull(settingsService);
        }

        [Test]
        public async Task UpdateClinic_WhenDoctorProfileNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var clinicDetails = new Mock<IClinicDetails>();
            clinicDetails.Setup(d => d.ClinicAddress).Returns("test address");
            clinicDetails.Setup(d => d.CountryCode).Returns("+91");
            clinicDetails.Setup(d => d.Logo).Returns("someLogoInBase64");  
            clinicDetails.Setup(d => d.ClinicName).Returns("Name");  
            clinicDetails.Setup(d => d.PhoneNumber).Returns("8934567389");  

            clinicDetails.Setup(d => d.UserId).Returns("TestuserId");

            // Use InMemory database for testing
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new MediAssistDbContext(options);

            // Add Clinic details to the in-memory db
            await dbContext.SaveChangesAsync(); // Saving initial state, if needed.

            var settingsService = new SettingsService(dbContext, _mockUserManager.Object, _mockUserRepository.Object, _mockLogger.Object);

            // Act
            var result = await settingsService.UpdateClinic(clinicDetails.Object);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Doctor profile not found", result.Message);
        }

        [Test]
        public async Task UpdateClinic_WhenSuccessful_ReturnsSuccessResponse()
        {
            // Arrange
            var clinicDetails = new Mock<IClinicDetails>();

            // Ensure that the properties you need are mocked
            clinicDetails.Setup(d => d.UserId).Returns("userId");
            clinicDetails.Setup(d => d.Logo).Returns("someLogoInBase64"); // Mocking Logo property if needed
            clinicDetails.Setup(d => d.ClinicName).Returns("ClinicName");
            clinicDetails.Setup(d => d.ClinicAddress).Returns("Clinic Address");
            clinicDetails.Setup(d => d.PhoneNumber).Returns("1234567890");
            clinicDetails.Setup(d => d.CountryCode).Returns("+91");

            // Create a doctor profile with required properties
            var doctorProfile = new DoctorProfile
            {
                UserId = "userId",
                ClinicId = null,
                Specialization = "General Medicine",  // Provide the required Specialization field
            };

            // Create a clinic with required properties, including Logo
            var clinic = new Clinic
            {
                Name = "ClinicName",
                Address = "Clinic Address", // Add Address
                PhoneNumber = "1234567890", // Add PhoneNumber
                Logo = ConvertBase64ToByteArray("someLogoInBase64"),
                CountryCode = "+91"
            };

            // Use InMemory database for testing
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new MediAssistDbContext(options);

            // Add the doctor profile and clinic details to the in-memory database
            dbContext.DoctorProfiles.Add(doctorProfile);
            dbContext.Clinics.Add(clinic); // Add the Clinic entity to the in-memory database
            dbContext.SaveChanges();

            // Create a settings service instance with the in-memory database
            var settingsService = new SettingsService(dbContext, _mockUserManager.Object, _mockUserRepository.Object, _mockLogger.Object);

            // Act
            var result = await settingsService.UpdateClinic(clinicDetails.Object);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Clinic details saved successfully", result.Message);
        }

        #endregion

        #region GETCLINICBYUSERIDASYNC

        [Test]
        public async Task GetClinicByUserIdAsync_WhenClinicDetailsNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userId = "userIdTest";

            // Create a doctor profile without a clinic
            var doctorProfile = new DoctorProfile
            {
                UserId = userId,
                Specialization = "General Medicine",  // Provide a value for 'Specialization'
                ClinicId = null,
                Clinic = null
            };

            // Use InMemory database for testing
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            await using var dbContext = new MediAssistDbContext(options);

            // Add doctor profile to the in-memory database
            dbContext.DoctorProfiles.Add(doctorProfile);
            await dbContext.SaveChangesAsync();

            // Initialize the service
            var settingsService = new SettingsService(dbContext, _mockUserManager.Object, _mockUserRepository.Object, _mockLogger.Object);

            // Act
            var result = await settingsService.GetClinicByUserIdAsync(userId);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Clinic details not found", result.Message);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task GetClinicByUserIdAsync_WhenDoctorProfileFound_ReturnsSuccessResponse()
        {
            // Arrange
            var userId = "userId";

            // Create a mock DoctorProfile with all required fields, including 'Specialization' and 'Clinic'
            var doctorProfile = new DoctorProfile
            {
                UserId = userId,
                Specialization = "General Medicine",  // Provide a value for 'Specialization'
                ClinicId = 1,  // Assuming ClinicId is 1, modify as per your model
                Clinic = new Clinic
                {
                    Name = "Test Clinic",
                    Address = "123 Main St",
                    PhoneNumber = "123-456-7890",
                    Logo = new byte[] { 0x20, 0x20 },
                    CountryCode = "+91"
                }
            };

            // Use InMemory database for testing
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new MediAssistDbContext(options);

            // Add the DoctorProfile to the in-memory database
            await dbContext.DoctorProfiles.AddAsync(doctorProfile);
            await dbContext.SaveChangesAsync();

            var settingsService = new SettingsService(dbContext, _mockUserManager.Object, _mockUserRepository.Object, _mockLogger.Object);

            // Act
            var result = await settingsService.GetClinicByUserIdAsync(userId);

            // Assert
            Assert.IsTrue(result.Success);  // Assert that the response is successful
            Assert.AreEqual("Clinic details fetched successfully", result.Message);  // Assert the success message
            Assert.IsNotNull(result.Data);  // Assert that the data is not null
            Assert.AreEqual("Test Clinic", result.Data.Name);  // Assert that the correct clinic details are returned
        }

        #endregion

        #region GETCLINICANDDOCTORDETAILSASYNC

        [Test]
        public async Task GetClinicAndDoctorDetailsAsync_WhenDoctorProfileNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var user = new ApplicationUser { Id = "userId", FullName = "John Doe" };

            // Use InMemory database for testing
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new MediAssistDbContext(options);

            // Add Clinic details to the in-memory db
            await dbContext.SaveChangesAsync(); // Saving initial state, if needed.

            var settingsService = new SettingsService(dbContext, _mockUserManager.Object, _mockUserRepository.Object, _mockLogger.Object);

            // Act
            var result = await settingsService.GetClinicAndDoctorDetailsAsync(user);

            // Assert
            Assert.IsFalse(result.Success);
        }

        [Test]
        public async Task GetClinicAndDoctorDetailsAsync_WhenDoctorProfileFound_ReturnsSuccessResponse()
        {
            // Arrange
            var user = new ApplicationUser { Id = "userId", FullName = "John Doe" };

            // Create a mock DoctorProfile with all required fields, including 'Signature', 'Clinic', and 'Title'
            var doctorProfile = new DoctorProfile
            {
                UserId = "userId",
                Specialization = "General Medicine",
                Title = 1,
                Signature = new byte[] { 0x01, 0x02, 0x03 },  // Sample signature byte array
                Clinic = new Clinic
                {
                    Name = "Test Clinic",
                    Address = "123 Main St",
                    Logo = new byte[] { 0x10, 0x20 },  // Sample logo byte array
                    PhoneNumber = "123-456-7890",
                    CountryCode = "+91"
                }
            };

            // Create a mock UserTitle
            var userTitle = new UserTitle { Abbreviations = "Dr" };

            // Setup the mock repository to return the mock DoctorProfile
            _mockUserRepository.Setup(repo => repo.GetUserTitlebyIdAsync(It.IsAny<int?>()))
                .ReturnsAsync(userTitle);

            // Use InMemory database for testing
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new MediAssistDbContext(options);

            // Add DoctorProfile and Clinic details to the in-memory db
            await dbContext.DoctorProfiles.AddAsync(doctorProfile);
            await dbContext.SaveChangesAsync();

            var settingsService = new SettingsService(dbContext, _mockUserManager.Object, _mockUserRepository.Object, _mockLogger.Object);

            // Act
            var result = await settingsService.GetClinicAndDoctorDetailsAsync(user);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Report details fetched successfully", result.Message);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("John Doe", result.Data.DoctorName);
            Assert.AreEqual("General Medicine", result.Data.DoctorSpecialization);
            Assert.AreEqual("Dr", result.Data.DoctorTitle);
            Assert.IsNotNull(result.Data.DoctorSignature);  // Assert that signature is properly converted to Base64
            Assert.AreEqual("Test Clinic", result.Data.HospitalName);
            Assert.AreEqual("123 Main St", result.Data.HospitalAddress);
            Assert.IsNotNull(result.Data.HospitalLogo);  // Assert that logo is properly converted to Base64
        }

        #endregion

        #region UPDATEDOCTORPROFILE

        [Test]
        public async Task UpdateDoctorProfile_WhenUserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var doctorDetailsSettings = new Mock<IDoctorProfileSettings>();
            doctorDetailsSettings.Setup(d => d.UserId).Returns("userId");

            _mockUserManager.Setup(um => um.FindByIdAsync("userId"))
                .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _settingsService.UpdateDoctorProfile(doctorDetailsSettings.Object);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("User not found", result.Message);
        }

        [Test]
        public async Task UpdateDoctorProfile_WhenDoctorProfileNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_DoctorProfileNotFound")
                .Options;

            var dbContext = new MediAssistDbContext(options);
            var doctorDetailsSettings = new Mock<IDoctorProfileSettings>();
            doctorDetailsSettings.Setup(d => d.UserId).Returns("TestuserId");

            var user = new ApplicationUser { Id = "TestuserId" };

            _mockUserManager.Setup(um => um.FindByIdAsync("TestuserId"))
                .ReturnsAsync(user);

            // Act
            var result = await _settingsService.UpdateDoctorProfile(doctorDetailsSettings.Object);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Doctor's profile not found", result.Message);
        }

        #endregion

        #region CHECKWHETHERSETTINGSUPDATED

        [Test]
        public async Task CheckWhetherSettingsUpdated_WhenSettingsNotUpdated_ReturnsFalse()
        {
            // Arrange
            var user = new ApplicationUser { Id = "userId" };

            // Mock the IUserRepository to return a DoctorProfile with Signature and ClinicId as null
            _mockUserRepository.Setup(repo => repo.GetUserDetailsbyIdAsync("userId"))
                .ReturnsAsync(new DoctorProfile { Signature = null, ClinicId = null });

            // Act
            var result = await _settingsService.CheckWhetherSettingsUpdated(user);

            // Assert
            Assert.IsFalse(result);  // Assert that the method returns false when settings are not updated
        }

        [Test]
        public async Task CheckWhetherSettingsUpdated_WhenSettingsUpdated_ReturnsTrue()
        {
            // Arrange
            var user = new ApplicationUser { Id = "userId" };

            // Mock the IUserRepository to return a DoctorProfile with Signature and ClinicId set to non-null values
            _mockUserRepository.Setup(repo => repo.GetUserDetailsbyIdAsync("userId"))
                .ReturnsAsync(new DoctorProfile { Signature = Encoding.UTF8.GetBytes("ValidSignature"), ClinicId = 1 });

            // Act
            var result = await _settingsService.CheckWhetherSettingsUpdated(user);

            // Assert
            Assert.IsTrue(result);  // Assert that the method returns true when settings are updated
        }

        #endregion

        #region CONVERTBASE64TOBYTEARRAY

        public byte[] ConvertBase64ToByteArray(string base64)
        {
            return Convert.FromBase64String(base64);
        }

        #endregion
    }
}
