using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace WeatherReport.Business.DTOs;
public class ResetPasswordDTO
{
    [SwaggerSchema("The username of the user.", Nullable = false)]
    public string Username { get; set; }

    [SwaggerSchema("The new password chosen by the user.", Nullable = false)]
    public string Password { get; set; }
}

public class ResetPasswordDTOExample : IExamplesProvider<ResetPasswordDTO>
{
    public ResetPasswordDTO GetExamples()
    {
        return new ResetPasswordDTO
        {
            Username = "john_doe",
            Password = "NewPassword123!"
        };
    }
}