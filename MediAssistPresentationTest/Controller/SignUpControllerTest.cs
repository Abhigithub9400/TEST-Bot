using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.DbContext;
using MediAssist.UI.Controllers;
using MediAssist.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MediAssistPresentationTest.Controller
{
    [TestFixture]
    public class SignUpControllerTest
    {
        #region PRIVATE INSTANCE FIELDS

        private Mock<IUserService> _mockUserService;
        private Mock<ILogger<SignUpController>> _mockLogger;
        private SignUpController _signUpController;
        private MediAssistDbContext _mediAssistDbContext;
        private MethodInfo _validateDateOfBirthMethod;
        #endregion


        #region TEST SETUP
        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<SignUpController>>();
            _signUpController = new SignUpController(_mockUserService.Object, _mediAssistDbContext, _mockLogger.Object);
            _validateDateOfBirthMethod = typeof(SignUpController)
          .GetMethod("ValidateDateOfBirth", BindingFlags.NonPublic | BindingFlags.Instance);
        }
        #endregion

        #region PUBLIC METHODS

        #region CHECK EMAIL IF EXIST

        [Test]
        public async Task CheckEmailIfExist_ReturnsOk_WhenEmailExists()
        {
            // Arrange
            string emailId = "megha.sh@pitsolutions.com";
            _mockUserService.Setup(s => s.CheckWhetherEmailExistOrNot(emailId)).ReturnsAsync(true);

            // Act
            var result = await _signUpController.CheckEmailIfExist(emailId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>(), "Expected OkObjectResult but got a different type.");

            if (result is OkObjectResult okResult) // Safe casting
            {
                Assert.That(okResult.Value, Is.Not.Null, "Response value is null.");
            }
            else
            {
                Assert.Fail("Result is not an OkObjectResult, cannot proceed with further validation.");
            }
        }


        [Test]

        public async Task CheckEmailIfExist_WhenEmailDoesNotExist()
        {
            //Arrange
            string emailId = "megha123@gmail.com";
            _mockUserService.Setup(s => s.CheckWhetherEmailExistOrNot(emailId)).ReturnsAsync(false);

            // Act
            var result = await _signUpController.CheckEmailIfExist(emailId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>(), "Expected OkObjectResult but got a different type.");

            if (result is OkObjectResult okResult) // Safe casting
            {
                Assert.That(okResult.Value, Is.Not.Null, "Response value is null.");
            }
            else
            {
                Assert.Fail("Result is not an OkObjectResult, cannot proceed with further validation.");
            }
        }

        [Test]
        public async Task CheckEmailIfExist_ReturnsBadRequest_OnValidationFailure()
        {
            // Arrange
            string invalidEmail = "12233@yopmail.com";
            _mockUserService.Setup(s => s.CheckWhetherEmailExistOrNot(It.IsAny<string>()))
                .Throws(new Exception("Email validation failed."));

            // Act
            var result = await _signUpController.CheckEmailIfExist(invalidEmail);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected OkObjectResult but got a different type.");

            if (result is BadRequestObjectResult badRequestResult) // Safe casting
            {
                Assert.That(badRequestResult.Value, Is.Not.Null, "Response value is null.");
            }
            else
            {
                Assert.Fail("Result is not an OkObjectResult, cannot proceed with further validation.");
            }
        }

        [Test]
        public async Task CheckEmailIfExist_WithNullEmail_ReturnsBadRequest()
        {
            // Arrange
            string emailId = null!;

            // Act
            var result = await _signUpController.CheckEmailIfExist(emailId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");

            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");

            // Ensure Value is not null before accessing properties
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");
        }


        [Test]
        public async Task CheckEmailIfExist_WhenServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            string emailId = "megha.sh@pitsolutions.com";
            var expectedErrorMessage = "Database connection error";

            _mockUserService.Setup(x => x.CheckWhetherEmailExistOrNot(emailId))
                .ThrowsAsync(new Exception(expectedErrorMessage));

            // Act
            // Act
            var result = await _signUpController.CheckEmailIfExist(emailId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");

            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");

            // Verify that the error was logged
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(expectedErrorMessage)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true) // Nullable Exception
                ),
                Times.Once
            );


        }

        [Test]
        public async Task CheckEmailIfExist_WithInvalidEmailFormat_ReturnsBadRequest()
        {
            // Arrange
            string emailId = "invalid-email";

            // Act
            var result = await _signUpController.CheckEmailIfExist(emailId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");

            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");
        }


        #endregion

        #region SIGNUP
        [Test]
        public async Task SignUp_NameNullOrEmpty_ThrowsBadRequestException()
        {
            // Arrange
            _signUpController.ModelState.AddModelError("", "Name is required");
            var viewModel = GetNameNullOrEmptyViewModel();

            // Act
            var result = await _signUpController.SignUp(viewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");

            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");

        }


        [Test]
        public async Task SignUp_NameIsMatch_ThrowsBadRequestException()
        {
            //Arrage
            _signUpController.ModelState.AddModelError("Name@", "Name must contain only alphabetic characters.");
            var viewModel = GetMismatchNameViewModel();

            //Act
            var result = await _signUpController.SignUp(viewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>() ,"Expected BadRequestObjectResult.");
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");
        }

        [Test]
        public async Task SignUp_NameLessThanThree_ThrowsBadRequestException()
        {
            //Arrange
            _signUpController.ModelState.AddModelError("Na", "Name must be at least 3 characters long.");
            var viewModel = GetNameLessThanThreeViewModel();

            //Act
            var result = await _signUpController.SignUp(viewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");
     
        }


        [Test]
        public async Task SignUp_WithoutEmail_ThrowsBadRequestException()
        {
            //Arrange
            _signUpController.ModelState.AddModelError("", "Email is required");
            var viewModel = GetWithoutEmailViewModel();

            //Act
            var result = await _signUpController.SignUp(viewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");
        }



        [Test]
        public async Task SignUp_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _signUpController.ModelState.AddModelError("Email", "Email is required");
            var viewModel = GetInValidViewModel();

            // Act
            var result = await _signUpController.SignUp(viewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");
        }


        [Test]
        public async Task SignUp_InvalidEmail_ReturnsBadRequest()
        {
            // Arrange
            _signUpController.ModelState.AddModelError("Email", "Invalid email format");
            var viewModel = GetInValidEmailViewModel();

            // Act
            var result = await _signUpController.SignUp(viewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");
        }

        [Test]
        public async Task SignUp_PasswordMismatch_ThrowsBadRequestException()
        {
            //Arrange
            _signUpController.ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
            var viewModel = GetPasswordMismatchViewModel();

            // Act
            var result = await _signUpController.SignUp(viewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");
        }

        [Test]
        public async Task SignUp_InvalidDateOfBirth_ReturnsBadRequest()
        {
            //Arrange
            _signUpController.ModelState.AddModelError("DateOfBirth", "Invalid date of birth");
            var viewModel = GetInvalidDateOfBirthViewModel();

            // Act
            var result = await _signUpController.SignUp(viewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");
        }

        [Test]
        public async Task SignUp_InvalidCredentials_ThrowsBadRequestException()
        {
            //Arrage
            _signUpController.ModelState.AddModelError("MedicalCredentials", "Invalid credentials");
            var viewModel = GetInvalidCredentialsViewModel();

            //Act
            var result = await _signUpController.SignUp(viewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");

        }

        [Test]
        public async Task SignUp_InvalidTitle_ThrowsBadRequestException()
        {
            //Arrage
            _signUpController.ModelState.AddModelError("Title", "Invalid title");
            var viewModel = GetInvalidTitleViewModel();

            //Act
            var result = await _signUpController.SignUp(viewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");
        }

        [Test]
        public async Task SignUp_InvalidGender_ThrowsBadRequestException()
        {
            //Arrange
            _signUpController.ModelState.AddModelError("Gender", "Invalid gender");
            var viewModel = GetInvalidGenderViewModel();

            //Act
            var result = await _signUpController.SignUp(viewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");
        }

        [Test]
        public async Task SignUp_EmptySpecialization_ThrowsBadRequestException()
        {
            //Arrange
            _signUpController.ModelState.AddModelError("Gender", "Invalid gender");
            var viewModel = GetEmptySpecializationViewModel();

            //Act
            var result = await _signUpController.SignUp(viewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");
            
        }

        [Test]
        public async Task SignUp_TermsAndPrivacyNotAccepted_ThrowsBadRequestException()
        {
            //Arrange
            _signUpController.ModelState.AddModelError("Gender", "Invalid gender");
            var viewModel = GetTermsAndPrivacyNotAcceptedViewModel();
            //Act
            var result = await _signUpController.SignUp(viewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");

        }

        [Test]
        public async Task SignUp_LicenseAgreementNotAccepted_ThrowsBadRequestException()
        {
            //Arrange
            _signUpController.ModelState.AddModelError("Gender", "Invalid gender");
            var viewModel = GetLicenseAgreementNotAcceptedViewModel();
            //Act
            var result = await _signUpController.SignUp(viewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestObjectResult.");
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "BadRequestObjectResult is null.");
            Assert.That(badRequestResult!.Value, Is.Not.Null, "BadRequestResult value is null.");
          
        }

        [Test]
        public async Task SignUp_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var viewModel = GetValidViewModel();
            _mockUserService
                .Setup(x => x.SignUpWithEmailAndPasswordAsync(It.IsAny<SignUpUserDetails>()))
                .ReturnsAsync(HttpStatusCode.OK);

            // Act
            var result = await _signUpController.SignUp(viewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>(), "Expected ObjectResult.");
            var ObjectResult = result as ObjectResult;
            Assert.That(ObjectResult, Is.Not.Null, "ObjectResult is null.");
            Assert.That(ObjectResult!.Value, Is.Not.Null, "ObjectResult value is null.");
        }


        [TestCase("1990-01-01", true, "")] // Valid date
        [TestCase("2010-01-01", false, "The age must be above 18 years.")] // Under 18
        [TestCase("2004-02-29", true, "")] // Valid leap year
        public void ValidateDateOfBirth_ReturnsExpectedResult(string dobString, bool expectedIsValid, string expectedError)
        {
            // Arrange
            DateTime dob;
            object param;

            // Use a format provider to ensure consistent parsing
            bool isValidDate = DateTime.TryParseExact(
                dobString,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dob
            );

            param = isValidDate ? (object)dob : dobString; // Handle invalid date scenario

            // Act
            var result = _validateDateOfBirthMethod.Invoke(_signUpController, new object[] { param });

            // Ensure result is not null before unboxing
            Assert.That(result, Is.Not.Null, "Validation result is null.");

            // Convert result to ValueTuple safely
            var (isValid, errorMessage) = ((bool IsValid, string ErrorMessage))result!;

            // Assert
            Assert.That(isValid, Is.EqualTo(expectedIsValid), "Validation result does not match expected value.");
            Assert.That(errorMessage, Is.EqualTo(expectedError), "Error message does not match expected value.");
        }


        #endregion

        #endregion


        #region PRIVATE METHODS

        private SignUpViewModel GetValidViewModel()
        {
            return new SignUpViewModel
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = 1,
                Gender = 1,
                DateOfBirth = DateTime.Now.AddYears(-30),
                MedicalCredentials = 1,
                Specialization = "Cardiology",
                TermsAndPrivacy = true,
                LicenseAgreement = true
            };
        }

        private SignUpViewModel GetInValidViewModel()
        {
            return new SignUpViewModel
            {
                Name = "",
                Email = "",
                Password = "",
                ConfirmPassword = "!",
                Title = 10,
                Gender = 6,
                DateOfBirth = DateTime.Now.AddYears(1),
                MedicalCredentials = 10,
                Specialization = "",
                TermsAndPrivacy = false,
                LicenseAgreement = false
            };
        }

        private SignUpViewModel GetInValidEmailViewModel()
        {
            return new SignUpViewModel
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = 1,
                Gender = 1,
                DateOfBirth = DateTime.Now.AddYears(-30),
                MedicalCredentials = 1,
                Specialization = "Cardiology",
                TermsAndPrivacy = true,
                LicenseAgreement = true
            };
        }

        private SignUpViewModel GetPasswordMismatchViewModel()
        {
            return new SignUpViewModel
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                ConfirmPassword = "DifferentPassword123!",
                Title = 1,
                Gender = 1,
                DateOfBirth = DateTime.Now.AddYears(-30),
                MedicalCredentials = 1,
                Specialization = "Cardiology",
                TermsAndPrivacy = true,
                LicenseAgreement = true
            };
        }

        private SignUpViewModel GetInvalidDateOfBirthViewModel()
        {
            return new SignUpViewModel
            {
                Name = "John Doe",
                Email = "invalid-email",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = 1,
                Gender = 1,
                DateOfBirth = DateTime.Now.AddYears(1),
                MedicalCredentials = 1,
                Specialization = "Cardiology",
                TermsAndPrivacy = true,
                LicenseAgreement = true
            };
        }

        private SignUpViewModel GetInvalidCredentialsViewModel()
        {
            return new SignUpViewModel
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = 1,
                Gender = 1,
                DateOfBirth = DateTime.Now.AddYears(-30),
                MedicalCredentials = 10,
                Specialization = "Cardiology",
                TermsAndPrivacy = true,
                LicenseAgreement = true
            };
        }

        private SignUpViewModel GetInvalidTitleViewModel()
        {
            return new SignUpViewModel
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = 7,
                Gender = 1,
                DateOfBirth = DateTime.Now.AddYears(-30),
                MedicalCredentials = 1,
                Specialization = "Cardiology",
                TermsAndPrivacy = true,
                LicenseAgreement = true
            };
        }

        private SignUpViewModel GetInvalidGenderViewModel()
        {
            return new SignUpViewModel
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = 1,
                Gender = 3,
                DateOfBirth = DateTime.Now.AddYears(-30),
                MedicalCredentials = 1,
                Specialization = "Cardiology",
                TermsAndPrivacy = true,
                LicenseAgreement = true
            };
        }

        private SignUpViewModel GetEmptySpecializationViewModel()
        {
            return new SignUpViewModel
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = 1,
                Gender = 1,
                DateOfBirth = DateTime.Now.AddYears(-30),
                MedicalCredentials = 1,
                Specialization = "",
                TermsAndPrivacy = true,
                LicenseAgreement = true
            };
        }

        private SignUpViewModel GetTermsAndPrivacyNotAcceptedViewModel()
        {
            return new SignUpViewModel
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = 1,
                Gender = 1,
                DateOfBirth = DateTime.Now.AddYears(-30),
                MedicalCredentials = 1,
                Specialization = "Cardiology",
                TermsAndPrivacy = false,
                LicenseAgreement = true
            };
        }

        private SignUpViewModel GetLicenseAgreementNotAcceptedViewModel()
        {
            return new SignUpViewModel
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = 1,
                Gender = 1,
                DateOfBirth = DateTime.Now.AddYears(-30),
                MedicalCredentials = 1,
                Specialization = "Cardiology",
                TermsAndPrivacy = true,
                LicenseAgreement = false
            };
        }

        private SignUpViewModel GetNameNullOrEmptyViewModel()
        {
            return new SignUpViewModel 
            { 
                Name ="",
                Email = "john.doe@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = 1,
                Gender = 1,
                DateOfBirth = DateTime.Now.AddYears(-30),
                MedicalCredentials = 1,
                Specialization = "Cardiology",
                TermsAndPrivacy = true,
                LicenseAgreement = true

            };
        }

        private SignUpViewModel GetMismatchNameViewModel()
        {
            return new SignUpViewModel
            {
                Name = "Name@",
                Email = "john.doe@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = 1,
                Gender = 1,
                DateOfBirth = DateTime.Now.AddYears(-30),
                MedicalCredentials = 1,
                Specialization = "Cardiology",
                TermsAndPrivacy = true,
                LicenseAgreement = true
            };
        }

        private SignUpViewModel GetNameLessThanThreeViewModel()
        {
            return new SignUpViewModel
            {
                Name = "Na",
                Email = "john.doe@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = 1,
                Gender = 1,
                DateOfBirth = DateTime.Now.AddYears(-30),
                MedicalCredentials = 1,
                Specialization = "Cardiology",
                TermsAndPrivacy = true,
                LicenseAgreement = true
            };
        }

        private SignUpViewModel GetWithoutEmailViewModel()
        {
            return new SignUpViewModel
            {
                Name = "Na",
                Email = "",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = 1,
                Gender = 1,
                DateOfBirth = DateTime.Now.AddYears(-30),
                MedicalCredentials = 1,
                Specialization = "Cardiology",
                TermsAndPrivacy = true,
                LicenseAgreement = true

            };
        }

        #endregion

    }
}
