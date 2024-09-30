using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using WeatherReport.DataAccess.Helpers;

namespace WeatherReport.Business.DTOs;

public class RefreshTokenDTO
{
    [SwaggerSchema("The username of the user.", Nullable = false)]
    [SwaggerSchemaExample("john_doe")]
    public string Username { get; set; }

    [SwaggerSchema("The refresh token issued to the user.", Nullable = false)]
    [SwaggerSchemaExample("refresh_token_example")]
    public string RefreshToken { get; set; }
}