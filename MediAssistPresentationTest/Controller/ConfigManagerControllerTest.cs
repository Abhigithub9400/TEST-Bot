using MediAssist.Configurations;
using MediAssist.Infrastructure.Abstract.Configurations;
using MediAssist.UI.Controllers;
using MediAssist.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssistPresentationTest.Controller
{
    [TestFixture]
    public class ConfigManagerControllerTest
    {
        #region PRIVATE INSTANCE FIELD

        private ConfigManagerController _configManagerController;
        private Mock<ILogger<UsersController>> _mockLogger;
        private Mock<IAppSettings> _mockAppSettings;

        #endregion

        #region TEST SETUP

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<UsersController>>();

            _mockAppSettings = new Mock<IAppSettings>();
            _mockAppSettings.Setup(x => x.MediAssistDomainName).Returns("test.domain.com");
            _configManagerController = new ConfigManagerController(_mockAppSettings.Object,_mockLogger.Object);
        }

        #endregion

        #region PUBLIC METHODS

        [Test]
        public async Task GetMediAssistConfigurations_Success_ReturnsOkResult()
        {
            const string expectedDomain = "test.domain.com";
            _mockAppSettings.Setup(x => x.MediAssistDomainName)
                .Returns(expectedDomain);

            // Act
            var result = await _configManagerController.GetMediAssistConfigurations();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult!.Value, Is.InstanceOf<MediAssistConfigManager>());

            var config = okResult.Value as MediAssistConfigManager;
            Assert.That(config!.DomainName, Is.EqualTo(expectedDomain));
        }


        [Test]
        public async Task GetMediAssistConfigurations_WhenExceptionOccurs_ReturnsBadRequest()
        {
            // Arrange
            _mockAppSettings.Setup(x => x.MediAssistDomainName)
                .Throws(new Exception("Test exception"));

            // Act
            var result = await _configManagerController.GetMediAssistConfigurations();

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

            if (result is BadRequestObjectResult badRequestResult && badRequestResult.Value is not null)
            {
                // Verify that error was logged
                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                        ),
                        Times.Once
                    );
                                }
        }


        [Test]
        public void Controller_HasCorrectRouteAttributes()
        {
            // Assert controller attributes
            var controllerType = typeof(ConfigManagerController);
            var routeAttribute = controllerType.GetCustomAttributes(typeof(RouteAttribute), true)[0] as RouteAttribute;
            var apiControllerAttribute = controllerType.GetCustomAttributes(typeof(ApiControllerAttribute), true);

            Assert.That(routeAttribute, Is.Not.Null);
            Assert.That(routeAttribute.Template, Is.EqualTo("api/Config"));
            Assert.That(apiControllerAttribute.Length, Is.EqualTo(1));

            // Assert method attributes
            var methodInfo = controllerType.GetMethod("GetMediAssistConfigurations");
            var httpGetAttribute = methodInfo!.GetCustomAttributes(typeof(HttpGetAttribute), true)[0] as HttpGetAttribute;

            Assert.That(httpGetAttribute, Is.Not.Null);
            Assert.That(httpGetAttribute.Template, Is.EqualTo("get"));
        }
    }

    #endregion

}

