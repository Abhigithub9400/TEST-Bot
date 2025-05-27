using MediAssist.DbContext;
using MediAssist.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssistDataAccessTest.Respositories
{
    [TestFixture]
    public class EmailRepositoryTest
    {
        #region PRIVATE INSTANCE FIELD

        private DbContextOptions<MediAssistDbContext> _options;

        #endregion

        #region TEST SETUP

        [SetUp]
        public void SetUp()
        {
            // Configure an in-memory database for testing
            _options = new DbContextOptionsBuilder<MediAssistDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
        }

        #endregion

        #region GET_EMAILTEMPLATE

        [Test]
        public void Constructor_WithValidDbContext_ShouldNotThrowException()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => new EmailRepository(new MediAssistDbContext(_options)));
        }



        [Test]
        public async Task Get_EmailTemplate_ValidIdentifier_ReturnsEmailTemplate()
        {
            // Arrange
            var emailIdentifier = "welcome";
            var expectedTemplate = new Master_EmailTemplate
            {
                Identifier = emailIdentifier,
                Subject = "Welcome to MediAssist",
                HTMLBody = "Hello, thank you for signing up.",
                CreatedBy = "testUser",
                PlainTextBody = "Welcome to MediAssist. Hello, thank you for signing up."
            };

            using (var context = new MediAssistDbContext(_options))
            {
                // Add test data to the in-memory database
                context.Master_EmailTemplates.Add(expectedTemplate);
                await context.SaveChangesAsync();
            }

            // Act
            Master_EmailTemplate? result; // Declare result as nullable
            using (var context = new MediAssistDbContext(_options))
            {
                var emailRepository = new EmailRepository(context);
                result = await emailRepository.Get_EmailTemplate(emailIdentifier);
            }

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null, "Expected email template not found in database.");
                Assert.That(result!.Identifier, Is.EqualTo(expectedTemplate.Identifier));
                Assert.That(result.Subject, Is.EqualTo(expectedTemplate.Subject));
                Assert.That(result.HTMLBody, Is.EqualTo(expectedTemplate.HTMLBody));
            });
        }



        [Test]
        public async Task Get_EmailTemplate_InvalidIdentifier_ReturnsNull()
        {
            // Arrange
            var invalidEmailIdentifier = "nonexistent";

            // Act
            Master_EmailTemplate? result; // Declare as nullable
            using (var context = new MediAssistDbContext(_options))
            {
                var emailRepository = new EmailRepository(context);
                result = await emailRepository.Get_EmailTemplate(invalidEmailIdentifier);
            }

            // Assert
            Assert.That(result, Is.Null);  // Ensure the template does not exist
        }


        #endregion
    }
}
