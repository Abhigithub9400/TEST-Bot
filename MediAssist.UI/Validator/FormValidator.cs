using MediAssist.Application.Entities;
using MediAssist.Configurations.Exceptions;
using MediAssist.DbContext;
using Microsoft.EntityFrameworkCore;
using PhoneNumbers;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MediAssist.UI.Validator
{
    public static class FormValidator
    {
        #region PUBLIC METHOS
        public static void ValidateName(string name)
        {
            var namePattern = new Regex(@"^[A-Za-z\s.]+$");

            if (string.IsNullOrEmpty(name))
            {
                throw new BadRequestException("Name is required.");
            }
            if (!namePattern.IsMatch(name))
            {
                throw new BadRequestException("Name must contain only alphabetic characters.");
            }
            if (name.Length < 3)
            {
                throw new BadRequestException("Name must be at least 3 characters long.");
            }
        }

        public static void ValidateEmail(string email)
        {
            var emailPattern = new Regex(@"^[a-zA-Z0-9][\w.-]*[a-zA-Z0-9]@[a-zA-Z0-9]+[\w.-]*[a-zA-Z0-9]\.[a-zA-Z]{2,}$");
            var spamPattern = new Regex(@"^[a-zA-Z]{1,2}\d*$|^\d{5,}$");

            var disposableEmailProviders = new List<string>
                {
                  "yopmail.com", "mailinator.com", "guerrillamail.com", "10minutemail.com","aol.com", "example.com", "a.com", "test.com"

                };

            if (string.IsNullOrEmpty(email))
            {
                throw new BadRequestException("Email is required.");
            }
            if (!emailPattern.IsMatch(email))
            {
                throw new BadRequestException("Enter a valid email address.");
            }

            var emailParts = email.Split('@');
            if (emailParts.Length == 2)
            {
                var localPart = emailParts[0];
                var domain = emailParts[1].ToLower();

                if (disposableEmailProviders.Contains(domain))
                {
                    throw new BadRequestException("The email domain you have entered is not allowed.Please use a valid email provider.");
                }
                if (spamPattern.IsMatch(localPart))
                {
                    throw new BadRequestException("Username must be 8+ characters and include at least one letter.");
                }
                if (domain.Length == 5 || localPart.ToLower() == "user")
                {
                    throw new BadRequestException("Suspicious email addresses are not allowed.");
                }
            }
        }


        public static void ValidateEmailForLogin(string email)
        {
            var emailPattern = new Regex(@"^[a-zA-Z0-9][\w.-]*[a-zA-Z0-9]@[a-zA-Z0-9]+[\w.-]*[a-zA-Z0-9]\.[a-zA-Z]{2,}$");


            if (string.IsNullOrEmpty(email))
            {
                throw new BadRequestException("Email is required.");
            }
            if (!emailPattern.IsMatch(email))
            {
                throw new BadRequestException("Enter a valid email address.");
            }

        }

        public static void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new BadRequestException("Password is required.");
            }
            if (password.Length < 8)
            {
                throw new BadRequestException("Password must be at least 8 characters long.");
            }
            if (!password.Any(char.IsUpper))
            {
                throw new BadRequestException("Password must include at least one uppercase letter.");
            }
            if (!password.Any(char.IsLower))
            {
                throw new BadRequestException("Password must include at least one lowercase letter.");
            }
            if (!password.Any(char.IsDigit))
            {
                throw new BadRequestException("Password must include at least one digit.");
            }
            if (!password.Any(ch => "!@#$%^&*()_+-=<>?".Contains(ch)))
            {
                throw new BadRequestException("Password must include at least one special character.");
            }
        }


        public static void ValidateConfirmPassword(string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(confirmPassword))
            {
                throw new BadRequestException("Please confirm your password.");
            }
            if (confirmPassword != password)
            {
                throw new BadRequestException("Passwords do not match.");
            }
        }

        public static void ValidatePasswordForLogin(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new BadRequestException("Password is required.");
            }
            if (password.Length < 8)
            {
                throw new BadRequestException("Password must be at least 8 characters long.");
            }
        }
        public static void ValidateAgreeTerms(bool agreeTerms)
        {
            if (!agreeTerms)
            {
                throw new BadRequestException("You must agree to the terms.");
            }
        }

        public static async Task ValidateTitleAsync(int titleId, MediAssistDbContext _context)
        {
            var titleExists = await _context.UserTitles.AnyAsync(t => t.Id == titleId);
            if (!titleExists)
            {
                throw new BadRequestException("Invalid Title.");
            }
        }

        public static async Task ValidateGenderAsync(int genderId, MediAssistDbContext _context)
        {
            var genderExists = await _context.Genders.AnyAsync(g => g.Id == genderId);
            if (!genderExists)
            {
                throw new BadRequestException("Invalid Gender.");
            }
        }

        public static async Task ValidateMedicalCredentialsAsync(int medicalCredentialsId, MediAssistDbContext _context)
        {
            var credentialsExists = await _context.MedicalCredentials.AnyAsync(mc => mc.Id == medicalCredentialsId);
            if (!credentialsExists)
            {
                throw new BadRequestException("Invalid Medical Credentials.");
            }
        }

        public static async Task ValidateClinicDetails(ClinicDetails clinicDetails)
        {
            if (clinicDetails is null)
            {
                throw new BadRequestException("Clinic data is required.");
            }

            ValidateHospitalName(clinicDetails.ClinicName);

            ValidatePhoneNumber(clinicDetails.PhoneNumber,clinicDetails.CountryCode);
            ValidateClinicEmail(clinicDetails.Email);
            ValidateClinicAddress(clinicDetails.ClinicAddress);

            if (!string.IsNullOrEmpty(clinicDetails.Logo))
            {
                ValidateImageFormat(clinicDetails.Logo, "Logo");
            }
        }



        public static async Task ValidateDoctorProfileSettings(DoctorProfileSettings doctorProfileSettings, MediAssistDbContext mediAssistDbContext)
        {
            if (doctorProfileSettings is null)
            {
                throw new BadRequestException("Doctor profile settings are required.");
            }

            if (!string.IsNullOrEmpty(doctorProfileSettings.Signature))
            {
                ValidateImageFormat(doctorProfileSettings.Signature, "Signature");
            }

            ValidateSpecialization(doctorProfileSettings.Specialization);

            await ValidateMedicalCredentialsAsync(doctorProfileSettings.MedicalCredentials, mediAssistDbContext);
        }

        public static void ValidateSpecialization(string specialization)
        {
            if (string.IsNullOrEmpty(specialization))
            {
                throw new BadRequestException("Specialization is required.");
            }

            if (specialization.Length < 3)
            {
                throw new BadRequestException("Specialization must be at least 3 characters long.");
            }
        }
        public static void ValidateHospitalName(string ClinicName)
        {
            var namePattern = new Regex(@"^[A-Za-z\s'-.&]+$");

            if (string.IsNullOrWhiteSpace(ClinicName))
            {
                throw new BadRequestException("Hospital/Clinic Name is required.");
            }
            if (!namePattern.IsMatch(ClinicName))
            {
                throw new BadRequestException("Please Enter Valid Hospital/Clinic Name.");
            }
            if (ClinicName.Length < 3)
            {
                throw new BadRequestException("Hospital/Clinic Name must be at least 3 characters long.");
            }
            if (ClinicName.Length > 200)
            {
                throw new BadRequestException("Hospital/Clinic Name must not exceed 200 characters.");
            }
        }
        public static void ValidateClinicAddress(string ClinicAddress)
        {
            if (string.IsNullOrWhiteSpace(ClinicAddress))
            {
                throw new BadRequestException("Address is required.");
            }
            if (ClinicAddress.Length < 3)
            {
                throw new BadRequestException("Address must be at least 3 characters long.");
            }
        }


        public static void ValidateClinicEmail(string ClinicEmail)
        {
            var emailPattern = new Regex(@"^[a-zA-Z0-9][\w.-]*[a-zA-Z0-9]@[a-zA-Z0-9]+[\w.-]*[a-zA-Z0-9]\.[a-zA-Z]{2,}$");

            var disposableEmailProviders = new List<string>
                {
                  "yopmail.com", "mailinator.com", "guerrillamail.com", "10minutemail.com","aol.com", "example.com", "a.com", "test.com"

                };
            if (string.IsNullOrWhiteSpace(ClinicEmail))
            {
                return;
            }
            if (!emailPattern.IsMatch(ClinicEmail))
            {
                throw new BadRequestException("Enter a valid email address.");
            }

            var emailParts = ClinicEmail.Split('@');
            if (emailParts.Length == 2)
            {
                var domain = emailParts[1].ToLower();

                if (disposableEmailProviders.Contains(domain))
                {
                    throw new BadRequestException("The email domain you have entered is not allowed.Please use a valid email provider.");
                }
            }
        }

        public static void ValidateImageFormat(string base64String, string fieldName)
        {
            if (!base64String.StartsWith("data:image/"))
            {
                throw new BadRequestException("Invalid file format. Please upload a PNG or JPEG file.");
            }

            string mimeType = base64String.Split(',')[0].Split(';')[0].Split(':')[1].ToLower();

            if (mimeType != "image/jpeg" && mimeType != "image/jpg" && mimeType != "image/png")
            {
                throw new BadRequestException("Invalid file format. Please upload a PNG or JPEG file.");
            }
        }

        public static void ValidatePhoneNumber(string phoneNumber, string regionCode)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new BadRequestException("Phone number cannot be left blank.");
            }
            var countryCodeToRegionCodeMap = new Dictionary<string, string>
                {
                    { "+1", "US" },    // United States
                    { "+44", "GB" },   // United Kingdom
                    { "+91", "IN" },   // India
                    { "+971", "AE" }   // United Arab Emirates
                };

            // Check if the provided country code is valid
            if (!countryCodeToRegionCodeMap.ContainsKey(regionCode))
            {
                throw new BadRequestException("Invalid country code selected.");
            }

            string countryCode = countryCodeToRegionCodeMap[regionCode];
            var phoneUtil = PhoneNumberUtil.GetInstance();

            var number = phoneUtil.Parse(phoneNumber, countryCode);

            bool isValid = phoneUtil.IsValidNumber(number);

            if (!isValid)
            {
                throw new BadRequestException($"Please enter a valid phone number for {GetCountryCode(regionCode)}.");
            }

            if (phoneNumber.Contains("--") ||
                phoneNumber.Contains("  ") ||
                phoneNumber.Contains("- ") ||
                phoneNumber.Contains(" -"))
            {
                throw new BadRequestException("Please enter a valid phone number.");
            }
        }

        public static void ValidateRequirements(string requirements)
        {
            requirements = requirements.Trim();

            if (!string.IsNullOrEmpty(requirements) && (requirements.Length < 10 || requirements.Length > 500))
            {
                throw new BadRequestException("Requirements must be between 10 and 500 characters long.");
            }
        }


        #endregion

        #region PRIVATE METHODS

        private static string GetCountryCode(string countryCode)
        {
            return countryCode switch
            {
                "+1" => "US",
                "+44" => "UK",
                "+91" => "IND",
                "+971" => "UAE",
                _ => "the selected country"
            };
        }
        //private static void ValidatePhoneNumber(string phoneNumber)
        //{
        //    if (string.IsNullOrEmpty(phoneNumber))
        //    {
        //        throw new BadRequestException("Hospital/Clinic Phone Number is required.");
        //    }

        //    string cleanedPhone = phoneNumber.Replace(" ", "").Replace("-", "");
        //    if (!cleanedPhone.All(char.IsDigit))
        //    {
        //        throw new BadRequestException("Please enter a valid phone number.");
        //    }
        //    if (phoneNumber.Contains("--") ||
        //        phoneNumber.Contains("  ") ||
        //        phoneNumber.Contains("- ") ||
        //        phoneNumber.Contains(" -"))
        //    {
        //        throw new BadRequestException("Please enter a valid phone number.");
        //    }
        //}

        #endregion
    }
}

