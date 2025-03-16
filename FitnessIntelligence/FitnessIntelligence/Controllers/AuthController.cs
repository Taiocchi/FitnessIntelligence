using FitnessIntelligence.Model;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FitnessIntelligence.Controllers
{

    public class TokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }


        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] string credential)
        {
            //valido l'id token ricevuto a seguito di login da google
            string idToken = credential;

            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { "795875881614-6url7c5tbg8ptaemrpo8co0o5tvu9cbt.apps.googleusercontent.com" }
                };

                var validPayload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                //se arrivo qui senza eccezioni id token valido
                //genero un token jwt di accesso da rilasciare
                var token = GenerateJwtAccessToken(validPayload.Email,1);
                //dovrebbe essere memorizzato per essere invaldiato
                var refreshToken = GenerateJwtRefreshToken(validPayload.Email, 10);

                return Ok(new { AccessToken = token, RefreshToken = refreshToken });
            }
            catch (InvalidJwtException ex)
            {
                return Unauthorized(new { message = "Token non valido", error = ex.Message });
            }

        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] TokenRequest request)
        {
            
            //valida il refresh token
            var handler = new JwtSecurityTokenHandler();
            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]))
            };

            try
            {
                var claimsPrincipal = handler.ValidateToken(request.RefreshToken, tokenValidationParams, out _);
                var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                    return Unauthorized(new { message = "Token non valido", error = "email non trovata" });


                //se ok returno nuova coppia di token

                var newAccessToken = GenerateJwtAccessToken("ciao@ciao.it", 1);
                var newRefreshToken = GenerateJwtRefreshToken("ciao@ciao.it", 10);
                return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = "Token non valido", error = ex.Message });
            }



            
        }


        private string GenerateJwtAccessToken(string email, int expirationTimeHours)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: new List<Claim> { new Claim(ClaimTypes.Email, email) },
                expires: DateTime.UtcNow.AddHours(expirationTimeHours),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateJwtRefreshToken(string email, int expirationTimeHours)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: new List<Claim> { new Claim(ClaimTypes.Email, email) },
                expires: DateTime.UtcNow.AddHours(expirationTimeHours),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }

}
