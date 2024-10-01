using Swashbuckle.AspNetCore.Annotations;
using WeatherReport.DataAccess.Enums;
using WeatherReport.DataAccess.Helpers;

namespace WeatherReport.Business.DTOs;

public class UserDTO
{
    [SwaggerSchema("The name of the user.", Nullable = false)]
    [SwaggerSchemaExample("John")]
    public string Name { get; set; }

    [SwaggerSchema("The surname of the user.", Nullable = false)]
    [SwaggerSchemaExample("Doe")]
    public string Surname { get; set; }

    [SwaggerSchema("The city of the user.", Nullable = false)]
    [SwaggerSchemaExample("New York")]
    public string CityOfResidence { get; set; }

    [SwaggerSchema("The subscrition of the user.", Nullable = false)]
    [SwaggerSchemaExample(10)]
    public SubscriptionType SubscriptionType { get; set; } 
}