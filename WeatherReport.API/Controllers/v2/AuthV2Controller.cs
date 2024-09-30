using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;

namespace WeatherReport.API.Controllers.v2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[AllowAnonymous]
public class AuthController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Authenticates a user and returns a token upon successful login.
    /// </summary>
    /// <param name="loginDTO">The login details including username and password.</param>
    /// <returns>JWT token if successful; 401 Unauthorized if login fails.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> LogIn([FromBody] LoginDTO loginDTO)
    {
        var user = await userService.LogIn(loginDTO);
        if (user == null)
        {
            return Unauthorized("Incorrect password or username.");
        }
        return Ok(user);
    }

    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="registerDTO">The registration details including username, password, and email.</param>
    /// <returns>A success message if registration is successful.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        await userService.Register(registerDTO);
        return Ok("Registration successful. Please check your email to confirm.");
    }

    /// <summary>
    /// Confirms the user's email using a token sent to the email address.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="token">The confirmation token sent via email.</param>
    /// <returns>A success message if the email is confirmed; 400 Bad Request if the token is invalid.</returns>
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string username, string token)
    {
        if (await userService.ConfirmOTP(username, token))
        {
            return Ok("Email confirmed. You can now log in.");
        }
        return BadRequest("Invalid confirmation link.");
    }

    /// <summary>
    /// Confirms the OTP for password reset.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="token">The OTP token.</param>
    /// <returns>A success message if the OTP is confirmed; 400 Bad Request if the OTP is invalid.</returns>
    [HttpPost("confirm-otp")]
    public async Task<IActionResult> ConfirmPasswordOTP(string username, string token)
    {
        if (await userService.ConfirmOTP(username, token))
        {
            return Ok("OTP confirmed. You can now reset your password.");
        }
        return BadRequest("Invalid OTP.");
    }

    /// <summary>
    /// Refreshes the JWT token using a valid refresh token.
    /// </summary>
    /// <param name="dto">The refresh token details.</param>
    /// <returns>New JWT token if successful; 401 Unauthorized if the token is invalid.</returns>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO dto)
    {
        var user = await userService.RefreshToken(dto);
        if (user != null)
        {
            return Ok(user);
        }
        return Unauthorized("Invalid refresh token.");
    }

    /// <summary>
    /// Sends an OTP to the user's email to reset the password.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>A message indicating that the OTP has been sent.</returns>
    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword(string username)
    {
        await userService.ForgetPassword(username);
        return Ok("Check your email for the OTP.");
    }

    /// <summary>
    /// Resets the user's password using the provided OTP and new password details.
    /// </summary>
    /// <param name="resetPasswordDTO">The password reset details including the OTP, new password, and username.</param>
    /// <returns>A success message if the password is reset; 400 Bad Request if the reset fails.</returns>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
    {
        if (await userService.ResetPassword(resetPasswordDTO))
        {
            return Ok("Password has been reset.");
        }
        return BadRequest("Failed to reset password.");
    }
}