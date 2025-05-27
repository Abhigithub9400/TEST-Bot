using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Services;
using MediAssist.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MediAssistApplicationTest.Services
{
    [TestFixture]
    public class UserSessionServiceTest
    {
        #region PRIVATE INSTANCE FIELD

        private MediAssistDbContext _dbContext;
        private Mock<ILogger<UserSessionService>> _mockLogger;
        private UserSessionService _userSessionService;
        private Mock<DbSet<UserSession>> _mockUserSessionSet;
        private MethodInfo _calculateRemainingTimeMethod;
        private MethodInfo _createSessionResponseMethod;
        private MethodInfo _updateUserConfigurationsMethod;


        #endregion

        #region TEST SETUP

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb") // Use In-Memory database
            .Options;

            _dbContext = new MediAssistDbContext(options);
            _mockLogger = new Mock<ILogger<UserSessionService>>();

            // Mock DbSet<UserSession>
            //_mockUserSessionSet = new Mock<DbSet<UserSession>>();
            //_dbContext.UserSession = _mockUserSessionSet.Object;
            _calculateRemainingTimeMethod = typeof(UserSessionService).GetMethod("CalculateRemainingTime",
               BindingFlags.NonPublic | BindingFlags.Instance);

            _userSessionService = new UserSessionService(_dbContext, _mockLogger.Object);

            _createSessionResponseMethod = typeof(UserSessionService).GetMethod("createSessionResponse",
               BindingFlags.NonPublic | BindingFlags.Instance);
            _updateUserConfigurationsMethod = typeof(UserSessionService).GetMethod("UpdateUserConfigureations",
                BindingFlags.NonPublic | BindingFlags.Instance);


        }

        #endregion

        #region TEAR DOWN

        [TearDown]
        public void TearDown()
        {
            // Dispose of the DbContext to avoid memory leaks
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        #endregion

        #region STARTORRESUMESESSION

        [Test]
        public async Task StartOrResumeSession_NewSession_ShouldCreateAndMapSessionDetailsCorrectly()
        {
            // Arrange
            var userId = "testUser";
            var sessionDurationLimit = 60L; // 60 minutes
            int totalToken = 10;
            decimal totalCost = 10;

            var startSessionDetails = new Mock<IStartSessionDetails>();

            startSessionDetails.Setup(details => details.UserId).Returns(userId);
            startSessionDetails.Setup(details => details.SessionId).Returns(0);
            startSessionDetails.Setup(details => details.TotalToken).Returns(totalToken);
            startSessionDetails.Setup(details => details.TotalCost).Returns(totalCost);
            startSessionDetails.Setup(details => details.IsPotentialDiagnosisOn).Returns(true);

            // Add test data directly to the in-memory database
            _dbContext.UserConfiguration.Add(new UserConfiguration
            {
                UserId = userId,
                SessionDurationLimit = sessionDurationLimit,
                AvailableHours = 120 // Adding some available hours
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userSessionService.StartOrResumeSession(startSessionDetails.Object);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.SessionId, Is.EqualTo(1));
            Assert.That(result.SessionVersion, Is.EqualTo(1));
            Assert.That(result.SessionExpired, Is.False);
            Assert.That(result.RemainingTime, Is.EqualTo(TimeSpan.FromMinutes(sessionDurationLimit)));

            // Verify the session was actually saved to the database
            var savedSession = await _dbContext.UserSession.FirstOrDefaultAsync(s => s.UserId == userId);
            Assert.That(savedSession, Is.Not.Null);
            Assert.That(savedSession.SessionId, Is.EqualTo(1));
        }

        [Test]
        public async Task StartOrResumeSession_ExistingSession_ShouldMapSessionDetailsCorrectly()
        {
            // Arrange
            var userId = "testUser";
            var sessionId = 1;
            var sessionDurationLimit = 60L;
            var now = DateTime.Now;
            int totalToken = 10;
            decimal totalCost = 10;
            var isPotentialDiagnosisOn = true;

            var startSessionDetails = new Mock<IStartSessionDetails>();

            startSessionDetails.Setup(details => details.UserId).Returns(userId);
            startSessionDetails.Setup(details => details.SessionId).Returns(sessionId);
            startSessionDetails.Setup(details => details.TotalToken).Returns(totalToken);
            startSessionDetails.Setup(details => details.TotalCost).Returns(totalCost);
            startSessionDetails.Setup(details => details.IsPotentialDiagnosisOn).Returns(isPotentialDiagnosisOn);

            // Add required configuration
            _dbContext.UserConfiguration.Add(new UserConfiguration
            {
                UserId = userId,
                SessionDurationLimit = sessionDurationLimit,
                AvailableHours = 120
            });

            // Add existing session
            _dbContext.UserSession.Add(new UserSession
            {
                UserId = userId,
                SessionId = sessionId,
                SessionVersion = 1,
                SessionExpired = false,
                ReportGenerated = false,
                SessionStartTime = now.AddMinutes(-30),
                SessionRemainingTime = TimeSpan.FromMinutes(30),
                CreatedDate = now.AddMinutes(-30),
                ModifiedDate = now.AddMinutes(-30),
                FeaturePlanId = 1,
                CreatedBy = userId,
                ModifiedBy = userId,
                IsPotentialDiagnosisOn = isPotentialDiagnosisOn
            });

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userSessionService.StartOrResumeSession(startSessionDetails.Object);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.SessionId, Is.EqualTo(sessionId));
            Assert.That(result.SessionVersion, Is.EqualTo(2)); // Version should be incremented
            Assert.That(result.SessionExpired, Is.False);
        }

        [Test]
        public async Task StopUserSession_ShouldMapSessionDetailsCorrectly()
        {
            // Arrange
            var userId = "testUser";
            var sessionId = 1;
            var now = DateTime.Now;
            int totalToken = 10;
            decimal totalCost = 10;

            // Add required configuration
            _dbContext.UserConfiguration.Add(new UserConfiguration
            {
                UserId = userId,
                SessionDurationLimit = 60,
                AvailableHours = 120
            });

            // Add existing session
            _dbContext.UserSession.Add(new UserSession
            {
                UserId = userId,
                SessionId = sessionId,
                SessionVersion = 1,
                SessionExpired = false,
                ReportGenerated = false,
                SessionStartTime = now.AddMinutes(-30),
                SessionRemainingTime = TimeSpan.FromMinutes(30),
                CreatedDate = now.AddMinutes(-30),
                ModifiedDate = now.AddMinutes(-30),
                FeaturePlanId = 1,
                CreatedBy = userId,
                ModifiedBy = userId
            });

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userSessionService.StopUserSession(sessionId, userId, totalToken, totalCost);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.SessionId, Is.EqualTo(sessionId));
            Assert.That(result.SessionExpired, Is.True);
        }

        [Test]
        public async Task UpdateUserSession_ShouldMapSessionDetailsCorrectly()
        {
            // Arrange
            var userId = "testUser";
            var sessionId = 1;
            var now = DateTime.Now;
            int totalToken = 10;
            decimal totalCost = 10;

            // Add required configuration
            _dbContext.UserConfiguration.Add(new UserConfiguration
            {
                UserId = userId,
                SessionDurationLimit = 60,
                AvailableHours = 120
            });

            // Add existing session
            _dbContext.UserSession.Add(new UserSession
            {
                UserId = userId,
                SessionId = sessionId,
                SessionVersion = 1,
                SessionExpired = false,
                ReportGenerated = false,
                SessionStartTime = now.AddMinutes(-30),
                SessionRemainingTime = TimeSpan.FromMinutes(30),
                CreatedDate = now.AddMinutes(-30),
                ModifiedDate = now.AddMinutes(-30),
                FeaturePlanId = 1,
                CreatedBy = userId,
                ModifiedBy = userId
            });

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userSessionService.UpdateUserSession(sessionId, userId, totalToken, totalCost);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.SessionId, Is.EqualTo(sessionId));
            Assert.That(result.SessionVersion, Is.EqualTo(2)); // Version should be incremented
        }


        [Test]
        public async Task StartOrResumeSession_UserIdNull_ReturnsEmptyResult()
        {
            // Arrange
            string userId = null;
            int sessionId = 1;
            int totalToken = 10;
            decimal totalCost = 10;
            var isPotentialDiagnosisOn = true;

            var startSessionDetails = new Mock<IStartSessionDetails>();

            startSessionDetails.Setup(details => details.UserId).Returns(userId);
            startSessionDetails.Setup(details => details.SessionId).Returns(sessionId);
            startSessionDetails.Setup(details => details.TotalToken).Returns(totalToken);
            startSessionDetails.Setup(details => details.TotalCost).Returns(totalCost);
            startSessionDetails.Setup(details => details.IsPotentialDiagnosisOn).Returns(isPotentialDiagnosisOn);

            // Create a mock UserSession with required properties
            var mockUserSession = new UserSession
            {
                UserId = userId,
                SessionId = sessionId,
                SessionStartTime = DateTime.Now,
                SessionRemainingTime = TimeSpan.FromMinutes(30),
                SessionVersion = 1,
                CreatedBy = "defaultUser",  // Required property
                ModifiedBy = "defaultUser", // Required property
                IsPotentialDiagnosisOn = isPotentialDiagnosisOn
            };

            // Mock DbSet<UserSession>
            _mockUserSessionSet = new Mock<DbSet<UserSession>>();
            _dbContext.UserSession = _mockUserSessionSet.Object;

            var mockUserSessions = new List<UserSession> { mockUserSession }.AsQueryable();
            _mockUserSessionSet.As<IQueryable<UserSession>>()
                               .Setup(m => m.Provider).Returns(mockUserSessions.Provider);
            _mockUserSessionSet.As<IQueryable<UserSession>>()
                               .Setup(m => m.Expression).Returns(mockUserSessions.Expression);
            _mockUserSessionSet.As<IQueryable<UserSession>>()
                               .Setup(m => m.ElementType).Returns(mockUserSessions.ElementType);
            _mockUserSessionSet.As<IQueryable<UserSession>>()
                               .Setup(m => m.GetEnumerator()).Returns(mockUserSessions.GetEnumerator());

            // Act
            var result = await _userSessionService.StartOrResumeSession(startSessionDetails.Object);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.SessionId); // Ensure the session is empty or default
        }

        [Test]
        public async Task StartOrResumeSession_SessionIdIsValid_ReturnsValidSession()
        {
            // Arrange
            string userId = "testUser";
            int sessionId = 1;
            int totalToken = 10;
            decimal totalCost = 10;
            var isPotentialDiagnosisOn = true;

            var startSessionDetails = new Mock<IStartSessionDetails>();

            startSessionDetails.Setup(details => details.UserId).Returns(userId);
            startSessionDetails.Setup(details => details.SessionId).Returns(sessionId);
            startSessionDetails.Setup(details => details.TotalToken).Returns(totalToken);
            startSessionDetails.Setup(details => details.TotalCost).Returns(totalCost);
            startSessionDetails.Setup(details => details.IsPotentialDiagnosisOn).Returns(isPotentialDiagnosisOn);

            var mockUserSession = new UserSession
            {
                UserId = userId,
                SessionId = sessionId,
                SessionStartTime = DateTime.Now,
                SessionRemainingTime = TimeSpan.FromMinutes(30),
                SessionVersion = 1,
                CreatedBy = "defaultUser",  // Required property
                ModifiedBy = "defaultUser" // Required property
            };

            // Use InMemoryDatabase and ensure the session is added directly
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new MediAssistDbContext(options);

            // Ensure to add mockUserSession before calling the service method
            dbContext.UserSession.Add(mockUserSession);
            await dbContext.SaveChangesAsync(); // Commit the changes to the in-memory database

            // Instantiate your service with the in-memory db context
            var userSessionService = new UserSessionService(dbContext, _mockLogger.Object);

            // Act
            var result = await userSessionService.StartOrResumeSession(startSessionDetails.Object);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(sessionId, result.SessionId);
            Assert.AreEqual(2, result.SessionVersion);
            Assert.IsFalse(result.SessionExpired);
        }

        [Test]
        public async Task UpdateUserSession_WithRemainingTime_ShouldCalculateCorrectRemainingTime()
        {
            // Arrange
            var userId = "testUser";
            var sessionId = 1;
            var sessionDurationLimit = 60L; // 60 minutes
            var now = DateTime.Now;
            var sessionStartTime = now.AddMinutes(-30); // Session started 30 minutes ago
            int totalToken = 10;
            decimal totalCost = 10;

            // Add required configuration
            _dbContext.UserConfiguration.Add(new UserConfiguration
            {
                UserId = userId,
                SessionDurationLimit = sessionDurationLimit,
                AvailableHours = 120
            });

            // Add existing session
            _dbContext.UserSession.Add(new UserSession
            {
                UserId = userId,
                SessionId = sessionId,
                SessionVersion = 1,
                SessionExpired = false,
                ReportGenerated = false,
                SessionStartTime = sessionStartTime,
                SessionRemainingTime = TimeSpan.FromMinutes(30),
                CreatedDate = sessionStartTime,
                ModifiedDate = sessionStartTime,
                FeaturePlanId = 1,
                CreatedBy = userId,
                ModifiedBy = userId
            });

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userSessionService.UpdateUserSession(sessionId, userId, totalToken, totalCost);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RemainingTime.TotalMinutes, Is.LessThanOrEqualTo(30)); // Should have less than 30 minutes remaining
            Assert.That(result.RemainingTime.TotalMinutes, Is.GreaterThan(0)); // Should still have some time remaining
        }

        [Test]
        public async Task UpdateUserSession_WithExpiredTime_ShouldReturnZeroRemainingTime()
        {
            // Arrange
            var userId = "testUser";
            var sessionId = 1;
            var sessionDurationLimit = 60L; // 60 minutes
            var now = DateTime.Now;
            var sessionStartTime = now.AddMinutes(-120); // Session started 2 hours ago (exceeding the duration limit)
            int totalToken = 10;
            decimal totalCost = 10;

            // Add required configuration
            _dbContext.UserConfiguration.Add(new UserConfiguration
            {
                UserId = userId,
                SessionDurationLimit = sessionDurationLimit,
                AvailableHours = 120
            });

            // Add existing session
            _dbContext.UserSession.Add(new UserSession
            {
                UserId = userId,
                SessionId = sessionId,
                SessionVersion = 1,
                SessionExpired = false,
                ReportGenerated = false,
                SessionStartTime = sessionStartTime,
                SessionRemainingTime = TimeSpan.Zero,
                CreatedDate = sessionStartTime,
                ModifiedDate = sessionStartTime,
                FeaturePlanId = 1,
                CreatedBy = userId,
                ModifiedBy = userId
            });

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userSessionService.UpdateUserSession(sessionId, userId, totalToken, totalCost);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RemainingTime, Is.EqualTo(TimeSpan.Zero));
            Assert.That(result.SessionExpired, Is.True);
        }

        [Test]
        public async Task UpdateUserSession_WithExactDurationLimit_ShouldCalculateCorrectRemainingTime()
        {
            // Arrange
            var userId = "testUser";
            var sessionId = 1;
            var sessionDurationLimit = 60L; // 60 minutes
            var now = DateTime.Now;
            var sessionStartTime = now.AddMinutes(-60); // Session started exactly at duration limit
            int totalToken = 10;
            decimal totalCost = 10;

            // Add required configuration
            _dbContext.UserConfiguration.Add(new UserConfiguration
            {
                UserId = userId,
                SessionDurationLimit = sessionDurationLimit,
                AvailableHours = 120
            });

            // Add existing session
            _dbContext.UserSession.Add(new UserSession
            {
                UserId = userId,
                SessionId = sessionId,
                SessionVersion = 1,
                SessionExpired = false,
                ReportGenerated = false,
                SessionStartTime = sessionStartTime,
                SessionRemainingTime = TimeSpan.Zero,
                CreatedDate = sessionStartTime,
                ModifiedDate = sessionStartTime,
                FeaturePlanId = 1,
                CreatedBy = userId,
                ModifiedBy = userId
            });

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userSessionService.UpdateUserSession(sessionId, userId, totalToken, totalCost);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RemainingTime, Is.EqualTo(TimeSpan.Zero));
            Assert.That(result.SessionExpired, Is.True);
        }

        [Test]
        public async Task UpdateUserSession_WithConfigurationUpdate_ShouldRecalculateRemainingTime()
        {
            // Arrange
            var userId = "testUser";
            var sessionId = 1;
            var initialSessionDurationLimit = 60L;
            var now = DateTime.Now;
            var sessionStartTime = now.AddMinutes(-30); // Session started 30 minutes ago
            int totalToken = 10;
            decimal totalCost = 10;

            // Add required configuration
            var userConfig = new UserConfiguration
            {
                UserId = userId,
                SessionDurationLimit = initialSessionDurationLimit,
                AvailableHours = 120
            };
            _dbContext.UserConfiguration.Add(userConfig);

            // Add existing session
            _dbContext.UserSession.Add(new UserSession
            {
                UserId = userId,
                SessionId = sessionId,
                SessionVersion = 1,
                SessionExpired = false,
                ReportGenerated = false,
                SessionStartTime = sessionStartTime,
                SessionRemainingTime = TimeSpan.FromMinutes(30),
                CreatedDate = sessionStartTime,
                ModifiedDate = sessionStartTime,
                FeaturePlanId = 1,
                CreatedBy = userId,
                ModifiedBy = userId
            });

            await _dbContext.SaveChangesAsync();

            // Update configuration to shorter duration
            userConfig.SessionDurationLimit = 40L; // Reduce to 40 minutes
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userSessionService.UpdateUserSession(sessionId, userId, totalToken, totalCost);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RemainingTime.TotalMinutes, Is.LessThanOrEqualTo(10)); // Should have less than 10 minutes remaining
            Assert.That(result.RemainingTime.TotalMinutes, Is.GreaterThanOrEqualTo(0));
        }

        #endregion

        #region STOPUSERSESSION
        [Test]
        public async Task StopUserSession_WithMultipleSessions_ShouldUpdateConfigurationCorrectly()
        {
            // Arrange
            var userId = "testUser";
            var sessionDurationLimit = 60L; // 60 minutes
            var availableHours = 180L; // 3 hours total available
            var now = DateTime.Now;
            int totalToken = 10;
            decimal totalCost = 10;

            // Add user configuration
            var userConfig = new UserConfiguration
            {
                UserId = userId,
                SessionDurationLimit = sessionDurationLimit,
                AvailableHours = availableHours,
                Transcriptions = 10
            };
            _dbContext.UserConfiguration.Add(userConfig);

            // Add multiple sessions with different remaining times
            var sessions = new[]
            {
        new UserSession
        {
            UserId = userId,
            SessionId = 1,
            SessionVersion = 1,
            SessionExpired = false,
            ReportGenerated = false,
            SessionStartTime = now.AddMinutes(-40),
            SessionRemainingTime = TimeSpan.FromMinutes(20), // Used 40 minutes
            FeaturePlanId = 1,
            CreatedBy = userId,
            ModifiedBy = userId,
            CreatedDate = now.AddMinutes(-40),
            ModifiedDate = now.AddMinutes(-40)
        },
        new UserSession
        {
            UserId = userId,
            SessionId = 2,
            SessionVersion = 1,
            SessionExpired = false,
            ReportGenerated = false,
            SessionStartTime = now.AddMinutes(-30),
            SessionRemainingTime = TimeSpan.FromMinutes(30), // Used 30 minutes
            FeaturePlanId = 1,
            CreatedBy = userId,
            ModifiedBy = userId,
            CreatedDate = now.AddMinutes(-30),
            ModifiedDate = now.AddMinutes(-30)
        }
    };

            _dbContext.UserSession.AddRange(sessions);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userSessionService.StopUserSession(2, userId, totalToken, totalCost);

            // Assert
            var updatedConfig = await _dbContext.UserConfiguration.FirstOrDefaultAsync(x => x.UserId == userId);
            Assert.That(updatedConfig, Is.Not.Null);

            // Total used time should be 70 minutes (40 + 30)
            Assert.That(updatedConfig.AvailableHours, Is.EqualTo(availableHours));
            Assert.That(updatedConfig.SessionDurationLimit, Is.EqualTo(sessionDurationLimit)); // Should not change as it's still greater than available time
        }

        [Test]
        public async Task StopUserSession_WhenAvailableTimeExceeded_ShouldSetConfigurationToZero()
        {
            // Arrange
            var userId = "testUser";
            var sessionDurationLimit = 60L;
            var availableHours = 120L; // 2 hours total available
            var now = DateTime.Now;
            int totalToken = 10;
            decimal totalCost = 10;

            // Add user configuration
            var userConfig = new UserConfiguration
            {
                UserId = userId,
                SessionDurationLimit = sessionDurationLimit,
                AvailableHours = availableHours,
                Transcriptions = 10
            };
            _dbContext.UserConfiguration.Add(userConfig);

            // Add sessions that will exceed available time
            var sessions = new[]
            {
        new UserSession
        {
            UserId = userId,
            SessionId = 1,
            SessionVersion = 1,
            SessionExpired = false,
            ReportGenerated = false,
            SessionStartTime = now.AddHours(-2),
            SessionRemainingTime = TimeSpan.Zero, // Used full 60 minutes
            FeaturePlanId = 1,
            CreatedBy = userId,
            ModifiedBy = userId,
            CreatedDate = now.AddHours(-2),
            ModifiedDate = now.AddHours(-2)
        },
        new UserSession
        {
            UserId = userId,
            SessionId = 2,
            SessionVersion = 1,
            SessionExpired = false,
            ReportGenerated = false,
            SessionStartTime = now.AddHours(-1),
            SessionRemainingTime = TimeSpan.Zero, // Used full 60 minutes
            FeaturePlanId = 1,
            CreatedBy = userId,
            ModifiedBy = userId,
            CreatedDate = now.AddHours(-1),
            ModifiedDate = now.AddHours(-1)
        }
    };

            _dbContext.UserSession.AddRange(sessions);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userSessionService.StopUserSession(2, userId, totalToken, totalCost);

            // Assert
            var updatedConfig = await _dbContext.UserConfiguration.FirstOrDefaultAsync(x => x.UserId == userId);
            Assert.That(updatedConfig, Is.Not.Null);
            Assert.That(updatedConfig.AvailableHours, Is.EqualTo(0));
            Assert.That(updatedConfig.SessionDurationLimit, Is.EqualTo(0));
        }

        [Test]
        public async Task StopUserSession_WhenAvailableTimeLessThanSessionLimit_ShouldUpdateBothLimits()
        {
            // Arrange
            var userId = "testUser";
            var sessionDurationLimit = 60L;
            var availableHours = 90L; // 1.5 hours total available
            var now = DateTime.Now;
            int totalToken = 10;
            decimal totalCost = 10;

            // Add user configuration
            var userConfig = new UserConfiguration
            {
                UserId = userId,
                SessionDurationLimit = sessionDurationLimit,
                AvailableHours = availableHours,
                Transcriptions = 10
            };
            _dbContext.UserConfiguration.Add(userConfig);

            // Add a session that leaves less than session duration limit available
            var session = new UserSession
            {
                UserId = userId,
                SessionId = 1,
                SessionVersion = 1,
                SessionExpired = false,
                ReportGenerated = false,
                SessionStartTime = now.AddMinutes(-70),
                SessionRemainingTime = TimeSpan.FromMinutes(20), // Used 50 minutes
                FeaturePlanId = 1,
                CreatedBy = userId,
                ModifiedBy = userId,
                CreatedDate = now.AddMinutes(-70),
                ModifiedDate = now.AddMinutes(-70)
            };

            _dbContext.UserSession.Add(session);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userSessionService.StopUserSession(1, userId, totalToken, totalCost);

            // Assert
            var updatedConfig = await _dbContext.UserConfiguration.FirstOrDefaultAsync(x => x.UserId == userId);
            Assert.That(updatedConfig, Is.Not.Null);

            // 90 minutes total - 50 minutes used = 40 minutes remaining
            Assert.That(updatedConfig.AvailableHours, Is.EqualTo(30));
            Assert.That(updatedConfig.SessionDurationLimit, Is.EqualTo(30)); // Should be updated to match available time
        }

        [Test]
        public async Task StopUserSession_WithNullUserConfiguration_ShouldThrowArgumentException()
        {
            // Arrange
            var userId = "testUser";
            var sessionId = 1;
            var now = DateTime.Now;
            int totalToken = 10;
            decimal totalCost = 10;

            // Add session without configuration
            var session = new UserSession
            {
                UserId = userId,
                SessionId = sessionId,
                SessionVersion = 1,
                SessionExpired = false,
                ReportGenerated = false,
                SessionStartTime = now.AddMinutes(-30),
                SessionRemainingTime = TimeSpan.FromMinutes(30),
                FeaturePlanId = 1,
                CreatedBy = userId,
                ModifiedBy = userId,
                CreatedDate = now.AddMinutes(-30),
                ModifiedDate = now.AddMinutes(-30)
            };

            _dbContext.UserSession.Add(session);
            await _dbContext.SaveChangesAsync();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _userSessionService.StopUserSession(sessionId, userId, totalToken, totalCost));
            Assert.That(ex.Message, Does.Contain("userConfiguration is null for userid"));
        }



        [Test]
        public async Task StopUserSession_ValidSessionId_StopsSession()
        {
            // Arrange
            string userId = "testUser";
            int sessionId = 1;
            int totalToken = 10;
            decimal totalCost = 10;

            var mockUserSession = new UserSession
            {
                UserId = userId,
                SessionId = sessionId,
                SessionStartTime = DateTime.Now,
                SessionRemainingTime = TimeSpan.FromMinutes(30),
                SessionVersion = 1,
                CreatedBy = userId,  // Set CreatedBy
                ModifiedBy = userId, // Set ModifiedBy
                SessionExpired = false
            };

            var mockUserConfiguration = new UserConfiguration
            {
                UserId = userId,
                CreatedBy = "SomeValue" // Add necessary properties here
            };

            // Use InMemoryDatabase and add both the session and configuration
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new MediAssistDbContext(options);

            // Add session and configuration, then save to the in-memory database
            dbContext.UserSession.Add(mockUserSession);
            dbContext.UserConfiguration.Add(mockUserConfiguration);
            await dbContext.SaveChangesAsync();

            // Instantiate the service with the in-memory db context
            var userSessionService = new UserSessionService(dbContext, _mockLogger.Object);

            // Act
            var result = await userSessionService.StopUserSession(sessionId, userId, totalToken, totalCost);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.SessionExpired);
        }

        [Test]
        public async Task StopUserSession_InvalidSessionId_ReturnsEmptyResult()
        {
            // Arrange
            string userId = "testUser";
            int invalidSessionId = 999;  // An ID that doesn't exist in the database
            int totalToken = 10;
            decimal totalCost = 10;

            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new MediAssistDbContext(options);

            // Add a valid session to the in-memory database
            var mockUserSession = new UserSession
            {
                UserId = userId,
                SessionId = 1,
                SessionStartTime = DateTime.Now,
                SessionRemainingTime = TimeSpan.FromMinutes(30),
                SessionVersion = 1,
                CreatedBy = userId,
                ModifiedBy = userId,
                SessionExpired = false
            };
            dbContext.UserSession.Add(mockUserSession);
            await dbContext.SaveChangesAsync();

            // Instantiate the service with the in-memory db context
            var userSessionService = new UserSessionService(dbContext, _mockLogger.Object);

            // Act
            var result = await userSessionService.StopUserSession(invalidSessionId, userId, totalToken, totalCost);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.SessionId);  // Ensure the session ID is zero for the empty result
            Assert.AreEqual(TimeSpan.Zero, result.RemainingTime);  // Default remaining time is zero
        }

        #endregion

        #region UPDATEUSERSESSION

        [Test]
        public void CalculateRemainingTime_ValidSession_ReturnsCorrectTime()
        {
            // Arrange
            var userSession = new UserSession
            {
                UserId = "testUser",
                SessionStartTime = DateTime.Now.AddMinutes(-30)
            };

            // Setup mock UserConfiguration
            var userConfig = new UserConfiguration
            {
                UserId = "testUser",
                SessionDurationLimit = 60
            };

            var userConfigs = new List<UserConfiguration> { userConfig }.AsQueryable();
            var mockUserConfigSet = new Mock<DbSet<UserConfiguration>>();

            mockUserConfigSet.As<IQueryable<UserConfiguration>>()
                .Setup(m => m.Provider)
                .Returns(userConfigs.Provider);
            mockUserConfigSet.As<IQueryable<UserConfiguration>>()
                .Setup(m => m.Expression)
                .Returns(userConfigs.Expression);
            mockUserConfigSet.As<IQueryable<UserConfiguration>>()
                .Setup(m => m.ElementType)
                .Returns(userConfigs.ElementType);
            mockUserConfigSet.As<IQueryable<UserConfiguration>>()
                .Setup(m => m.GetEnumerator())
                .Returns(userConfigs.GetEnumerator());
            _dbContext.UserConfiguration.Add(userConfig);
            _dbContext.SaveChanges();

            // Act
            var result = (TimeSpan)_calculateRemainingTimeMethod.Invoke(_userSessionService, new object[] { userSession });

            // Assert
            Assert.That(result.TotalMinutes, Is.InRange(29, 31)); // Allowing 1-minute margin for test execution
        }

        [Test]
        public void CalculateRemainingTime_ExpiredSession_ReturnsZero()
        {
            // Arrange
            var userSession = new UserSession
            {
                UserId = "testUser",
                SessionStartTime = DateTime.Now.AddHours(-2) // Session started 2 hours ago
            };

            var userConfig = new UserConfiguration
            {
                UserId = "testUser",
                SessionDurationLimit = 60 // 60 minutes limit
            };

            var userConfigs = new List<UserConfiguration> { userConfig }.AsQueryable();
            var mockUserConfigSet = new Mock<DbSet<UserConfiguration>>();

            mockUserConfigSet.As<IQueryable<UserConfiguration>>()
                .Setup(m => m.Provider)
                .Returns(userConfigs.Provider);
            mockUserConfigSet.As<IQueryable<UserConfiguration>>()
                .Setup(m => m.Expression)
                .Returns(userConfigs.Expression);
            mockUserConfigSet.As<IQueryable<UserConfiguration>>()
                .Setup(m => m.ElementType)
                .Returns(userConfigs.ElementType);
            mockUserConfigSet.As<IQueryable<UserConfiguration>>()
                .Setup(m => m.GetEnumerator())
                .Returns(userConfigs.GetEnumerator());

            _dbContext.UserConfiguration.Add(userConfig);
            _dbContext.SaveChanges();
            // Act
            var result = (TimeSpan)_calculateRemainingTimeMethod.Invoke(_userSessionService, new object[] { userSession });

            // Assert
            Assert.That(result, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void CalculateRemainingTime_NewSession_ReturnsFullDuration()
        {
            // Arrange
            var userSession = new UserSession
            {
                UserId = "testUser",
                SessionStartTime = DateTime.Now // Session just started
            };

            var userConfig = new UserConfiguration
            {
                UserId = "testUser",
                SessionDurationLimit = 60
            };

            var userConfigs = new List<UserConfiguration> { userConfig }.AsQueryable();
            var mockUserConfigSet = new Mock<DbSet<UserConfiguration>>();

            mockUserConfigSet.As<IQueryable<UserConfiguration>>()
                .Setup(m => m.Provider)
                .Returns(userConfigs.Provider);
            mockUserConfigSet.As<IQueryable<UserConfiguration>>()
                .Setup(m => m.Expression)
                .Returns(userConfigs.Expression);
            mockUserConfigSet.As<IQueryable<UserConfiguration>>()
                .Setup(m => m.ElementType)
                .Returns(userConfigs.ElementType);
            mockUserConfigSet.As<IQueryable<UserConfiguration>>()
                .Setup(m => m.GetEnumerator())
                .Returns(userConfigs.GetEnumerator());

            _dbContext.UserConfiguration.Add(userConfig);
            _dbContext.SaveChanges();
            // Act
            var result = (TimeSpan)_calculateRemainingTimeMethod.Invoke(_userSessionService, new object[] { userSession });

            // Assert
            Assert.That(result.TotalMinutes, Is.InRange(59, 60)); // Allowing 1-minute margin for test execution
        }

        [Test]
        public void CalculateRemainingTime_NoUserConfiguration_ReturnsZero()
        {
            // Arrange
            var userSession = new UserSession
            {
                UserId = "testUser",
                SessionStartTime = DateTime.Now
            };

            var emptyUserConfigs = new UserConfiguration() { UserId = userSession.UserId };


            _dbContext.UserConfiguration.Add(emptyUserConfigs);
            _dbContext.SaveChanges();

            // Act
            var result = (TimeSpan)_calculateRemainingTimeMethod.Invoke(_userSessionService, new object[] { userSession });

            // Assert
            Assert.That(result, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public async Task UpdateUserSession_ValidSessionId_UpdatesSession()
        {
            // Arrange
            string userId = "testUser";
            int sessionId = 1;
            int totalToken = 10;
            decimal totalCost = 10;

            // Create a DbContext with an in-memory database
            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            // Create a new instance of the MediAssistDbContext using the in-memory database
            using (var context = new MediAssistDbContext(options))
            {
                // Add a sample user session to the in-memory database
                var userSession = new UserSession
                {
                    UserId = userId,
                    SessionId = sessionId,
                    SessionStartTime = DateTime.Now,
                    SessionRemainingTime = TimeSpan.FromMinutes(30),
                    SessionVersion = 1,
                    CreatedBy = userId,
                    ModifiedBy = userId,
                    SessionExpired = false
                };
                context.UserSession.Add(userSession);
                await context.SaveChangesAsync();

                // Create the service
                var userSessionService = new UserSessionService(context, _mockLogger.Object);

                // Act
                var result = await userSessionService.UpdateUserSession(sessionId, userId, totalToken, totalCost);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(sessionId, result.SessionId);
            }
        }

        [Test]
        public async Task UpdateUserSession_InvalidSessionId_ReturnsEmptyResult()
        {
            // Arrange
            string userId = "testUser";
            int invalidSessionId = 999;  // An ID that doesn't exist in the database
            int totalToken = 10;
            decimal totalCost = 10;

            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new MediAssistDbContext(options);

            // Add a valid session to the in-memory database
            var mockUserSession = new UserSession
            {
                UserId = userId,
                SessionId = 1,
                SessionStartTime = DateTime.Now,
                SessionRemainingTime = TimeSpan.FromMinutes(30),
                SessionVersion = 1,
                CreatedBy = userId,
                ModifiedBy = userId,
                SessionExpired = false
            };
            dbContext.UserSession.Add(mockUserSession);
            await dbContext.SaveChangesAsync();

            // Instantiate the service with the in-memory db context
            var userSessionService = new UserSessionService(dbContext, _mockLogger.Object);

            // Act
            var result = await userSessionService.UpdateUserSession(invalidSessionId, userId, totalToken, totalCost);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.SessionId);  // Ensure the sessionId is zero for the empty result
            Assert.AreEqual(TimeSpan.Zero, result.RemainingTime);  // Default value for SessionRemainingTime is zero
        }

        #endregion

        #region REPORTGENERATED
        [Test]
        public async Task ReportGenerated_ValidSessionId_GeneratesReport()
        {
            // Arrange
            string userId = "testUser";
            int sessionId = 1;
            int totalToken = 10;
            decimal totalCost = 10;

            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            // Create a new instance of the MediAssistDbContext using the in-memory database
            using (var context = new MediAssistDbContext(options))
            {
                // Add a sample user session to the in-memory database
                var mockUserSession = new UserSession
                {
                    UserId = userId,
                    SessionId = sessionId,
                    ReportGenerated = false, // Initial value is false
                    CreatedBy = "user",
                    ModifiedBy = "user2",
                };

                context.UserSession.Add(mockUserSession);
                await context.SaveChangesAsync();

                // Create the service
                var userSessionService = new UserSessionService(context, _mockLogger.Object);

                // Ensure the session exists in the in-memory database
                var existingSession = context.UserSession.FirstOrDefault(x => x.SessionId == sessionId);
                Assert.IsNotNull(existingSession);

                // Act
                var result = await userSessionService.ReportGenerated(sessionId, userId, totalToken, totalCost);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(sessionId, result.SessionId);  // Ensure the sessionId is correct
            }
        }

        [Test]
        public async Task ReportGenerated_InvalidSessionId_ReturnsEmptyResult()
        {
            // Arrange
            string userId = "testUser";
            int invalidSessionId = 999;  // A sessionId that doesn't exist in the database
            int totalToken = 10;
            decimal totalCost = 10;

            var options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new MediAssistDbContext(options))
            {
                // Add a sample user session to the in-memory database
                var mockUserSession = new UserSession
                {
                    UserId = userId,
                    SessionId = 1,  // Session ID is different to simulate the invalid session
                    ReportGenerated = false, // Initial value is false
                    CreatedBy = "user",
                    ModifiedBy = "user2",
                };

                // Add the session to the in-memory database
                context.UserSession.Add(mockUserSession);
                await context.SaveChangesAsync();

                // Add a sample user configuration to the in-memory database
                var mockUserConfiguration = new UserConfiguration
                {
                    UserId = userId,
                    Transcriptions = 5
                };

                context.UserConfiguration.Add(mockUserConfiguration);
                await context.SaveChangesAsync();

                // Create the service
                var userSessionService = new UserSessionService(context, _mockLogger.Object);

                // Act
                var result = await userSessionService.ReportGenerated(invalidSessionId, userId, totalToken, totalCost);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.SessionId);  // Session ID should be zero for the empty result (no session found)
            }
        }

        #endregion
    }
}