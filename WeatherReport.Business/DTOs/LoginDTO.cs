using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using WeatherReport.Business.Helpers;

namespace WeatherReport.Business.DTOs;

public class LoginDTO
{
    [SwaggerSchema("The username of the user.", Nullable = false)]
    [SwaggerSchemaExample("john_doe")]
    public string Username { get; set; }

    [SwaggerSchema("The password for the user account.", Nullable = false)]
    [SwaggerSchemaExample("Password123!")]
    public string Password { get; set; }
}