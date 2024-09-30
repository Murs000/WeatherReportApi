using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace WeatherReport.Business.DTOs;

public class RefreshTokenDTO
{
    [SwaggerSchema("The username of the user.", Nullable = false)]
    public string Username { get; set; }

    [SwaggerSchema("The refresh token issued to the user.", Nullable = false)]
    public string RefreshToken { get; set; }
}

public class RefreshTokenDTOExample : IExamplesProvider<RefreshTokenDTO>
{
    public RefreshTokenDTO GetExamples()
    {
        return new RefreshTokenDTO
        {
            Username = "john_doe",
            RefreshToken = "refresh_token_example"
        };
    }
}