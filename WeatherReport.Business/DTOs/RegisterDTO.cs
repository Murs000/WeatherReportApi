using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace WeatherReport.Business.DTOs;

public class RegisterDTO
{
    [SwaggerSchema("The unique identifier for the user.", Nullable = false)]
    public int Id { get; set; }

    [SwaggerSchema("The username chosen by the user.", Nullable = false)]
    public string Username { get; set; }

    [SwaggerSchema("The password chosen by the user.", Nullable = false)]
    public string Password { get; set; }

    [SwaggerSchema("The first name of the user.", Nullable = false)]
    public string Name { get; set; }

    [SwaggerSchema("The surname of the user.", Nullable = false)]
    public string Surname { get; set; }

    [SwaggerSchema("The email address of the user.", Nullable = false)]
    public string Email { get; set; }

    [SwaggerSchema("The city where the user resides.", Nullable = false)]
    public string CityOfResidence { get; set; }
}

public class RegisterDTOExample : IExamplesProvider<RegisterDTO>
{
    public RegisterDTO GetExamples()
    {
        return new RegisterDTO
        {
            Id = 1,
            Username = "john_doe",
            Password = "Password123!",
            Name = "John",
            Surname = "Doe",
            Email = "john@example.com",
            CityOfResidence = "New York"
        };
    }
}