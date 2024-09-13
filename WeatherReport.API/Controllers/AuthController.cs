using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
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
    [HttpGet("confirm-email")]
    public async Task<IActionResult> SendOTPEmail(string username, string token)
    {
        var users = await service.SubscriberService.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Username == username);

        var decodedToken = Uri.UnescapeDataString(token);

        if (user.RefreshToken != decodedToken)
        {
            return BadRequest("Invalid confirmation link.");
        }

        user.IsActivated = true;
        user.RefreshToken = null;  // Clear token after confirmation

        await service.SubscriberService.UpdateAsync(user); 

        return Ok("Email confirmed. You can now log in.");
    }
    private async Task SendConfirmationEmailAsync(SubscriberDTO user)
    {
        var encodedToken = Uri.EscapeDataString(user.RefreshToken);
        await emailService.SendEmailAsync(user.Email, "OTP email",$"Please confirm your email by clicking the link: {"http://localhost:5109"}/api/Auth/confirm-email?username={user.Username}&token={encodedToken}");
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LogIn([FromBody] LoginDTO loginDTO)
    {
        var users = await service.SubscriberService.GetAllAsync();
        var user = users.First(u => u.Username == loginDTO.Username && u.IsActivated == true) ?? new SubscriberDTO();
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

        subscriberDTO.RefreshToken = GenerateRefreshToken();

        var subscriber = await service.SubscriberService.AddAsync(subscriberDTO);
        await SendConfirmationEmailAsync(subscriberDTO);

        return Ok("Registration successful. Please check your email to confirm.");
    }
    // [HttpPost("refresh-token")]
    // public IActionResult RefreshToken([FromBody] RefreshTokenRequest model)
    // {
    //     var user = users.FirstOrDefault(u => u.Username == model.Username && u.RefreshToken == model.RefreshToken);
    //     if (user == null || user.RefreshTokenExpiryTime <= DateTime.Now)
    //     {
    //         return Unauthorized("Invalid refresh token or token has expired.");
    //     }

    //     var newAccessToken = GenerateJwtToken(user);
    //     var newRefreshToken = GenerateRefreshToken();

    //     user.RefreshToken = newRefreshToken;
    //     user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

    //     return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
    // }
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
                expires: DateTime.Now.AddMinutes(180),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}