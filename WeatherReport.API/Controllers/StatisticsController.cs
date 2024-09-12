using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;


namespace WeatherReport.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class StaticticsController(IStatisticsService service) : ControllerBase
{
    [HttpGet("City-stats")]
    public async Task<IActionResult> GetCityStats()
    {
        return Ok(await service.GetCityStats());
    }
    [HttpGet("Email-stats")]
    public async Task<IActionResult> GetEmailStats()
    {
        return Ok(await service.GetEmailStats());
    }
    [HttpGet("Subsription-stats")]
    public async Task<IActionResult> GetSubsciptionsStats()
    {
        return Ok(await service.GetSubsciptionsStats());
    }
}