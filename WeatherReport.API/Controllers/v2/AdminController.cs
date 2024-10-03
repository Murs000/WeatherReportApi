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
[Authorize(Roles = "Admin")]
public class AdminController(IStatisticsService statService, IUserService userService, IServiceUnitOfWork service) : ControllerBase
{
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        return Ok(await statService.GetStatisticsAsync());
    }
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await userService.GetAllUsersAsync());
    }
    [HttpGet("user")]
    public async Task<IActionResult> GetUsers(int id)
    {
        return Ok(await userService.GetUserAsync(id));
    }
    [HttpPut("user-role")]
    public async Task<IActionResult> ChangeUserRole(int userId, UserRole userRole)
    {
        if(await userService.UpdateRole(userId, userRole))
        {
            return Ok();
        }
        return BadRequest(); 
    }
}