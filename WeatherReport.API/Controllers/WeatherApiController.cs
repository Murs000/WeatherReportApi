using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;

namespace WeatherReport.API.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class WeatherApiController (IWeatherApiService weatherApiService) : ControllerBase
{
    [HttpGet("current-weather")]
    public async Task<ActionResult<string>> GetCurrentWeather([FromQuery] string cityName)
    {
        if (string.IsNullOrEmpty(cityName))
        {
            return BadRequest("City name is required.");
        }

        var weatherData = await weatherApiService.GetCurrentWeatherDataAsync(cityName);
        return Ok(weatherData);
    }
    [HttpGet("next-week-weather")]
    public async Task<ActionResult<string>> GetForWeekWeather([FromQuery] string cityName)
    {
        if (string.IsNullOrEmpty(cityName))
        {
            return BadRequest("City name is required.");
        }

        var weatherData = await weatherApiService.GetForWeekWeatherDataAsync(cityName);
        return Ok(weatherData);
    }
}