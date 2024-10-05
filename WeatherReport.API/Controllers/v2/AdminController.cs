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
[Authorize(Roles = "Admin")]
public class AdminController(IStatisticsService statService, IUserService userService) : ControllerBase
{
    [HttpGet("stats")]
    [SwaggerOperation(Summary = "Get system statistics", Description = "Returns various system statistics such as user activity, usage metrics, etc.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Statistics retrieved successfully.", typeof(StatisticsDTO))]
    [SwaggerResponseExample(200, typeof(StatisticsDTOExample))]
    public async Task<IActionResult> GetStats()
    {
        return Ok(await statService.GetStatisticsAsync());
    }

    [HttpGet("users")]
    [SwaggerOperation(Summary = "Get all users", Description = "Returns a list of all users in the system.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User list retrieved successfully.", typeof(IEnumerable<UserInfoDTO>))]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await userService.GetAllUsersAsync());
    }

    [HttpGet("user")]
    [SwaggerOperation(Summary = "Get a specific user", Description = "Returns the details of a specific user by their ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User retrieved successfully.", typeof(UserInfoDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found.")]
    public async Task<IActionResult> GetUsers(int id)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        return Ok(user);
    }

    [HttpPut("user-role")]
    [SwaggerOperation(Summary = "Change a user's role", Description = "Changes the role of a specific user by their ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User role updated successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed to update user role.")]
    public async Task<IActionResult> ChangeUserRole(int userId, UserRole userRole)
    {
        if(await userService.UpdateRole(userId, userRole))
        {
            return Ok();
        }
        return BadRequest("Failed to update user role.");
    }
}