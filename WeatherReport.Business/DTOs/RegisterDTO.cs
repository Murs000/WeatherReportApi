using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using WeatherReport.DataAccess.Helpers;

namespace WeatherReport.Business.DTOs;

public class RegisterDTO
{
    [SwaggerSchema("The unique identifier for the user.", Nullable = false)]
    [SwaggerSchemaExample("1")]
    public int Id { get; set; }

    [SwaggerSchema("The username chosen by the user.", Nullable = false)]
    [SwaggerSchemaExample("john_doe")]
    public string Username { get; set; }

    [SwaggerSchema("The password chosen by the user.", Nullable = false)]
    [SwaggerSchemaExample("Password123!")]
    public string Password { get; set; }

    [SwaggerSchema("The first name of the user.", Nullable = false)]
    [SwaggerSchemaExample("John")]
    public string Name { get; set; }

    [SwaggerSchema("The surname of the user.", Nullable = false)]
    [SwaggerSchemaExample("Doe")]
    public string Surname { get; set; }

    [SwaggerSchema("The email address of the user.", Nullable = false)]
    [SwaggerSchemaExample("john@example.com")]
    public string Email { get; set; }

    [SwaggerSchema("The city where the user resides.", Nullable = false)]
    [SwaggerSchemaExample("New York")]
    public string CityOfResidence { get; set; }
}