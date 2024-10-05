using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace WeatherReport.API.Controllers.v2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[AllowAnonymous]
[SwaggerTag("Operations related to user authentication and account management.")]
public class AuthController(IUserService userService) : ControllerBase
{

    [HttpPost("login")]
    [SwaggerOperation(Summary = "Log in a user", Description = "Authenticates a user and returns a JWT token.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully logged in.", typeof(string))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Incorrect password or username.")]
    public async Task<IActionResult> LogIn([FromBody] LoginDTO loginDTO)
    {
        var user = await userService.LogIn(loginDTO);
        if (user == null)
        {
            return Unauthorized("Incorrect password or username.");
        }
        return Ok(user);
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register a new user", Description = "Registers a new user in the system.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Registration successful. Please check your email to confirm.")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        await userService.Register(registerDTO);
        return Ok("Registration successful. Please check your email to confirm.");
    }

    [HttpPost("confirm-email")]
    [SwaggerOperation(Summary = "Confirm email", Description = "Confirms the user's email using a token sent to the email address.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Email confirmed. You can now log in.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid confirmation link.")]
    public async Task<IActionResult> ConfirmEmail(string username, string token)
    {
        if (await userService.ConfirmOTP(username, token))
        {
            return Ok("Email confirmed. You can now log in.");
        }
        return BadRequest("Invalid confirmation link.");
    }

    [HttpPost("confirm-otp")]
    [SwaggerOperation(Summary = "Confirm OTP for password reset", Description = "Confirms the OTP for password reset.")]
    [SwaggerResponse(StatusCodes.Status200OK, "OTP confirmed. You can now reset your password.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid OTP.")]
    public async Task<IActionResult> ConfirmPasswordOTP(string username, string token)
    {
        if (await userService.ConfirmOTP(username, token))
        {
            return Ok("OTP confirmed. You can now reset your password.");
        }
        return BadRequest("Invalid OTP.");
    }

    [HttpPost("refresh-token")]
    [SwaggerOperation(Summary = "Refresh JWT token", Description = "Refreshes the JWT token using a valid refresh token.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully refreshed the token.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid refresh token.")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO dto)
    {
        var user = await userService.RefreshToken(dto);
        if (user != null)
        {
            return Ok(user);
        }
        return Unauthorized("Invalid refresh token.");
    }

    [HttpPost("forget-password")]
    [SwaggerOperation(Summary = "Send OTP for password reset", Description = "Sends an OTP to the user's email to reset the password.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Check your email for the OTP.")]
    public async Task<IActionResult> ForgetPassword(string username)
    {
        await userService.ForgetPassword(username);
        return Ok("Check your email for the OTP.");
    }

    [HttpPost("reset-password")]
    [SwaggerOperation(Summary = "Reset user password", Description = "Resets the user's password using the provided OTP and new password details.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Password has been reset.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed to reset password.")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
    {
        if (await userService.ResetPassword(resetPasswordDTO))
        {
            return Ok("Password has been reset.");
        }
        return BadRequest("Failed to reset password.");
    }
}