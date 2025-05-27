using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.Application.Services;
using MediAssist.Configurations;
using MediAssist.DataAccess.Repository;
using MediAssist.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MediAssistApplicationTest.Services
{
    [TestFixture]
    public class UserServiceTest
    {
        #region PRIVATE INSTANCE FIELD

        private UserService _userService;
        private Mock<IUserRepository> _userRepositoryMock;
        private MediAssistDbContext _dbContext;
        private Mock<ILogger<UserService>> _loggerMock;
        private MethodInfo _splitFullNameMethod;
        private MethodInfo _setFeatureValuesMethod;
        private List<FeaturePlanConfiguration> _featurePlanConfigurations;
        private Mock<DbSet<FeaturePlanConfiguration>> _featurePlanConfigurationDbSetMock;

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

            _userRepositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<UserService>>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _dbContext,
                _loggerMock.Object);

            _splitFullNameMethod = typeof(UserService).GetMethod("SplitFullName",
                BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException("Method SplitFullName not found.");

            _featurePlanConfigurations = new List<FeaturePlanConfiguration>();
            _setFeatureValuesMethod = typeof(UserService).GetMethod("SetFeatureValues",
    BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException("Method SetFeatureValues not found.");

            // Setup DbSet mock
            _featurePlanConfigurationDbSetMock = CreateDbSetMock(_featurePlanConfigurations);


        }

        #endregion

        #region TEAR DOWN

        [TearDown]
        public void TearDown()
        {
            // Dispose of the DbContext to avoid memory leaks
            _dbContext.Dispose();
        }

        #endregion

        #region SIGNINWITHEMAILANDPASSWORDASYNC

        [Test]
        public void UserService_Constructor_ShouldInitializeSuccessfully()
        {
            // Act
            var userService = new UserService(
                _userRepositoryMock.Object,
                _dbContext,
                _loggerMock.Object
            );

            // Assert
            Assert.NotNull(userService);
        }

        [Test]
        public async Task SignInWithEmailAndPasswordAsync_ValidCredentials_ReturnsUserDetails()
        {
            // Arrange
            var email = "test@example.com";
            var password = "ValidPassword";
            var user = new ApplicationUser { Id = "1", FullName = "Test User", FirstName = "Test" };
            var userDetails = new DoctorProfile { Specialization = "Specialist", Title = 1, Image = null, Signature = null, ClinicId = 123 };
            var userTitle = new UserTitle { Abbreviations = "Dr." };

            // Mock repository methods
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(email)).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.GetUserDetailsbyIdAsync(user.Id)).ReturnsAsync(userDetails);
            _userRepositoryMock.Setup(repo => repo.GetUserTitlebyIdAsync(userDetails.Title)).ReturnsAsync(userTitle);
            _userRepositoryMock.Setup(repo => repo.VerifyPasswordAsync(user, password)).ReturnsAsync(true);

            // Act
            var result = await _userService.SignInWithEmailAndPasswordAsync(email, password);

            // Assert
            Assert.That(result.HttpStatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.FullName, Is.EqualTo(user.FullName));
            Assert.That(result.FirstName, Is.EqualTo(user.FirstName));
            Assert.That(result.UserId, Is.EqualTo(user.Id));
            Assert.That(result.Image, Is.Null);  // If image is null, it should be null in the result
            Assert.That(result.TitleAbbreviation, Is.EqualTo(userTitle.Abbreviations));
            Assert.That(result.Specialization, Is.EqualTo(userDetails.Specialization));
        }

        [Test]
        public async Task SignInWithEmailAndPasswordAsync_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var email = "test@example.com";
            var password = "InvalidPassword";
            var user = new ApplicationUser { Id = "1" };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(email)).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.VerifyPasswordAsync(user, password)).ReturnsAsync(false);

            // Act
            var result = await _userService.SignInWithEmailAndPasswordAsync(email, password);

            // Assert
            Assert.That(result.HttpStatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task SignInWithEmailAndPasswordAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var email = "test@example.com";
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(email)).ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _userService.SignInWithEmailAndPasswordAsync(email, "password");

            // Assert
            Assert.That(result.HttpStatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        #endregion

        #region SIGNUPWITHEMAILANDPASSWORDASYNC

        [Test]
        [TestCase("John", "John", "", "")]
        [TestCase("John Doe", "John", "", "Doe")]
        [TestCase("John Middle Doe", "John", "Middle", "Doe")]
        [TestCase("John Middle Other Doe", "John", "Middle Other", "Doe")]
        public void SplitFullName_ValidInput_ReturnsCorrectParts(
            string fullName, string expectedFirst, string expectedMiddle, string expectedLast)
        {
            // Act
            var result = _splitFullNameMethod.Invoke(_userService, new object[] { fullName })
                         as ValueTuple<string, string, string>?;
            // Assert
            Assert.NotNull(result, "SplitFullName method returned null");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result?.Item1, Is.EqualTo(expectedFirst), "First name doesn't match");
                Assert.That(result?.Item2, Is.EqualTo(expectedMiddle), "Middle name doesn't match");
                Assert.That(result?.Item3, Is.EqualTo(expectedLast), "Last name doesn't match");
            });
        }

        [Test]
        public void SplitFullName_SingleName_ReturnsOnlyFirstName()
        {
            // Arrange
            string fullName = "John";

            // Act
            var invokeResult = _splitFullNameMethod.Invoke(_userService, new object[] { fullName });

            Assert.That(invokeResult, Is.Not.Null, "SplitFullName method returned null");

            var result = (ValueTuple<string, string, string>)invokeResult!; // Safe unboxing after checking

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1, Is.EqualTo("John"), "First name should be 'John'");
                Assert.That(result.Item2, Is.Empty, "Middle name should be empty");
                Assert.That(result.Item3, Is.Empty, "Last name should be empty");
            });
        }


        [Test]
        public void SplitFullName_TwoNames_ReturnsFirstAndLastName()
        {
            // Arrange
            string fullName = "John Doe";

            // Act
            var invokeResult = _splitFullNameMethod.Invoke(_userService, new object[] { fullName });

            Assert.That(invokeResult, Is.Not.Null, "SplitFullName method returned null");

            var result = (ValueTuple<string, string, string>)invokeResult!; // Safe unboxing after null check

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1, Is.EqualTo("John"), "First name should be 'John'");
                Assert.That(result.Item2, Is.Empty, "Middle name should be empty");
                Assert.That(result.Item3, Is.EqualTo("Doe"), "Last name should be 'Doe'");
            });
        }


        [Test]
        public void SplitFullName_ThreeNames_ReturnsAllParts()
        {
            // Arrange
            string fullName = "John Middle Doe";

            // Act
            var invokeResult = _splitFullNameMethod.Invoke(_userService, new object[] { fullName });

            Assert.That(invokeResult, Is.Not.Null, "SplitFullName method returned null");

            var result = (ValueTuple<string, string, string>)invokeResult!; // Safe unboxing after null check

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1, Is.EqualTo("John"), "First name should be 'John'");
                Assert.That(result.Item2, Is.EqualTo("Middle"), "Middle name should be 'Middle'");
                Assert.That(result.Item3, Is.EqualTo("Doe"), "Last name should be 'Doe'");
            });
        }


        [Test]
        public void SplitFullName_MultipleMiddleNames_HandlesCorrectly()
        {
            // Arrange
            string fullName = "John Middle Other Doe";

            // Act
            var invokeResult = _splitFullNameMethod.Invoke(_userService, new object[] { fullName });

            Assert.That(invokeResult, Is.Not.Null, "SplitFullName method returned null");

            var result = (ValueTuple<string, string, string>)invokeResult!; // Safe unboxing after null check

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1, Is.EqualTo("John"), "First name should be 'John'");
                Assert.That(result.Item2, Is.EqualTo("Middle Other"), "Middle name should be 'Middle Other'");
                Assert.That(result.Item3, Is.EqualTo("Doe"), "Last name should be 'Doe'");
            });
        }


        [Test]
        [TestCase("")]
        [TestCase("   ")]
        public void SplitFullName_InvalidInput_ThrowsArgumentException(string fullName)
        {
            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() =>
                _splitFullNameMethod.Invoke(_userService, new object[] { fullName }));
            Assert.That(exception.InnerException, Is.TypeOf<ArgumentException>());
        }

        [Test]
        [TestCase("  John  Doe  ", "John", "", "Doe")]
        public void SplitFullName_ExtraWhitespace_HandlesCorrectly(
    string fullName, string expectedFirst, string expectedMiddle, string expectedLast)
        {
            // Act
            var invokeResult = _splitFullNameMethod.Invoke(_userService, new object[] { fullName });

            Assert.That(invokeResult, Is.Not.Null, "SplitFullName method returned null");

            var result = (ValueTuple<string, string, string>)invokeResult!; // Safe unboxing after null check

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1, Is.EqualTo(expectedFirst), "First name does not match");
                Assert.That(result.Item2, Is.EqualTo(expectedMiddle), "Middle name does not match");
                Assert.That(result.Item3, Is.EqualTo(expectedLast), "Last name does not match");
            });
        }

        [Test]
        [TestCase("Mr. James Middle Last", "Mr.", "James Middle", "Last")]
        public void SplitFullName_WithTitles_HandlesCorrectly(
            string fullName, string expectedFirst, string expectedMiddle, string expectedLast)
        {
            // Act
            var invokeResult = _splitFullNameMethod.Invoke(_userService, new object[] { fullName });

            Assert.That(invokeResult, Is.Not.Null, "SplitFullName method returned null");

            var result = (ValueTuple<string, string, string>)invokeResult!; // Safe unboxing after null check

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1, Is.EqualTo(expectedFirst), "First part (title) does not match");
                Assert.That(result.Item2, Is.EqualTo(expectedMiddle), "Middle part does not match");
                Assert.That(result.Item3, Is.EqualTo(expectedLast), "Last part does not match");
            });
        }


        [Test]
        [TestCase("Jean-Claude van Damme", "Jean-Claude", "van", "Damme")]
        [TestCase("Mary-Jane Wilson Smith", "Mary-Jane", "Wilson", "Smith")]
        public void SplitFullName_ExtraWhiteSpace_HandlesCorrectly( string fullName, string expectedFirst, string expectedMiddle, string expectedLast)
        {
            // Act
            var invokeResult = _splitFullNameMethod.Invoke(_userService, new object[] { fullName });

            Assert.That(invokeResult, Is.Not.Null, "SplitFullName method returned null");

            var result = (ValueTuple<string, string, string>)invokeResult!; // Safe unboxing after null check

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1, Is.EqualTo(expectedFirst), "First name does not match");
                Assert.That(result.Item2, Is.EqualTo(expectedMiddle), "Middle name does not match");
                Assert.That(result.Item3, Is.EqualTo(expectedLast), "Last name does not match");
            });
        }



        [Test]
        public async Task SignUpWithEmailAndPasswordAsync_ValidDetails_ReturnsOk()
        {
            // Arrange
            var signUpDetails = new Mock<ISignUpUserDetails>();

            // Mock the FullName property to return a valid full name
            signUpDetails.Setup(details => details.FullName).Returns("John Doe");

            var user = IdentityResult.Success; // Mocked IdentityResult for successful creation

            // Mock CreateUserAsync method to return IdentityResult.Success
            _userRepositoryMock.Setup(repo => repo.CreateUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                signUpDetails.Object)).ReturnsAsync(user);

            // Act
            var result = await _userService.SignUpWithEmailAndPasswordAsync(signUpDetails.Object);

            // Assert

            Assert.That(result, Is.EqualTo(HttpStatusCode.OK));
        }
       

        #endregion

        #region CHECKWHETHERCURRENTPASSWORDCORRECTORNOT

        [Test]
        public async Task CheckWhetherCurrentPasswordCorrectOrNot_ValidPassword_ReturnsTrue()
        {
            // Arrange
            var userId = "1";
            var password = "ValidPassword";
            var user = new ApplicationUser();

            _userRepositoryMock.Setup(repo => repo.GetUserByUserIdAsync(userId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.VerifyPasswordAsync(user, password)).ReturnsAsync(true);

            // Act
            var result = await _userService.CheckWhetherCurrentPasswordCorrectOrNot(userId, password);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CheckWhetherCurrentPasswordCorrectOrNot_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            var userId = "1";
            var password = "InvalidPassword";
            var user = new ApplicationUser();

            _userRepositoryMock.Setup(repo => repo.GetUserByUserIdAsync(userId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.VerifyPasswordAsync(user, password)).ReturnsAsync(false);

            // Act
            var result = await _userService.CheckWhetherCurrentPasswordCorrectOrNot(userId, password);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region CHECKWHETHEREMAILEXISTORNOT

        [Test]
        public async Task CheckWhetherEmailExistOrNot_EmailExists_ReturnsTrue()
        {
            // Arrange
            var emailId = "test@example.com";
            var user = new ApplicationUser { Email = emailId };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(emailId)).ReturnsAsync(user);

            // Act
            var result = await _userService.CheckWhetherEmailExistOrNot(emailId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
public async Task CheckWhetherEmailExistOrNot_EmailDoesNotExist_ReturnsFalse()
{
    // Arrange
    string emailId = "nonexistent@example.com"; // Ensure it's explicitly non-null

    // Simulate no user found in the repository
    _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync((ApplicationUser?)null);

    // Act
    var result = await _userService.CheckWhetherEmailExistOrNot(emailId);

    // Assert
    Assert.That(result, Is.False);
}



        #endregion

        #region UPDATECOUNTERASYNC

        [Test]
        public async Task UpdateCounterAsync_WithinLimit_UpdatesCounterSuccessfully()
        {
            // Arrange
            var userId = "user123";
            var user = new ApplicationUser
            {
                Id = userId,
                GenerateReportCount = 5,
                FirstName = "John",
                FullName = "John Doe",
                LastName = "Doe",
                MiddleName = "A"
            };

            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            using var dbContext = new MediAssistDbContext(options);
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            _userService = new UserService(
                _userRepositoryMock.Object,
                dbContext,
                _loggerMock.Object);

            // Act
            var (count, succeeded) = await _userService.UpdateCounterAsync(userId, user);

            // Assert
            Assert.That(count, Is.EqualTo(1), "Counter should increment by 1");
            Assert.That(succeeded, Is.True, "Operation should succeed");
        }

        [Test]
        public async Task UpdateCounterAsync_ExceptionThrown_ReturnsZeroAndFalse()
        {
            // Arrange
            var userId = "user123";
            var user = new ApplicationUser
            {
                Id = userId,
                GenerateReportCount = 5,
                FirstName = "John",
                FullName = "John Doe",
                LastName = "Doe",
                MiddleName = "A"
            };

            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new MediAssistDbContext(options);
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var mockDbContext = new Mock<MediAssistDbContext>(options);
            mockDbContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            _userService = new UserService(
                _userRepositoryMock.Object,
                mockDbContext.Object, // Mocked DbContext
                _loggerMock.Object);

            // Act
            var (count, succeeded) = await _userService.UpdateCounterAsync(userId, user);

            // Assert
            Assert.That(count, Is.EqualTo(0));
            Assert.That(succeeded, Is.False);
        }

        #endregion

        #region SETUSERCONFIGURATION


        [Test]
        public void SetFeatureValues_EmptyPlanConfiguration_ThrowsArgumentException()
        {
            // Arrange
            var userConfiguration = new UserConfiguration();
            int planId = 2;

            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() =>
                _setFeatureValuesMethod.Invoke(_userService, new object[] { userConfiguration, planId }));
            Assert.That(exception.InnerException, Is.TypeOf<ArgumentException>());
        }

        [Test]
        public void SetFeatureValues_ValidTranscriptionsFeature_SetsCorrectValue()
        {
            // Arrange
            var userConfiguration = new UserConfiguration();
            int planId = 3;

            _dbContext.FeaturePlanConfiguration.Add(new FeaturePlanConfiguration
            {
                PlanId = planId,
                FeatureId = (long)GlobalEnums.ConfigurableFeatures.Transcriptions,
                IsActive = true,
                Value = "100"
            });

            _dbContext.SaveChanges();

            // Act
            var result = (UserConfiguration?)_setFeatureValuesMethod.Invoke(_userService,
                new object[] { userConfiguration, planId });

            // Assert
            Assert.That(result, Is.Not.Null, "Result should not be null");
            Assert.That(result!.Transcriptions, Is.EqualTo(100));
        }

        [Test]
        public void SetFeatureValues_InactiveFeature_DoesNotSetValue()
        {
            // Arrange
            var userConfiguration = new UserConfiguration();
            int planId = 4;

            _dbContext.FeaturePlanConfiguration.Add(new FeaturePlanConfiguration
            {
                PlanId = planId,
                FeatureId = (long)GlobalEnums.ConfigurableFeatures.Transcriptions,
                IsActive = false,
                Value = "100"
            });

            _dbContext.SaveChanges();

            // Act
            var result = (UserConfiguration?)_setFeatureValuesMethod.Invoke(_userService,
                new object[] { userConfiguration, planId });

            // Assert
            Assert.That(result!.Transcriptions, Is.EqualTo(0));
        }

        [Test]
        public void SetFeatureValues_MultipleFeaturesActive_SetsAllValues()
        {
            // Arrange
            var userConfiguration = new UserConfiguration();
            int planId = 5;

            _dbContext.FeaturePlanConfiguration.Add(new FeaturePlanConfiguration
            {
                PlanId = planId,
                FeatureId = (long)GlobalEnums.ConfigurableFeatures.Transcriptions,
                IsActive = true,
                Value = "100"
            });

            _dbContext.FeaturePlanConfiguration.Add(new FeaturePlanConfiguration
            {
                PlanId = planId,
                FeatureId = (long)GlobalEnums.ConfigurableFeatures.AvailableHours,
                IsActive = true,
                Value = "24"
            });

            _dbContext.FeaturePlanConfiguration.Add(new FeaturePlanConfiguration
            {
                PlanId = planId,
                FeatureId = (long)GlobalEnums.ConfigurableFeatures.SessionDurationLimit,
                IsActive = true,
                Value = "60"
            });

            _dbContext.SaveChanges();

            // Act
            var result = (UserConfiguration?)_setFeatureValuesMethod.Invoke(_userService,
                new object[] { userConfiguration, planId });

            // Assert
            Assert.That(result, Is.Not.Null, "Result should not be null");
            Assert.Multiple(() =>
            {
                Assert.That(result!.Transcriptions, Is.EqualTo(100));
                Assert.That(result.AvailableHours, Is.EqualTo(24));
                Assert.That(result.SessionDurationLimit, Is.EqualTo(60));
            });
        }

        [Test]
        public void SetFeatureValues_BooleanFeatures_SetsCorrectly()
        {
            // Arrange
            var userConfiguration = new UserConfiguration();
            int planId = 1;

            _dbContext.FeaturePlanConfiguration.Add(new FeaturePlanConfiguration
            {
                PlanId = planId,
                FeatureId = (long)GlobalEnums.ConfigurableFeatures.RealtimeResults,
                IsActive = true
            });

            _dbContext.FeaturePlanConfiguration.Add(new FeaturePlanConfiguration
            {
                PlanId = planId,
                FeatureId = (long)GlobalEnums.ConfigurableFeatures.WatermarkRemoval,
                IsActive = false
            });

            _dbContext.SaveChanges();

            // Act
            var result = (UserConfiguration?)_setFeatureValuesMethod.Invoke(_userService,
                new object[] { userConfiguration, planId });

            // Assert
            Assert.That(result, Is.Not.Null, "Result should not be null");

            Assert.Multiple(() =>
            {
                Assert.That(result!.RealTimeResults, Is.True);
                Assert.That(result.WatermarkRemoval, Is.False);
            });
        }


        [Test]
        public void SetFeatureValues_InvalidFeatureId_SkipsFeature()
        {
            // Arrange
            var userConfiguration = new UserConfiguration();
            int planId = 1;

            _dbContext.FeaturePlanConfiguration.Add(new FeaturePlanConfiguration
            {
                PlanId = planId,
                FeatureId = 999, // Invalid feature ID
                IsActive = true,
                Value = "100"
            });
            _dbContext.SaveChanges();

            // Act
            var result = (UserConfiguration?)_setFeatureValuesMethod.Invoke(_userService,
                new object[] { userConfiguration, planId });

            // Assert
            Assert.That(result, Is.EqualTo(userConfiguration));
        }

        [Test]
        public void SetFeatureValues_InvalidValueFormat_ThrowsFormatException()
        {
            // Arrange
            var userConfiguration = new UserConfiguration();
            int planId = 1;

            _dbContext.FeaturePlanConfiguration.Add(new FeaturePlanConfiguration
            {
                PlanId = planId,
                FeatureId = (long)GlobalEnums.ConfigurableFeatures.Transcriptions,
                IsActive = true,
                Value = "invalid"
            });
            _dbContext.SaveChanges();

            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() =>
                _setFeatureValuesMethod.Invoke(_userService, new object[] { userConfiguration, planId }));
            Assert.That(exception.InnerException, Is.TypeOf<FormatException>());
        }

        [Test]
        public void SetFeatureValues_AllFeatureTypes_ConfiguresCorrectly()
        {
            // Arrange
            var userConfiguration = new UserConfiguration();
            int planId = 1;

            _dbContext.FeaturePlanConfiguration.Add(new FeaturePlanConfiguration
            {
                PlanId = planId,
                FeatureId = (long)GlobalEnums.ConfigurableFeatures.Transcriptions,
                IsActive = true,
                Value = "100"
            });

            _dbContext.FeaturePlanConfiguration.Add(new FeaturePlanConfiguration
            {
                PlanId = planId,
                FeatureId = (long)GlobalEnums.ConfigurableFeatures.RealtimeResults,
                IsActive = true
            });

            _dbContext.FeaturePlanConfiguration.Add(new FeaturePlanConfiguration
            {
                PlanId = planId,
                FeatureId = (long)GlobalEnums.ConfigurableFeatures.TailoredcapabilitiesAndAdvancedsupport,
                IsActive = true
            });

            _dbContext.SaveChanges();

            // Act
            var result = (UserConfiguration?)_setFeatureValuesMethod.Invoke(_userService,
                new object[] { userConfiguration, planId });

            // Assert
            Assert.That(result, Is.Not.Null, "Result should not be null");

            Assert.Multiple(() =>
            {
                Assert.That(result!.Transcriptions, Is.EqualTo(100));
                Assert.That(result.RealTimeResults, Is.True);
                Assert.That(result.TailoredCapabilitiesAndAdvancedSupport, Is.True);
            });
        }


        [Test]
        public async Task SetUserConfiguration_UserConfigDoesNotExist_AddsConfiguration()
        {
            // Arrange
            var userId = "Testuser123";
            var planId = 7;

            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new MediAssistDbContext(options);

            // Seed the necessary feature plan configuration for the test
            dbContext.FeaturePlanConfiguration.Add(new FeaturePlanConfiguration
            {
                PlanId = planId // Only include properties that exist in the base class
            });
            await dbContext.SaveChangesAsync();

            _userService = new UserService(
                _userRepositoryMock.Object,
                dbContext,
                _loggerMock.Object);

            // Verify that the user configuration does not exist initially
            var existingConfig = await dbContext.UserConfiguration
                .FirstOrDefaultAsync(c => c.UserId == userId);
            Assert.IsNull(existingConfig);

            // Act
            var result = await _userService.SetUserConfiguration(userId, planId);

            // Assert
            Assert.IsTrue(result);

            // Verify that a new user configuration was added
            var newConfig = await dbContext.UserConfiguration
                .FirstOrDefaultAsync(c => c.UserId == userId);
            Assert.IsNotNull(newConfig);
            Assert.That(newConfig.UserId, Is.EqualTo(userId));

            // Since we can't check PlanId, verify general existence
            Assert.IsTrue(await dbContext.UserConfiguration.AnyAsync(c => c.UserId == userId));
        }

        [Test]
        public void SetUserConfiguration_ExceptionThrown_ThrowsArgumentException()
        {
            // Arrange
            var userId = "Testuser";
            var planId = 10;

            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new MediAssistDbContext(options);

            // Seed the necessary feature plan configuration for the test
            dbContext.FeaturePlanConfiguration.Add(new FeaturePlanConfiguration
            {
                PlanId = planId // Only include properties that exist in the base class
            });
            dbContext.SaveChangesAsync();

            _userService = new UserService(
                _userRepositoryMock.Object,
                dbContext,
                _loggerMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _userService.SetUserConfiguration(userId, 0));
        }
        [Test]
        public async Task SetUserConfiguration_NewUser_CreatesConfiguration()
        {
            // Arrange
            string userId = "test-user-1";
            int planId = 112;
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
               .UseInMemoryDatabase(databaseName: "TestDb")
               .Options;

            using var dbContext = new MediAssistDbContext(options);

            // Seed the necessary feature plan configuration for the test
            dbContext.FeaturePlanConfiguration.AddRange(
                new FeaturePlanConfiguration
                {
                    PlanId = planId,
                    FeatureId = (long)GlobalEnums.ConfigurableFeatures.Transcriptions,
                    IsActive = true,
                    Value = "100"
                },
                new FeaturePlanConfiguration
                {
                    PlanId = planId,
                    FeatureId = (long)GlobalEnums.ConfigurableFeatures.AvailableHours,
                    IsActive = true,
                    Value = "24"
                },
                new FeaturePlanConfiguration
                {
                    PlanId = planId,
                    FeatureId = (long)GlobalEnums.ConfigurableFeatures.SessionDurationLimit,
                    IsActive = true,
                    Value = "2"
                },
                new FeaturePlanConfiguration
                {
                    PlanId = planId,
                    FeatureId = (long)GlobalEnums.ConfigurableFeatures.RealtimeResults,
                    IsActive = true
                });

            await dbContext.SaveChangesAsync();

            _userService = new UserService(_userRepositoryMock.Object, dbContext, _loggerMock.Object);

            // Act
            await _userService.SetUserConfiguration(userId, planId);

            // Assert
            var userConfig = await dbContext.UserConfiguration
                .FirstOrDefaultAsync(x => x.UserId == userId);

            Assert.Multiple(() =>
            {
                Assert.That(userConfig, Is.Not.Null);
                Assert.That(userConfig!.UserId, Is.EqualTo(userId));
                Assert.That(userConfig.CreatedBy, Is.EqualTo(userId));
                Assert.That(userConfig.CreatedDate.Date, Is.EqualTo(DateTime.UtcNow.Date)); 
                Assert.That(userConfig.Transcriptions, Is.EqualTo(100));
                Assert.That(userConfig.AvailableHours, Is.EqualTo(24));
                Assert.That(userConfig.SessionDurationLimit, Is.EqualTo(2));
                Assert.That(userConfig.RealTimeResults, Is.True);
            });
        }


        [Test]
        public async Task SetUserConfiguration_DuplicateUser_DoesNotCreateNewConfiguration()
        {
            // Arrange
            string userId = "test-user-2";
            int planId = 14;

            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new MediAssistDbContext(options);

            // Seed the necessary feature plan configuration for the test
            dbContext.FeaturePlanConfiguration.Add(new FeaturePlanConfiguration
            {
                PlanId = planId // Only include properties that exist in the base class
            });
            await  dbContext.SaveChangesAsync();

            // Act
            await _userService.SetUserConfiguration(userId, planId);

            // Assert
            var userConfigs = await _dbContext.UserConfiguration
                .Where(x => x.UserId == userId)
                .ToListAsync();

            Assert.That(userConfigs.Count, Is.EqualTo(1));
        }

        [Test]
        public void SetUserConfiguration_InvalidPlanId_ThrowsArgumentException()
        {
            // Arrange
            string userId = "test-user-3";
            int invalidPlanId = 999;

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _userService.SetUserConfiguration(userId, invalidPlanId)
            );

            Assert.That(ex.Message, Does.Contain("featurePlanConfiguration is empty"));
        }

        [Test]
        public async Task SetUserConfiguration_ValidInput_SetsAllFeatureValues()
        {
            // Arrange
            string userId = "test-user-4";
            int planId = 15;

            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using var dbContext = new MediAssistDbContext(options);

            // Seed the necessary feature plan configuration for the test
            dbContext.FeaturePlanConfiguration.AddRange(
                new FeaturePlanConfiguration
                {
                    PlanId = planId,
                    FeatureId = (long)GlobalEnums.ConfigurableFeatures.Transcriptions,
                    IsActive = true,
                    Value = "100"
                },
                new FeaturePlanConfiguration
                {
                    PlanId = planId,
                    FeatureId = (long)GlobalEnums.ConfigurableFeatures.AvailableHours,
                    IsActive = true,
                    Value = "24"
                },
                new FeaturePlanConfiguration
                {
                    PlanId = planId,
                    FeatureId = (long)GlobalEnums.ConfigurableFeatures.SessionDurationLimit,
                    IsActive = true,
                    Value = "2"
                },
                new FeaturePlanConfiguration
                {
                    PlanId = planId,
                    FeatureId = (long)GlobalEnums.ConfigurableFeatures.RealtimeResults,
                    IsActive = true
                });

            await dbContext.SaveChangesAsync(); 

            _userService = new UserService(_userRepositoryMock.Object, dbContext, _loggerMock.Object);

            // Act
            await _userService.SetUserConfiguration(userId, planId);

            // Assert
            var userConfig = await dbContext.UserConfiguration
                .FirstOrDefaultAsync(x => x.UserId == userId);

            Assert.Multiple(() =>
            {
                Assert.That(userConfig, Is.Not.Null);
                Assert.That(userConfig!.Transcriptions, Is.EqualTo(100));
                Assert.That(userConfig.AvailableHours, Is.EqualTo(24));
                Assert.That(userConfig.SessionDurationLimit, Is.EqualTo(2));
                Assert.That(userConfig.RealTimeResults, Is.True);
                Assert.That(userConfig.CreatedDate.Date, Is.EqualTo(DateTime.UtcNow.Date)); 
                Assert.That(userConfig.CreatedBy, Is.EqualTo(userId));
            });
        }


        private static Mock<DbSet<T>> CreateDbSetMock<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return dbSetMock;
        }

        #endregion

        #region GETUSERCONFIGURATIONASYNC

        [Test]
        public async Task GetUserConfigurationAsync_ConfigurationExists_ReturnsConfig()
        {
            // Arrange
            var userId = "user123";
            var config = new UserConfiguration { UserId = userId, Transcriptions = 10, SessionDurationLimit = 60 };

            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new MediAssistDbContext(options);
            dbContext.UserConfiguration.Add(config);
            await dbContext.SaveChangesAsync();

            _userService = new UserService(
                _userRepositoryMock.Object,
                dbContext,
                _loggerMock.Object);

            // Act
            var result = await _userService.GetUserConfigurationAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Transcriptions, Is.EqualTo(10));
            Assert.That(result.SessionDurationLimit, Is.EqualTo(60));
        }
        
        [Test]
        public async Task GetUserConfigurationAsync_ConfigurationDoesNotExist_ReturnsDefaultConfig()
        {
            // Arrange
            var userId = "user123";

            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new MediAssistDbContext(options);

            _userService = new UserService(
                _userRepositoryMock.Object,
                dbContext,
                _loggerMock.Object);

            // Act
            var result = await _userService.GetUserConfigurationAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Transcriptions, Is.EqualTo(0));
        }

        #endregion
    }
}
