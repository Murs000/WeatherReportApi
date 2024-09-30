using WeatherReport.DataAccess.Enums;
using WeatherReport.DataAccess.Helpers;

namespace WeatherReport.Business.DTOs;

public class UserDTO
{
    [SwaggerSchemaExample("John")]
    public string Name { get; set; }

    [SwaggerSchemaExample("Doe")]
    public string Surname { get; set; }

    [SwaggerSchemaExample("New York")]
    public string CityOfResidence { get; set; }

    [SwaggerSchemaExample("Daily")]
    public SubscriptionType SubscriptionType { get; set; } 
}