using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;

namespace WeatherReport.API.Controllers.v1;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "Admin")]
public class EmailController (IEmailService emailService) : ControllerBase
{
    [HttpPost("send-email")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequestDTO emailRequest)
    {
        if (emailRequest == null)
        {
            return BadRequest("Email request is null.");
        }

        await emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);
        return Ok("Email sent successfully.");
    }
}