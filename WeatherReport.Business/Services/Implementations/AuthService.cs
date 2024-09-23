using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.Business.Settings;

namespace WeatherReport.Business.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;

        public AuthService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        // Hash password without async, since it's a CPU-bound operation
        public (string passwordHash, string passwordSalt) HashPassword(string password)
        {
            using var hmac = new HMACSHA256();
            var salt = hmac.Key;
            var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            var passwordSalt = Convert.ToBase64String(salt);
            return (passwordHash, passwordSalt);
        }

        // Verify password without async
        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            using var hmac = new HMACSHA256(Convert.FromBase64String(storedSalt));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(computedHash) == storedHash;
        }

        // Generate JWT token
        public string GenerateJwtToken(int userId, string username, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.AccessKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(4).AddMinutes(_jwtSettings.AccessExpireAt),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Generate Refresh Token
        public string GenerateRefreshToken()
        {
            return GenerateToken(_jwtSettings.RefreshKey, _jwtSettings.RefreshExpireAt);
        }

        // Validate Refresh Token with expiration check
        public bool ValidateRefreshToken(string token)
        {
            return ValidateToken(token, _jwtSettings.RefreshKey);
        }

        // Generate Email Token
        public string GenerateEmailToken()
        {
            return GenerateToken(_jwtSettings.EmailKey, _jwtSettings.EmailExpireAt);
        }

        // Validate Email Token
        public bool ValidateEmailToken(string token)
        {
            return ValidateToken(token, _jwtSettings.EmailKey);
        }

        // Generate token
        private string GenerateToken(string authKey, int expireAt)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(expireAt).ToUnixTimeSeconds().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireAt),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Validate token without try-catch
        private bool ValidateToken(string token, string authKey)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true, // Ensure lifetime validation
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key
            };

            // Validate the token
            var validatedToken = handler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
            var jwtToken = securityToken as JwtSecurityToken;

            // If the token is expired or invalid, return false
            if (jwtToken == null || jwtToken.ValidTo < DateTime.UtcNow)
            {
                return false;
            }

            return true; // Token is valid
        }
    }
}