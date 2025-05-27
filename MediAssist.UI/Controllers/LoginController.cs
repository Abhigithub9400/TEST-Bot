using MediAssist.Application.Abstract.Services;
using MediAssist.Infrastructure.Abstract.Configurations;
using MediAssist.UI.Models;
using MediAssist.UI.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MediAssist.UI.Controllers
{
    [Route("api/manage")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region PRIVATE FIELDS
        private readonly IUserService _userService;
        private readonly IAppSettings _appSettings;
        private readonly ILogger<LoginController> _logger;

        #endregion

        #region CONSTRUCTOR
        public LoginController(IUserService userService, IAppSettings appSettings, ILogger<LoginController> logger)
        {
            _userService = userService;
            _appSettings = appSettings;
            _logger = logger;
        }
        #endregion

        #region PUBLIC METHODS
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            try
            {
                // Validate input
                FormValidator.ValidateEmailForLogin(loginViewModel.Email);
                FormValidator.ValidatePasswordForLogin(loginViewModel.Password);

                // Attempt to sign in
                var response = await _userService.SignInWithEmailAndPasswordAsync(loginViewModel.Email, loginViewModel.Password);

                if (response.HttpStatusCode == HttpStatusCode.OK || response.HttpStatusCode == HttpStatusCode.Created)
                {

                    var authToken = GenerateAuthToken(loginViewModel.Email, response.UserId);

                    DateTime expirationDate = DateTime.UtcNow.AddDays(_appSettings.ExpirationDays);

                    var cookieOptions = new CookieOptions
                    {
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = expirationDate
                    };

                    if (loginViewModel.RememberMe)
                    {
                        cookieOptions.Expires = expirationDate;
                    }

                    Response.Cookies.Append("sessionToken", authToken, cookieOptions);

                    var result = new
                    {
                        success = true,
                        authToken = authToken,
                        redirectUrl = "/dashboard",
                        expirationTime = expirationDate,
                        UserId = response.UserId,
                        FullName = response.FullName,
                        FirstName = response.FirstName,
                        Image = response.Image,
                        Title = response.TitleAbbreviation,
                        Specialization = response.Specialization,
                        IsSettingsUpdated = response.IsSettingsUpdated,
                    };

                    return Ok(result);
                    
                }

                if (response.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound(new { success = false, message = "No account found with this email. Verify your input or create a new account" });
                }

                if (response.HttpStatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized(new { success = false, message = "Incorrect email id or password." });
                }

                return StatusCode((int)HttpStatusCode.InternalServerError, new { success = false, message = "An error occurred. Please try again." });
            }
            catch (BadRequestException ex)
            {
                _logger.LogError("Bad Request {0}",ex);

                return BadRequest(new { success = false, message = ex.Message });

            }
            catch (Exception ex)
            {
                _logger.LogError("Bad Request {0}",ex);

                return StatusCode((int)HttpStatusCode.InternalServerError, new { success = false, message = "An error occurred. Please try again." });
            }
        }


        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Logout()
        {
            
            // Remove the session token by setting an expired cookie
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("sessionToken", "", cookieOptions);

            return Ok(new { success = true, message = "Successfully logged out." });
        }
        #endregion

        #region PRIVATE METHODS

        private string GenerateAuthToken(string email, string userId)
        {
            // Determine environment and choose appropriate settings
            var key = _appSettings.Key;

            var issuer = _appSettings.Issuer;

            var audience = _appSettings.Audience;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId),
                // Additional claims as needed
            };

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key.PadRight(32, '0')));
            var creds = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
