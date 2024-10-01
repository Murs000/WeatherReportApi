using Swashbuckle.AspNetCore.Annotations;
using WeatherReport.DataAccess.Helpers;

namespace WeatherReport.Business.DTOs;
public class CurrentWeatherDTO
{
    [SwaggerSchema("Description of weather condition.", Nullable = false)]
    [SwaggerSchemaExample("scattered clouds")]
    public string Description { get; set; }
    
    [SwaggerSchema("Url of weather condition icon.", Nullable = false)]
    [SwaggerSchemaExample("http://openweathermap.org/img/wn/03d.png")]
    public string Icon { get; set; }
}