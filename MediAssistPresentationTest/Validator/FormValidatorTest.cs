using MediAssist.Application.Entities;
using MediAssist.Configurations.Exceptions;
using MediAssist.DbContext;
using MediAssist.UI.Validator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MediAssistPresentationTest.Validator
{
    [TestFixture]
    public class FormValidatorTest
    {

        [TestFixture]
        public class NameValidatorTests
        {
            [Test]
            public void ValidateName_ValidName_NoExceptionThrown()
            {
                Assert.DoesNotThrow(() => FormValidator.ValidateName("John Doe"));
            }

            [TestCase("")]
            public void ValidateName_NullOrEmpty_ThrowsBadRequestException(string name)
            {
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidateName(name));
                Assert.That(ex.Message, Is.EqualTo("Name is required."));
            }

            [TestCase("John@Doe")]
            [TestCase("John123")]
            public void ValidateName_InvalidCharacters_ThrowsBadRequestException(string name)
            {
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidateName(name));
                Assert.That(ex.Message, Is.EqualTo("Name must contain only alphabetic characters."));
            }

            [Test]
            public void ValidateName_TooShort_ThrowsBadRequestException()
            {
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidateName("Jo"));
                Assert.That(ex.Message, Is.EqualTo("Name must be at least 3 characters long."));
            }
        }

        [TestFixture]
        public class EmailValidatorTests
        {

            [TestCase("test@.com")]
            [TestCase("@example.com")]
            [TestCase("testexample.com")]
            [TestCase("test@example")]
            [TestCase("test.example@")]
            [TestCase("test@exam ple.com")]
            [TestCase(".test@example.com")]
            [TestCase("test@example..com")]
            public void ValidateEmail_InvalidFormat_ThrowsBadRequestException(string email)
            {
                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidateEmail(email));
                Assert.That(ex.Message, Is.EqualTo("Enter a valid email address."));
            }

            [Test]
            public void ValidateEmail_WithDotInDomain_NoExceptionThrown()
            {
                // Arrange
                string email = "test@sub.domain.com";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateEmail(email));
            }

            [Test]
            public void ValidateEmail_LongEmailAddress_NoExceptionThrown()
            {
                // Arrange
                string email = "very.long.email.address.123@really.long.domain.name.com";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateEmail(email));
            }


            [TestCase("john.doe@example.com")]
            [TestCase("jane_doe@company.co.uk")]
            public void ValidateEmail_ValidEmail_NoExceptionThrown(string email)
            {
                Assert.DoesNotThrow(() => FormValidator.ValidateEmailForLogin(email));
            }

            [TestCase("")]
            public void ValidateEmail_NullOrEmpty_ThrowsBadRequestException(string email)
            {
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidateEmail(email));
                Assert.That(ex.Message, Is.EqualTo("Email is required."));
            }

            [Test]
            public void ValidateEmail_DisposableEmailProvider_ThrowsBadRequestException()
            {
                var ex = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateEmail("test@yopmail.com"));
                Assert.That(ex.Message, Is.EqualTo("The email domain you have entered is not allowed.Please use a valid email provider."));
            }

            [Test]
            public void ValidateEmail_SpamPatternWithOnlyNumbers_ThrowsBadRequestException()
            {
                // Arrange
                string email = "123456@domain.com";

                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidateEmail(email));
                Assert.That(ex.Message, Is.EqualTo("Username must be 8+ characters and include at least one letter."));
            }

            [Test]
            public void ValidateEmail_SpamPatternWithShortLettersAndNumbers_ThrowsBadRequestException()
            {
                // Arrange
                string email = "ab123@domain.com";

                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidateEmail(email));
                Assert.That(ex.Message, Is.EqualTo("Username must be 8+ characters and include at least one letter."));
            }

            [Test]
            public void ValidateEmail_ValidUsernameWithLettersAndNumbers_NoExceptionThrown()
            {
                // Arrange
                string email = "john.doe123@domain.com";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateEmail(email));
            }

            [Test]
            public void ValidateEmail_UsernameIsUser_ThrowsBadRequestException()
            {
                // Arrange
                string email = "user@domain.com";

                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidateEmail(email));
                Assert.That(ex.Message, Is.EqualTo("Suspicious email addresses are not allowed."));
            }

            [Test]
            public void ValidateEmail_UsernameIsUSER_ThrowsBadRequestException()
            {
                // Arrange
                string email = "USER@domain.com";

                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidateEmail(email));
                Assert.That(ex.Message, Is.EqualTo("Suspicious email addresses are not allowed."));
            }

            [TestCase("ab@domain.com")]
            [TestCase("a1@domain.com")]
            [TestCase("b2@domain.com")]
            public void ValidateEmail_SpamPatternWithOneOrTwoCharacters_ThrowsBadRequestException(string email)
            {
                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidateEmail(email));
                Assert.That(ex.Message, Is.EqualTo("Username must be 8+ characters and include at least one letter."));
            }

            [TestCase("validuser@domain.com")]
            [TestCase("john.doe123@domain.com")]
            [TestCase("test.user890@domain.com")]
            public void ValidateEmail_ValidEmailPatterns_NoExceptionThrown(string email)
            {
                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateEmail(email));
            }

            [Test]
            public void ValidateEmail_UsernameMixedCaseUser_ThrowsBadRequestException()
            {
                // Arrange
                string email = "UsEr@domain.com";

                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidateEmail(email));
                Assert.That(ex.Message, Is.EqualTo("Suspicious email addresses are not allowed."));
            }

            [Test]
            public void ValidateEmail_ValidLongDomain_NoExceptionThrown()
            {
                // Arrange
                string email = "test@longerdomainname.com";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateEmail(email));
            }
        }


        [TestFixture]
        public class PasswordValidatorTests
        {

            [Test]
            public void ValidatePassword_UpperCasedRequestException()
            {
                // Arrange
                string password = "PASSWORD123!";

                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidatePassword(password));
                Assert.That(ex.Message, Is.EqualTo("Password must include at least one lowercase letter."));
            }

            [Test]
            public void ValidatePassword_NoDigit_ThrowsBadRequestException()
            {
                // Arrange
                string password = "Password!@#";

                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidatePassword(password));
                Assert.That(ex.Message, Is.EqualTo("Password must include at least one digit."));
            }

            [Test]
            public void ValidatePassword_NoSpecialCharacter_ThrowsBadRequestException()
            {
                // Arrange
                string password = "Password123";

                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidatePassword(password));
                Assert.That(ex.Message, Is.EqualTo("Password must include at least one special character."));
            }

            [Test]
            public void ValidatePassword_ValidPassword_NoExceptionThrown()
            {
                // Arrange
                string password = "Password123!";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidatePassword(password));
            }

            [TestCase("PASSWORD123!")]
            [TestCase("TESTING123@")]
            [TestCase("UPPER123#")]
            public void ValidatePassword_NoLowercaseVariations_ThrowsBadRequestException(string password)
            {
                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidatePassword(password));
                Assert.That(ex.Message, Is.EqualTo("Password must include at least one lowercase letter."));
            }

            [TestCase("Password!@#")]
            [TestCase("TestingTest$")]
            [TestCase("SecurePass%")]
            public void ValidatePassword_NoDigitVariations_ThrowsBadRequestException(string password)
            {
                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidatePassword(password));
                Assert.That(ex.Message, Is.EqualTo("Password must include at least one digit."));
            }

            [TestCase("Password123")]
            [TestCase("SecurePass123")]
            [TestCase("TestTest123")]
            public void ValidatePassword_NoSpecialCharacterVariations_ThrowsBadRequestException(string password)
            {
                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidatePassword(password));
                Assert.That(ex.Message, Is.EqualTo("Password must include at least one special character."));
            }

            [TestCase("Password123!")]
            [TestCase("Test1ng@Pass")]
            [TestCase("Secure123#Pass")]
            [TestCase("Complex1ty!")]
            public void ValidatePassword_ValidPasswords_NoExceptionThrown(string password)
            {
                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidatePassword(password));
            }

            [Test]
            public void ValidatePassword_AllSpecialCharacters_NoExceptionThrown()
            {
                // Testing each special character from the allowed set
                string[] specialChars = { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "+", "-", "=", "<", ">", "?" };

                foreach (var specialChar in specialChars)
                {
                    string password = $"Password123{specialChar}";
                    Assert.DoesNotThrow(() => FormValidator.ValidatePassword(password));
                }
            }

            [Test]
            public void ValidatePassword_MinimumRequirements_NoExceptionThrown()
            {
                // Arrange
                string password = "aA1!xxxx"; // Minimum length with all requirements

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidatePassword(password));
            }

            [TestCase("")]
            public void ValidatePassword_NullOrEmpty_ThrowsBadRequestException(string password)
            {
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidatePassword(password));
                Assert.That(ex.Message, Is.EqualTo("Password is required."));
            }

            [Test]
            public void ValidatePassword_TooShort_ThrowsBadRequestException()
            {
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidatePassword("Pass1!"));
                Assert.That(ex.Message, Is.EqualTo("Password must be at least 8 characters long."));
            }

            [Test]
            public void ValidatePassword_NoUppercase_ThrowsBadRequestException()
            {
                var ex = Assert.Throws<BadRequestException>(() => FormValidator.ValidatePassword("password1!"));
                Assert.That(ex.Message, Is.EqualTo("Password must include at least one uppercase letter."));
            }

            [Test]
            public void ValidateConfirmPassword_Matching_NoExceptionThrown()
            {
                Assert.DoesNotThrow(() => FormValidator.ValidateConfirmPassword("Password1!", "Password1!"));
            }

            [Test]
            public void ValidateConfirmPassword_NotMatching_ThrowsBadRequestException()
            {
                var ex = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateConfirmPassword("Password1!", "Password2!"));
                Assert.That(ex.Message, Is.EqualTo("Passwords do not match."));
            }
        }

        [TestFixture]
        public class TermsValidatorTests
        {
            [Test]
            public void ValidateAgreeTerms_WhenTermsAccepted_ShouldNotThrowException()
            {
                // Arrange
                bool agreeTerms = true;

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateAgreeTerms(agreeTerms));
            }

            [Test]
            public void ValidateAgreeTerms_WhenTermsNotAccepted_ShouldThrowBadRequestException()
            {
                // Arrange
                bool agreeTerms = false;

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateAgreeTerms(agreeTerms));

                Assert.That(exception.Message, Is.EqualTo("You must agree to the terms."));
            }

            [TestCase(true)]
            [TestCase(false)]
            public void ValidateAgreeTerms_BothCases_ShouldBehaveAsExpected(bool agreeTerms)
            {
                if (agreeTerms)
                {
                    Assert.DoesNotThrow(() => FormValidator.ValidateAgreeTerms(agreeTerms));
                }
                else
                {
                    var exception = Assert.Throws<BadRequestException>(() =>
                        FormValidator.ValidateAgreeTerms(agreeTerms));
                    Assert.That(exception.Message, Is.EqualTo("You must agree to the terms."));
                }
            }
        }

        [TestFixture]
        public class ConfirmPasswordValidationTests
        {
            [Test]
            public void ValidateConfirmPassword_MatchingPasswords_NoExceptionThrown()
            {
                // Arrange
                string password = "Password123!";
                string confirmPassword = "Password123!";

                // Act & Assert
                Assert.DoesNotThrow(() =>
                    FormValidator.ValidateConfirmPassword(password, confirmPassword));
            }

            [Test]
            public void ValidateConfirmPassword_NonMatchingPasswords_ThrowsBadRequestException()
            {
                // Arrange
                string password = "Password123!";
                string confirmPassword = "DifferentPassword123!";

                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateConfirmPassword(password, confirmPassword));
                Assert.That(ex.Message, Is.EqualTo("Passwords do not match."));
            }

            [Test]
            public void ValidateConfirmPassword_CaseSensitiveMatch_ThrowsBadRequestException()
            {
                // Arrange
                string password = "Password123!";
                string confirmPassword = "password123!";

                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateConfirmPassword(password, confirmPassword));
                Assert.That(ex.Message, Is.EqualTo("Passwords do not match."));
            }

            [Test]
            public void ValidateConfirmPassword_SpacesDifferent_ThrowsBadRequestException()
            {
                // Arrange
                string password = "Password123!";
                string confirmPassword = "Password123! ";  // Extra space at end

                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateConfirmPassword(password, confirmPassword));
                Assert.That(ex.Message, Is.EqualTo("Passwords do not match."));
            }

            [TestCase("Password123!", "Password123!", true)]
            [TestCase("Password123!", "DifferentPass123!", false)]
            [TestCase("Pass123!", "pass123!", false)]
            [TestCase("Test123!", "", false)]
            [TestCase("Complex!123", "Complex!123 ", false)]
            public void ValidateConfirmPassword_VariousScenarios(string password, string confirmPassword, bool shouldPass)
            {
                if (shouldPass)
                {
                    Assert.DoesNotThrow(() =>
                        FormValidator.ValidateConfirmPassword(password, confirmPassword));
                }
                else
                {
                    Assert.Throws<BadRequestException>(() =>
                        FormValidator.ValidateConfirmPassword(password, confirmPassword));
                }
            }

            [Test]
            public void ValidateConfirmPassword_SpecialCharactersMatch_NoExceptionThrown()
            {
                // Arrange
                string password = "Pass!@#$%^&*()_+-=<>?";
                string confirmPassword = "Pass!@#$%^&*()_+-=<>?";

                // Act & Assert
                Assert.DoesNotThrow(() =>
                    FormValidator.ValidateConfirmPassword(password, confirmPassword));
            }

            [Test]
            public void ValidateConfirmPassword_LongPasswordMatch_NoExceptionThrown()
            {
                // Arrange
                string password = "VeryLongPassword123!WithLotsOfCharacters";
                string confirmPassword = "VeryLongPassword123!WithLotsOfCharacters";

                // Act & Assert
                Assert.DoesNotThrow(() =>
                    FormValidator.ValidateConfirmPassword(password, confirmPassword));
            }
        }

        [TestFixture]
        public class PasswordValidatorForLoginTests
        {
            [Test]
            public void ValidatePasswordForLogin_WithValidPassword_ShouldNotThrowException()
            {
                // Arrange
                string validPassword = "Password123";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidatePasswordForLogin(validPassword));
            }

            [Test]
            public void ValidatePasswordForLogin_WithNullPassword_ShouldThrowBadRequestException()
            {
                // Arrange
                string nullPassword = null!;

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidatePasswordForLogin(nullPassword!));

                Assert.That(exception.Message, Is.EqualTo("Password is required."));
            }

            [Test]
            public void ValidatePasswordForLogin_WithEmptyPassword_ShouldThrowBadRequestException()
            {
                // Arrange
                string emptyPassword = string.Empty;

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidatePasswordForLogin(emptyPassword));

                Assert.That(exception.Message, Is.EqualTo("Password is required."));
            }

            [Test]
            public void ValidatePasswordForLogin_WithShortPassword_ShouldThrowBadRequestException()
            {
                // Arrange
                string shortPassword = "Pass123"; // 7 characters

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidatePasswordForLogin(shortPassword));

                Assert.That(exception.Message, Is.EqualTo("Password must be at least 8 characters long."));
            }

            [TestCase("")]
            public void ValidatePasswordForLogin_WithNullOrEmptyPassword_ShouldThrowBadRequestException(string password)
            {
                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidatePasswordForLogin(password));

                Assert.That(exception.Message, Is.EqualTo("Password is required."));
            }

            [TestCase("1234567")]
            [TestCase("a")]
            [TestCase("1234")]
            public void ValidatePasswordForLogin_WithPasswordLessThan8Chars_ShouldThrowBadRequestException(string password)
            {
                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidatePasswordForLogin(password));

                Assert.That(exception.Message, Is.EqualTo("Password must be at least 8 characters long."));
            }
        }

        [TestFixture]
        public class ClinicDetailsValidatorTests
        {
            [Test]
            public void ValidateClinicDetails_ValidDetails_NoExceptionThrown()
            {
                var clinicDetails = new ClinicDetails
                {
                    ClinicName = "Test Clinic",
                    PhoneNumber = "1234567890",
                    CountryCode = "+1",
                    Email = "clinic@valid.com",
                    ClinicAddress = "123 Test Street"
                };

                Assert.DoesNotThrow(() => FormValidator.ValidateClinicDetails(clinicDetails));
            }


            [Test]
            public void ValidateClinicDetails_NullDetails_ThrowsBadRequestException()
            {
                Assert.ThrowsAsync<BadRequestException>(() =>
                    FormValidator.ValidateClinicDetails(null!));
            }


        }

        [TestFixture]
        public class DoctorProfileSettingsValidatorTests
        {
            private Mock<MediAssistDbContext> _contextMock;

            [SetUp]
            public void Setup()
            {
                _contextMock = new Mock<MediAssistDbContext>();
            }


            [Test]
            public void ValidateSpecialization_InvalidLength_ThrowsBadRequestException()
            {
                var ex = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateSpecialization("Ca"));
                Assert.That(ex.Message, Is.EqualTo("Specialization must be at least 3 characters long."));
            }
        }

        [TestFixture]
        public class HospitalNameValidatorTests
        {
            [Test]
            public void ValidateHospitalName_WithValidName_ShouldNotThrowException()
            {
                // Arrange
                string validName = "St. Mary's Hospital & Clinic";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateHospitalName(validName));
            }

            [TestCase("")]
            [TestCase("   ")]
            public void ValidateHospitalName_WithNullOrWhiteSpace_ShouldThrowBadRequestException(string clinicName)
            {
                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateHospitalName(clinicName));

                Assert.That(exception.Message, Is.EqualTo("Hospital/Clinic Name is required."));
            }

            [TestCase("A")]
            [TestCase("AB")]
            public void ValidateHospitalName_WithLessThan3Characters_ShouldThrowBadRequestException(string clinicName)
            {
                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateHospitalName(clinicName));

                Assert.That(exception.Message, Is.EqualTo("Hospital/Clinic Name must be at least 3 characters long."));
            }

            [Test]
            public void ValidateHospitalName_WithMoreThan200Characters_ShouldThrowBadRequestException()
            {
                // Arrange
                string longName = new string('A', 201); // Creates a string of 201 'A' characters

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateHospitalName(longName));

                Assert.That(exception.Message, Is.EqualTo("Hospital/Clinic Name must not exceed 200 characters."));
            }

            [TestCase("Hospital@123")]
            [TestCase("Clinic#Name")]
            [TestCase("Hospital$Center")]
            public void ValidateHospitalName_WithInvalidCharacters_ShouldThrowBadRequestException(string clinicName)
            {
                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateHospitalName(clinicName));

                Assert.That(exception.Message, Is.EqualTo("Please Enter Valid Hospital/Clinic Name."));
            }

            [TestCase("City Hospital")]
            [TestCase("St. Mary's Hospital")]
            [TestCase("Children's Medical Center")]
            [TestCase("Health & Wellness Clinic")]
            [TestCase("North-South Medical Center")]
            public void ValidateHospitalName_WithValidNames_ShouldNotThrowException(string clinicName)
            {
                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateHospitalName(clinicName));
            }

            [Test]
            public void ValidateHospitalName_WithExactly3Characters_ShouldNotThrowException()
            {
                // Arrange
                string name = "ABC";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateHospitalName(name));
            }

            [Test]
            public void ValidateHospitalName_WithExactly200Characters_ShouldNotThrowException()
            {
                // Arrange
                string name = new string('A', 200); // Creates a string of 200 'A' characters

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateHospitalName(name));
            }
        }

        [TestFixture]
        public class ClinicAddressValidatorTests
        {
            [Test]
            public void ValidateClinicAddress_WithValidAddress_ShouldNotThrowException()
            {
                // Arrange
                string validAddress = "123 Main Street";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateClinicAddress(validAddress));
            }

            [Test]
            public void ValidateClinicAddress_WithNullAddress_ShouldThrowBadRequestException()
            {
                // Arrange
                string nullAddress = null;

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                  FormValidator.ValidateClinicAddress(nullAddress!));
                Assert.That(exception.Message, Is.EqualTo("Address is required."));
            }

            [Test]
            public void ValidateClinicAddress_WithEmptyAddress_ShouldThrowBadRequestException()
            {
                // Arrange
                string emptyAddress = "";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                   FormValidator.ValidateClinicAddress(emptyAddress));
                Assert.That(exception.Message, Is.EqualTo("Address is required."));
            }

            [Test]
            public void ValidateClinicAddress_WithWhiteSpaceAddress_ShouldThrowBadRequestException()
            {
                // Arrange
                string whiteSpaceAddress = "   ";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                   FormValidator.ValidateClinicAddress(whiteSpaceAddress));
                Assert.That(exception.Message, Is.EqualTo("Address is required."));
            }

            [Test]
            public void ValidateClinicAddress_WithTooShortAddress_ShouldThrowBadRequestException()
            {
                // Arrange
                string shortAddress = "AB";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                   FormValidator.ValidateClinicAddress(shortAddress));
                Assert.That(exception.Message, Is.EqualTo("Address must be at least 3 characters long."));
            }

            [TestCase("123 Main Street")]
            [TestCase("ABC")]
            [TestCase("1234 Long Street Name, City, Country")]
            public void ValidateClinicAddress_WithValidAddresses_ShouldNotThrowException(string address)
            {
                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateClinicAddress(address));
            }
        }

        [TestFixture]
        public class ClinicEmailValidatorTests
        {
            [Test]
            public void ValidateClinicEmail_WithNullEmail_ShouldNotThrowException()
            {
                // Arrange
                string nullEmail = null!;

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateClinicEmail(nullEmail));
            }

            [Test]
            public void ValidateClinicEmail_WithEmptyEmail_ShouldNotThrowException()
            {
                // Arrange
                string emptyEmail = "";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateClinicEmail(emptyEmail));
            }

            [Test]
            public void ValidateClinicEmail_WithWhitespaceEmail_ShouldNotThrowException()
            {
                // Arrange
                string whitespaceEmail = "   ";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateClinicEmail(whitespaceEmail));
            }

            [TestCase("test@gmail.com")]
            [TestCase("user.name@company.com")]
            [TestCase("user-name@domain.co.uk")]
            [TestCase("first.last@subdomain.domain.org")]
            public void ValidateClinicEmail_WithValidEmails_ShouldNotThrowException(string email)
            {
                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateClinicEmail(email));
            }

            [TestCase("@domain.com")]
            [TestCase("user@")]
            [TestCase("user.@domain.com")]
            [TestCase(".user@domain.com")]
            [TestCase("user@domain")]
            [TestCase("user@.com")]
            [TestCase("user@domain.")]
            [TestCase("user name@domain.com")]
            public void ValidateClinicEmail_WithInvalidEmailFormat_ShouldThrowBadRequestException(string invalidEmail)
            {
                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateClinicEmail(invalidEmail));
                Assert.That(exception.Message, Is.EqualTo("Enter a valid email address."));
            }

            [TestCase("test@yopmail.com")]
            [TestCase("user@mailinator.com")]
            [TestCase("test@guerrillamail.com")]
            [TestCase("user@10minutemail.com")]
            [TestCase("test@aol.com")]
            [TestCase("user@example.com")]
            [TestCase("user@test.com")]
            public void ValidateClinicEmail_WithDisposableEmailDomains_ShouldThrowBadRequestException(string disposableEmail)
            {
                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateClinicEmail(disposableEmail));
                Assert.That(exception.Message, Is.EqualTo("The email domain you have entered is not allowed.Please use a valid email provider."));
            }

            [TestCase("TEST@YOPMAIL.COM")]
            [TestCase("User@Mailinator.com")]
            [TestCase("Test.User@GUERRILLAMAIL.COM")]
            public void ValidateClinicEmail_WithDisposableEmailDomainsInDifferentCase_ShouldThrowBadRequestException(string disposableEmail)
            {
                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateClinicEmail(disposableEmail));
                Assert.That(exception.Message, Is.EqualTo("The email domain you have entered is not allowed.Please use a valid email provider."));
            }

            [Test]
            public void ValidateClinicEmail_WithSpecialCharactersInLocalPart_ShouldNotThrowException()
            {
                // Arrange
                string email = "user.name-123@domain.com";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateClinicEmail(email));
            }
        }

        [TestFixture]
        public class ImageFormatValidatorTests
        {
            [Test]
            public void ValidateImageFormat_WithValidJpegFormat_ShouldNotThrowException()
            {
                // Arrange
                string validJpegBase64 = "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEASABIAAD...";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateImageFormat(validJpegBase64, "TestImage"));
            }

            [Test]
            public void ValidateImageFormat_WithValidJpgFormat_ShouldNotThrowException()
            {
                // Arrange
                string validJpgBase64 = "data:image/jpg;base64,/9j/4AAQSkZJRgABAQEASABIAAD...";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateImageFormat(validJpgBase64, "TestImage"));
            }

            [Test]
            public void ValidateImageFormat_WithValidPngFormat_ShouldNotThrowException()
            {
                // Arrange
                string validPngBase64 = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAA...";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateImageFormat(validPngBase64, "TestImage"));
            }

            [Test]
            public void ValidateImageFormat_WithInvalidDataPrefix_ShouldThrowBadRequestException()
            {
                // Arrange
                string invalidPrefix = "invalid:image/jpeg;base64,/9j/4AAQSkZJRgABAQEASABIAAD...";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateImageFormat(invalidPrefix, "TestImage"));
                Assert.That(exception.Message, Is.EqualTo("Invalid file format. Please upload a PNG or JPEG file."));
            }

            [Test]
            public void ValidateImageFormat_WithMissingDataPrefix_ShouldThrowBadRequestException()
            {
                // Arrange
                string missingPrefix = "image/jpeg;base64,/9j/4AAQSkZJRgABAQEASABIAAD...";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateImageFormat(missingPrefix, "TestImage"));
                Assert.That(exception.Message, Is.EqualTo("Invalid file format. Please upload a PNG or JPEG file."));
            }

            [TestCase("data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7")]
            [TestCase("data:image/webp;base64,UklGRh4AAABXRUJQVlA4TBEAAAAvAAAAAAfQ//73v/+BiOh/AAA=")]
            [TestCase("data:image/bmp;base64,Qk1BAAAAAAAAAD4AAAAoAAAAAgAAAAIAAAABABgAAAAAAA==")]
            public void ValidateImageFormat_WithUnsupportedImageFormats_ShouldThrowBadRequestException(string unsupportedFormat)
            {
                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateImageFormat(unsupportedFormat, "TestImage"));
                Assert.That(exception.Message, Is.EqualTo("Invalid file format. Please upload a PNG or JPEG file."));
            }

        }

        [TestFixture]
        public class PhoneNumberValidatorTests
        {
            [Test]
            public void ValidatePhoneNumber_WithNullPhoneNumber_ShouldThrowBadRequestException()
            {
                // Arrange
                string nullPhone = null!;
                string regionCode = "+1";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidatePhoneNumber(nullPhone, regionCode));
                Assert.That(exception.Message, Is.EqualTo("Phone number cannot be left blank."));
            }

            [Test]
            public void ValidatePhoneNumber_WithEmptyPhoneNumber_ShouldThrowBadRequestException()
            {
                // Arrange
                string emptyPhone = "";
                string regionCode = "+1";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidatePhoneNumber(emptyPhone, regionCode));
                Assert.That(exception.Message, Is.EqualTo("Phone number cannot be left blank."));
            }

            [Test]
            public void ValidatePhoneNumber_WithWhitespacePhoneNumber_ShouldThrowBadRequestException()
            {
                // Arrange
                string whitespacePhone = "   ";
                string regionCode = "+1";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                   FormValidator.ValidatePhoneNumber(whitespacePhone, regionCode));
                Assert.That(exception.Message, Is.EqualTo("Phone number cannot be left blank."));
            }

            [Test]
            public void ValidatePhoneNumber_WithInvalidRegionCode_ShouldThrowBadRequestException()
            {
                // Arrange
                string phoneNumber = "1234567890";
                string invalidRegionCode = "+99";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidatePhoneNumber(phoneNumber, invalidRegionCode));
                Assert.That(exception.Message, Is.EqualTo("Invalid country code selected."));
            }

            [TestCase("+1", "202-555-0123")]       // US
            [TestCase("+44", "20 7123 4567")]      // UK
            [TestCase("+91", "99999 99999")]       // India
            [TestCase("+971", "50 123 4567")]      // UAE
            public void ValidatePhoneNumber_WithValidPhoneNumbers_ShouldNotThrowException(string regionCode, string phoneNumber)
            {
                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidatePhoneNumber(phoneNumber, regionCode));
            }

            [TestCase("+1", "123")]                // Too short
            [TestCase("+44", "123456789012345")]   // Too long
            [TestCase("+91", "ABC1234567")]        // Invalid characters
            [TestCase("+971", "000000000")]        // Invalid format
            public void ValidatePhoneNumber_WithInvalidPhoneNumbers_ShouldThrowBadRequestException(string regionCode, string phoneNumber)
            {
                // Act & Assert
                Assert.Throws<BadRequestException>(() => FormValidator.ValidatePhoneNumber(phoneNumber, regionCode));
            }

            [TestCase("123--456")]
            [TestCase("123  456")]
            [TestCase("123- 456")]
            [TestCase("123 -456")]
            public void ValidatePhoneNumber_WithInvalidFormatting_ShouldThrowBadRequestException(string phoneNumber)
            {
                // Arrange
                string regionCode = "+1";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidatePhoneNumber(phoneNumber, regionCode));
                Assert.That(exception.Message, Is.EqualTo("Please enter a valid phone number for US."));
            }

            [Test]
            public void ValidatePhoneNumber_WithValidInternationalFormat_ShouldNotThrowException()
            {
                // Arrange
                string phoneNumber = "+1-202-555-0123";
                string regionCode = "+1";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidatePhoneNumber(phoneNumber, regionCode));
            }

            [Test]
            public void ValidatePhoneNumber_WithMismatchedRegionAndNumber_ShouldThrowBadRequestException()
            {
                // Arrange
                string ukPhoneNumber = "020 7123 4567";
                string usRegionCode = "+1";

                // Act & Assert
                Assert.Throws<BadRequestException>(() => FormValidator.ValidatePhoneNumber(ukPhoneNumber, usRegionCode));
            }

            // Individual test cases for better coverage reporting
            [Test]
            public void ValidatePhoneNumber_WithDoubleHyphen_ShouldThrowBadRequestException()
            {
                // Arrange
                string phoneNumber = "123--456";
                string regionCode = "+1";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                   FormValidator.ValidatePhoneNumber(phoneNumber, regionCode));
                Assert.That(exception.Message, Is.EqualTo("Please enter a valid phone number for US."));
            }

            [Test]
            public void ValidatePhoneNumber_WithSingleSpace_ShouldThrowBadRequestException()
            {
                // Arrange
                string phoneNumber = "123 456";
                string regionCode = "+1";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidatePhoneNumber(phoneNumber, regionCode));
                Assert.That(exception.Message, Is.EqualTo("Please enter a valid phone number for US."));
            }

            [Test]
            public void ValidatePhoneNumber_WithHyphenFollowedBySpace_ShouldThrowBadRequestException()
            {
                // Arrange
                string phoneNumber = "123- 456";
                string regionCode = "+1";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidatePhoneNumber(phoneNumber, regionCode));
                Assert.That(exception.Message, Is.EqualTo("Please enter a valid phone number for US."));
            }

            [Test]
            public void ValidatePhoneNumber_WithSpaceFollowedByHyphen_ShouldThrowBadRequestException()
            {
                // Arrange
                string phoneNumber = "123 -456";
                string regionCode = "+1";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidatePhoneNumber(phoneNumber, regionCode));
                Assert.That(exception.Message, Is.EqualTo("Please enter a valid phone number for US."));
            }

            // Testing multiple format issues in the same number
            [Test]
            public void ValidatePhoneNumber_WithMultipleFormatIssues_ShouldThrowBadRequestException()
            {
                // Arrange
                string phoneNumber = "123-- 456 -789";
                string regionCode = "+1";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidatePhoneNumber(phoneNumber, regionCode));
                Assert.That(exception.Message, Is.EqualTo("Please enter a valid phone number for US."));
            }

            // Testing edge cases with spaces and hyphens at different positions
            [TestCase("--123456")]      // Double hyphen at start
            [TestCase("123456--")]      // Double hyphen at end
            [TestCase(" 123456")]       // Space at start
            [TestCase("123456 ")]       // Space at end
            [TestCase("- 123456")]      // Hyphen space at start
            [TestCase("123456 -")]      // Space hyphen at end
            public void ValidatePhoneNumber_WithValidFormatting_ShouldThrowBadRequestException(string phoneNumber)
            {
                // Arrange
                string regionCode = "+1";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidatePhoneNumber(phoneNumber, regionCode));
                Assert.That(exception.Message, Is.EqualTo("Please enter a valid phone number for US."));
            }
        }

        [TestFixture]
        public class RequirementsValidatorTests
        {
            [Test]
            public void ValidateRequirements_WithNullRequirements_ShouldNotThrowException()
            {
                // Arrange
                string requirements = null!;

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateRequirements(requirements?? string.Empty));
            }

            [Test]
            public void ValidateRequirements_WithEmptyRequirements_ShouldNotThrowException()
            {
                // Arrange
                string requirements = "";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateRequirements(requirements));
            }

            [Test]
            public void ValidateRequirements_WithWhitespaceRequirements_ShouldNotThrowException()
            {
                // Arrange
                string requirements = "    ";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateRequirements(requirements));
            }

            [Test]
            public void ValidateRequirements_WithValidLengthRequirements_ShouldNotThrowException()
            {
                // Arrange
                string requirements = "This is a valid requirement text with proper length.";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateRequirements(requirements));
            }

            [Test]
            public void ValidateRequirements_WithMinimumLengthRequirements_ShouldNotThrowException()
            {
                // Arrange
                string requirements = "1234567890"; // Exactly 10 characters

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateRequirements(requirements));
            }

            [Test]
            public void ValidateRequirements_WithMaximumLengthRequirements_ShouldNotThrowException()
            {
                // Arrange
                string requirements = new string('x', 500); // Exactly 500 characters

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateRequirements(requirements));
            }

            [Test]
            public void ValidateRequirements_WithTooShortRequirements_ShouldThrowBadRequestException()
            {
                // Arrange
                string requirements = "123456789"; // 9 characters

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateRequirements(requirements));
                Assert.That(exception.Message, Is.EqualTo("Requirements must be between 10 and 500 characters long."));
            }

            [Test]
            public void ValidateRequirements_WithTooLongRequirements_ShouldThrowBadRequestException()
            {
                // Arrange
                string requirements = new string('x', 501); // 501 characters

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateRequirements(requirements));
                Assert.That(exception.Message, Is.EqualTo("Requirements must be between 10 and 500 characters long."));
            }

            [Test]
            public void ValidateRequirements_WithRequirementsContainingWhitespace_ShouldTrimBeforeValidation()
            {
                // Arrange
                string requirements = "    Valid requirement with spaces around    ";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateRequirements(requirements));
            }

            [Test]
            public void ValidateRequirements_WithTrimmedRequirementsTooShort_ShouldThrowBadRequestException()
            {
                // Arrange
                string requirements = "    123    "; // Will be too short after trimming

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateRequirements(requirements));
                Assert.That(exception.Message, Is.EqualTo("Requirements must be between 10 and 500 characters long."));
            }
        }

        [TestFixture]
        public class EmailLoginValidatorTests
        {
            [Test]
            public void ValidateEmailForLogin_WithNullEmail_ShouldThrowBadRequestException()
            {
                // Arrange
                string email = null!;

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                   FormValidator.ValidateEmailForLogin(email));
                Assert.That(exception.Message, Is.EqualTo("Email is required."));
            }

            [Test]
            public void ValidateEmailForLogin_WithEmptyEmail_ShouldThrowBadRequestException()
            {
                // Arrange
                string email = "";

                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateEmailForLogin(email));
                Assert.That(exception.Message, Is.EqualTo("Email is required."));
            }

            [Test]
            public void ValidateEmailForLogin_WithValidEmail_ShouldNotThrowException()
            {
                // Arrange
                string email = "test.user@example.com";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateEmailForLogin(email));
            }

            // Invalid Email Format Tests
            [TestCase("@example.com", "Missing local part")]
            [TestCase("test@", "Missing domain")]
            [TestCase("test@example", "Missing top-level domain")]
            [TestCase(".test@example.com", "Starting with dot")]
            [TestCase("test.@example.com", "Ending with dot")]
            [TestCase("test@.example.com", "Domain starting with dot")]
            [TestCase("test@example..com", "Consecutive dots")]
            [TestCase("test@example.c", "TLD too short")]
            [TestCase("test user@example.com", "Contains space")]
            public void ValidateEmailForLogin_WithInvalidEmailFormat_ShouldThrowBadRequestException(string email, string testCase)
            {
                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                   FormValidator.ValidateEmailForLogin(email),
                    $"Test case: {testCase}");
                Assert.That(exception.Message, Is.EqualTo("Enter a valid email address."));
            }

            [Test]
            public void ValidateEmailForLogin_WithLongValidEmail_ShouldNotThrowException()
            {
                // Arrange
                string email = "very.long.email.address123@really.long.domain.name.company.com";

                // Act & Assert
                Assert.DoesNotThrow(() => FormValidator.ValidateEmailForLogin(email));
            }

            // Additional Pattern Tests
            [TestCase("test+user@example.com", "Contains plus")]
            [TestCase("test@example.c", "Single character TLD")]
            public void ValidateEmailForLogin_WithInvalidPatternVariations_ShouldThrowBadRequestException(string email, string testCase)
            {
                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                    FormValidator.ValidateEmailForLogin(email),
                    $"Test case: {testCase}");
                Assert.That(exception.Message, Is.EqualTo("Enter a valid email address."));
            }

            // Special Characters Tests
            [TestCase("test!user@example.com")]
            [TestCase("test#user@example.com")]
            [TestCase("test$user@example.com")]
            [TestCase("test%user@example.com")]
            [TestCase("test*user@example.com")]
            public void ValidateEmailForLogin_WithSpecialCharacters_ShouldThrowBadRequestException(string email)
            {
                // Act & Assert
                var exception = Assert.Throws<BadRequestException>(() =>
                   FormValidator.ValidateEmailForLogin(email));
                Assert.That(exception.Message, Is.EqualTo("Enter a valid email address."));
            }
        }
    }
}
