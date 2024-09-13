using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.DataAccess.Entities;

namespace WeatherReport.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController (IEmailService emailService,IServiceUnitOfWork service,IMapper mapper) : ControllerBase
{
    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOTPEmail(string userEmail)
    {
        await emailService.SendEmailAsync(userEmail, "OTP email", $"{1234}");
        return Ok();
    }
    [HttpPost("login")]
    public async Task<IActionResult> LogIn([FromBody] LoginDTO loginDTO)
    {
        var users = await service.SubscriberService.GetAllAsync();
        var user = users.First(u => u.Username == loginDTO.Username);
        if(VerifyPassword(loginDTO.Password,user.PasswordHash,user.PasswordSalt))
        {
            return Ok(GenerateJwtToken(user.Id,user.Username,user.UserRole));
        }
        return BadRequest("Incorrect password or username");
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO registerDTO)
    {
        var subscriberDTO = mapper.Map<SubscriberDTO>(registerDTO);
        (var passwordHash, var passwordSalt) = HashPassword(registerDTO.Password);

        subscriberDTO.PasswordHash = passwordHash;
        subscriberDTO.PasswordSalt = passwordSalt;

        var subscriber = await service.SubscriberService.AddAsync(subscriberDTO);

        return Ok(subscriber);
    }
    private (string passwordHash, string passwordSalt) HashPassword(string password)
        {
            using var hmac = new HMACSHA256();
            var salt = hmac.Key;
            var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            var passwordSalt = Convert.ToBase64String(salt);
            return (passwordHash, passwordSalt);
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            using var hmac = new HMACSHA256(Convert.FromBase64String(storedSalt));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(computedHash) == storedHash;
        }

        private string GenerateJwtToken(int userId, string username, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ih5GO96Tw3WQJ4pl5jMmwKAwrXfBYRbcRUwp/kqCTJU="));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
}