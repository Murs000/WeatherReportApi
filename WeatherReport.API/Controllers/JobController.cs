using Microsoft.AspNetCore.Mvc;
using Quartz;
using WeatherReport.Business.Services.Interfaces;


namespace WeatherReport.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobController(IJobService jobService) : ControllerBase
{
    [HttpPost("save-hourly")]
    public async Task<IActionResult> SaveHourlyJob()
    {
        await jobService.SaveHourlyReportAsync();
        return Ok("Report saved successfully.");
    }
    [HttpPost("send-daily-email")]
    public async Task<IActionResult> DailyEmailJob()
    {
        await jobService.SendDailyEmailAsync();
        return Ok("Email sent successfully.");
    }
    [HttpPost("send-weekly-email")]
    public async Task<IActionResult> WeeklyEmailJob()
    {
        await jobService.SendWeeklyEmailAsync();
        return Ok("Email sent successfully.");
    }
}