using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace WeatherReport.Business.DTOs;

public class LoginDTO
{
    [SwaggerSchema("The username of the user.", Nullable = false)]
    public string Username { get; set; }

    [SwaggerSchema("The password for the user account.", Nullable = false)]
    public string Password { get; set; }
}

public class LoginDTOExample : IExamplesProvider<LoginDTO>
{
    public LoginDTO GetExamples()
    {
        return new LoginDTO
        {
            Username = "john_doe",
            Password = "Password123!"
        };
    }
}