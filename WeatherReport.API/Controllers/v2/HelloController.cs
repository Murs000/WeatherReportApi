using Microsoft.AspNetCore.Mvc;

namespace WeatherReport.API.Controllers.v2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class HelloController : ControllerBase
{
    /// <summary>
    /// Returns a hello message.
    /// </summary>
    /// <returns>A string message "Hello from WeatherReport API v2!"</returns>
    [HttpGet("hello")]
    public IActionResult GetHello()
    {
        return Ok("Hello from WeatherReport API v2!");
    }
}