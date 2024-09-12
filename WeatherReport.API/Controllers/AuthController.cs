using Microsoft.AspNetCore.Mvc;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;

namespace WeatherReport.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController (IEmailService emailService) : ControllerBase
{
    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOTPEmail(string userEmail)
    {
        emailService.SendEmailAsync(userEmail, "OTP email", $"{1234}");
        return Ok();
    }
    [HttpPost("login")]
    public async Task<IActionResult> LogIn([FromBody] LoginDTO loginDTO)
    {
        return Ok();
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO registerDTO)
    {
        return Ok();
    }
}