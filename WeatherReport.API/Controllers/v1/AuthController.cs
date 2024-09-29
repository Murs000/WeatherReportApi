using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.DataAccess.Entities;

namespace WeatherReport.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[AllowAnonymous]
public class AuthController(IUserService userService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LogIn([FromBody] LoginDTO loginDTO)
    {
        var user = await userService.LogIn(loginDTO);

        if (user == null)
        {
            return BadRequest("Incorrect password or username");
        }
        return Ok(user);
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        await userService.Register(registerDTO);

        return Ok("Registration successful. Please check your email to confirm.");
    }
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmOTP(string username, string token)
    {
        if (await userService.ConfirmOTP(username, token))
        {
            return Ok("Email confirmed. You can now log in.");
        }
        return BadRequest("Invalid confirmation link.");
    }

    [HttpPost("confirm-otp")]
    public async Task<IActionResult> ConfirmPasswordOTP(string username, string token)
    {
        if (await userService.ConfirmOTP(username, token))
        {
            return Ok("Email confirmed. You can now log in.");
        }
        return BadRequest("Invalid confirmation link.");
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO dTO)
    {
        var user = userService.RefreshToken(dTO);
        if(user != null)
        {
            return Ok(user);
        }
        return BadRequest();
    }
    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword(string username)
    {
        await userService.ForgetPassword(username);

        return Ok("Check your email for OTP");
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
    {
        if(await userService.ResetPassword(resetPasswordDTO))
        {
            return Ok("Password reseted");
        }
        return BadRequest(); 
    }
}
