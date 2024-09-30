using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;
using AutoMapper;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.API.Controllers.v2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class SubscriberController(IServiceUnitOfWork service, IMapper mapper) : ControllerBase
{
    [HttpGet("current")]
    [SwaggerOperation(Summary = "Get Current Subscriber", Description = "Retrieves the current logged-in subscriber's information.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved the subscriber.", typeof(UserDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized.")]
    public async Task<IActionResult> GetCurrent()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("User ID not found in claims.");
        }

        return Ok(mapper.Map<UserDTO>(await service.SubscriberService.GetByIdAsync(Convert.ToInt32(userId))));
    }

    [HttpGet("change-subscription")]
    [SwaggerOperation(Summary = "Change Subscription", Description = "Changes the subscriber's subscription type.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Subscription successfully updated.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized.")]
    public async Task<IActionResult> ChangeSubscription([FromQuery] SubscriptionType? type)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("User ID not found in claims.");
        }

        var subscriberDTO = await service.SubscriberService.GetByIdAsync(Convert.ToInt32(userId));

        if (type != null)
            subscriberDTO.SubscriptionType = (SubscriptionType)type;

        await service.SubscriberService.UpdateAsync(subscriberDTO);

        return Ok("Subscription successfully updated.");
    }

    [HttpGet("change-city")]
    [SwaggerOperation(Summary = "Change City", Description = "Changes the subscriber's city of residence.")]
    [SwaggerResponse(StatusCodes.Status200OK, "City successfully updated.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized.")]
    public async Task<IActionResult> ChangeCity([FromQuery] string? city)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("User ID not found in claims.");
        }

        var subscriberDTO = await service.SubscriberService.GetByIdAsync(Convert.ToInt32(userId));

        if (city != null)
            subscriberDTO.CityOfResidence = city;

        await service.SubscriberService.UpdateAsync(subscriberDTO);

        return Ok("City successfully updated.");
    }

    [HttpGet("change-credentials")]
    [SwaggerOperation(Summary = "Change Credentials", Description = "Changes the subscriber's name or surname.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Credentials successfully updated.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized.")]
    public async Task<IActionResult> ChangeCridentials([FromQuery] string? name, [FromQuery] string? surname)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("User ID not found in claims.");
        }

        var subscriberDTO = await service.SubscriberService.GetByIdAsync(Convert.ToInt32(userId));

        if (name != null)
            subscriberDTO.Name = name;
        if (surname != null)
            subscriberDTO.Surname = surname;

        await service.SubscriberService.UpdateAsync(subscriberDTO);

        return Ok("Credentials successfully updated.");
    }
}