using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using WeatherReport.Business.Helpers;

namespace WeatherReport.Business.DTOs;
public class ResetPasswordDTO
{
    [SwaggerSchema("The username of the user.", Nullable = false)]
    [SwaggerSchemaExample("john_doe")]
    public string Username { get; set; }

    [SwaggerSchema("The new password chosen by the user.", Nullable = false)]
    [SwaggerSchemaExample("NewPassword123!")]
    public string Password { get; set; }
}