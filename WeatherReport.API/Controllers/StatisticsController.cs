using Microsoft.AspNetCore.Mvc;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;


namespace WeatherReport.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaticticsController(ISubscriberService subscriberService) : ControllerBase
{
    [HttpGet("City")]
    public async Task<IActionResult> GetCityPercentageAsync()
    {
        var subscribers = await subscriberService.GetAllAsync();
        var cities = subscribers.Select(s=>s.CityOfResidence);
        var uniqueCities = subscribers.Select(s=>s.CityOfResidence).Distinct();
        var stats = new Dictionary<string,int>();
        foreach(var city in uniqueCities)
        {
            var percentageOfCity = cities.Where(c=>c == city).ToList().Count;
            stats.Add(city,percentageOfCity);
        }
        return Ok(stats);
    }
    [HttpGet("Email-stats")]
    public async Task<IActionResult> GetEmailStatsAsync()
    {
        return Ok();
    }
}