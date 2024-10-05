using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;
using AutoMapper;
using WeatherReport.DataAccess.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace WeatherReport.API.Controllers.v2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
[SwaggerTag("Operations related to subscriber management and weather information.")]
public class SubscriberController(ICustomerService service) : ControllerBase
{
    [HttpGet("current")]
    [SwaggerOperation(Summary = "Get Current Subscriber", Description = "Retrieves the current logged-in subscriber's information.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved the subscriber.", typeof(UserDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized.")]
    public async Task<IActionResult> GetCurrent()
    {
        var userData = await service.GetCurrent();
        if (userData == null)
        {
            return Unauthorized("User ID not found in claims.");
        }

        return Ok(userData);
    }
    [HttpGet("weather-now")]
    [SwaggerOperation(Summary = "Get Current Weather", Description = "Retrieved Current Weather Condition")]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved the current weather.", typeof(CurrentWeatherDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized.")]
    public async Task<IActionResult> GetCurrentWeather()
    {
        var weatherInfo = await service.GetCurrentWeather();
        if (weatherInfo == null)
        {
            return Unauthorized("User ID not found in claims.");
        }

        return Ok(weatherInfo);
    }

    [HttpPut("change-subscription")]
    [SwaggerOperation(Summary = "Change Subscription", Description = "Changes the subscriber's subscription type.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Subscription successfully updated.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized.")]
    public async Task<IActionResult> ChangeSubscription([FromQuery] SubscriptionType? type)
    {
        var result = await service.ChangeSubscription(type);
        if (result == false)
        {
            return Unauthorized("User ID not found in claims.");
        }

        return Ok("Subscription successfully updated.");
    }

    [HttpPut("change-city")]
    [SwaggerOperation(Summary = "Change City", Description = "Changes the subscriber's city of residence.")]
    [SwaggerResponse(StatusCodes.Status200OK, "City successfully updated.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized.")]
    public async Task<IActionResult> ChangeCity([FromQuery] string? city)
    {
        var result = await service.ChangeCity(city);
        if (result == false)
        {
            return Unauthorized("User ID not found in claims.");
        }

        return Ok("City successfully updated.");
    }

    [HttpPut("change-credentials")]
    [SwaggerOperation(Summary = "Change Credentials", Description = "Changes the subscriber's name or surname.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Credentials successfully updated.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized.")]
    public async Task<IActionResult> ChangeCridentials([FromQuery] string? name, [FromQuery] string? surname)
    {
        var result = await service.ChangeCridentials(name, surname);
        if (result == false)
        {
            return Unauthorized("User ID not found in claims.");
        }

        return Ok("Credentials successfully updated.");
    }
}