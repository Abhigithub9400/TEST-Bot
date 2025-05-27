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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediAssistApplicationTest.Services
{
    [TestFixture]
    public class ProfileManagementServiceTest
    {
        #region PRIVATE INSTANCE FIELD

        private Mock<IUserRepository> _mockUserRepository;
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<MediAssistDbContext> _mockDbContext;
        private Mock<ILogger<ProfileManagementService>> _mockLogger;
        private ProfileManagementService _service;

        #endregion

        #region TEST SETUP

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _mockDbContext = new Mock<MediAssistDbContext>(new DbContextOptions<MediAssistDbContext>());
            _mockLogger = new Mock<ILogger<ProfileManagementService>>();

            _service = new ProfileManagementService(
                _mockUserRepository.Object,
                _mockUserManager.Object,
                _mockDbContext.Object,
                _mockLogger.Object);
        }

        #endregion

        #region FINDUSERASYNC

        [Test]
        public void ProfileManagementService_Constructor_ShouldInitializeSuccessfully()
        {
            // Act
            var profileService = new ProfileManagementService(
                _mockUserRepository.Object,
                _mockUserManager.Object,
                _mockDbContext.Object,
                _mockLogger.Object
            );

            // Assert
            Assert.NotNull(profileService);
        }


        [Test]
        public async Task FindUserAsync_ShouldReturnUser_WhenUserExists()
        {
            var userId = "user123";
            var user = new ApplicationUser { Id = userId };

            _mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);

            var result = await _service.FindUserAsync(userId);

            Assert.AreEqual(user, result);
        }

        #endregion

        #region FINDUSERDEAILSASYNC

        [Test]
        public async Task FindUserDetailsAsync_ShouldReturnUserDetails_WhenDetailsExist()
        {
            var userId = "user123";
            var userDetails = new DoctorProfile { UserId = userId };

            _mockUserRepository.Setup(m => m.GetUserDetailsbyIdAsync(userId)).ReturnsAsync(userDetails);

            var result = await _service.FindUserDetailsAsync(userId);

            Assert.AreEqual(userDetails, result);
        }

        #endregion

        #region FINDUSERFEEDBACKASYNC

        [Test]
        public async Task FindUserFeedBackAsync_ShouldReturnFeedbacks_WhenFeedbacksExist()
        {
            var userId = "user123";
            var feedbacks = new List<Feedback>
        {
            new Feedback { Id = 1, UserId = userId }
        };

            _mockUserRepository.Setup(m => m.GetUserFeedBacksByIdAsync(userId)).ReturnsAsync(feedbacks);

            var result = await _service.FindUserFeedBackAsync(userId);

            Assert.AreEqual(feedbacks, result);
        }

        #endregion

        #region FINDUSERCLINICDETAILSASYNC

        [Test]
        public async Task FindUserClinicDetailsAsync_ShouldReturnClinicDetails_WhenClinicDetailsExist()
        {
            var clinicId = 1;
            var clinicDetails = new Clinic { Id = 1, Address = "Cliic Address" };

            _mockUserRepository.Setup(m => m.GetUserClinicDetailsByIdAsync(clinicId)).ReturnsAsync(clinicDetails);

            var result = await _service.FindUserClinicDetailsAsync(clinicId);

            Assert.AreEqual(clinicDetails, result);
        }

        #endregion

        #region CHECKWHETHERUSERHISTORYISEXIST

        [Test]
        public async Task CheckWhetherUserHistoryIsExist_ShouldReturnTrue_WhenHistoryExists()
        {
            var emailId = "test@example.com";

            _mockUserRepository.Setup(m => m.CheckWhetherUserHistoryIsExist(emailId)).ReturnsAsync(true);

            var result = await _service.CheckWhetherUserHistoryIsExist(emailId);

            Assert.IsTrue(result);
        }

        #endregion

        #region UPDATEPROFILEASYNC

        [Test]
        public async Task UpdateProfileAsync_ShouldReturnOk_WhenUpdateSucceeds()
        {
            // Arrange
            var userId = "user123";
            var updateUserDetails = Mock.Of<IUpdateUserDetails>(u =>
                u.Title == 1 &&
                u.Gender == 1 &&
                u.DOB == new DateTime(1985, 1, 1) &&
                u.Image == null &&
                u.LicenseNumber == "12345" &&
                u.MedicalCredentials == 1 &&
                u.Specialization == "Cardiology");

            // Set up the in-memory database
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var inMemoryContext = new MediAssistDbContext(options);

            // Seed the database with initial data
            var doctorProfile = new DoctorProfile
            {
                UserId = userId,
                Title = 1,
                Gender = 1,
                DOB = new DateTime(1985, 1, 1),
                Specialization = "Cardiology" // Required property
            };
            inMemoryContext.DoctorProfiles.Add(doctorProfile);
            await inMemoryContext.SaveChangesAsync();

            // Set up the repository and service
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(m => m.GetUserTitlebyIdAsync(It.IsAny<int?>()))
                              .ReturnsAsync(new UserTitle());

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var mockLogger = new Mock<ILogger<ProfileManagementService>>();

            var service = new ProfileManagementService(
                mockUserRepository.Object,
                mockUserManager.Object,
                inMemoryContext,
                mockLogger.Object);

            // Act
            var result = await service.UpdateProfileAsync(userId, updateUserDetails);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.HttpStatusCode);
        }

        #endregion

        #region DELETEUSERACCOUNT

        [Test]
        public async Task DeleteUserAccount_ShouldReturnSuccess_WhenUserDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new MediAssistDbContext(options);

            var userRepositoryMock = new Mock<IUserRepository>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            var loggerMock = new Mock<ILogger<ProfileManagementService>>();

            var service = new ProfileManagementService(
                userRepositoryMock.Object,
                userManagerMock.Object,
                context,
                loggerMock.Object);

            var user = new ApplicationUser { Id = "user123", UserName = "test@example.com", FullName = "Test User" };

            // Set up repository and UserManager mock behavior
            userRepositoryMock.Setup(m => m.CheckWhetherUserHistoryIsExist(user.UserName)).ReturnsAsync(false);
            userManagerMock.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await service.DeleteUserAccount(user);

            // Assert
            Assert.IsTrue(result.Succeeded);

            // Additional validation
            var userHistory = context.UsersHistories.FirstOrDefault();
            Assert.IsNotNull(userHistory);
            Assert.AreEqual("Test User", userHistory.Name);
            Assert.AreEqual("test@example.com", userHistory.EmailAddress);
        }

        #endregion
    }
}
