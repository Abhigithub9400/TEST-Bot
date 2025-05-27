using MediAssist.Application.Abstract.Services;
using MediAssist.Infrastructure.Abstract.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MediAssist.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region PRIVATE FIELDS
        private readonly IUserService _userService;
        private readonly IAppSettings _appSettings;
        private readonly ILogger<AuthController> _logger;

        #endregion

        #region CONSTRUCTOR
        public AuthController(IUserService userService, IAppSettings appSettings, ILogger<AuthController> logger)
        {
            _userService = userService;
            _appSettings = appSettings;
            _logger = logger;
        }
        #endregion

        [HttpGet]
        [Route("GetAuthToken")]
        public async Task<IActionResult> GetAuthToken(string email,string password)
        {
            try
            {
                var authToken = "";
                var response = await _userService.SignInWithEmailAndPasswordAsync(email, password);

                if (response.HttpStatusCode == HttpStatusCode.OK || response.HttpStatusCode == HttpStatusCode.Created)
                {

                     authToken = GenerateAuthToken(email, response.UserId);
                }
                else
                {
                    _logger.LogError("here i am find me ");
                    return Unauthorized();
                }

                return Ok(authToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Bad Request {0}", ex);
                throw;
            }
        }


        #region Private Methods 
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
