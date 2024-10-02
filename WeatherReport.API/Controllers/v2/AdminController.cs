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
public class AdminController(IStatisticsService statService, IUserService userService) : ControllerBase
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
    [HttpPut("user-role")]
    public async Task<IActionResult> ChangeUserRole()
    {
        return Ok();
    }
}