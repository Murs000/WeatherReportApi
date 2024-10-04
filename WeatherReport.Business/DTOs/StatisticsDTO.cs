using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using WeatherReport.DataAccess.Helpers;

namespace WeatherReport.Business.DTOs;

public class StatisticsDTO
{
    [SwaggerSchema("Statistics about users subscibtions.", Nullable = false)]
    public IEnumerable<SubscriberTypeStat> SubscriberTypeStats { get; set; }
    [SwaggerSchema("Statistics about users city.", Nullable = false)]
    public IEnumerable<CityStat> CityStats { get; set; }
}

public class SubscriberTypeStat
{
    [SwaggerSchema("The type of user subscibtion.", Nullable = false)]
    [SwaggerSchemaExample("Daily")]
    public string Type { get; set; }
    [SwaggerSchema("The count of this type of subscibtion.", Nullable = false)]
    [SwaggerSchemaExample(190)]
    public int Count { get; set; } 
}

public class CityStat
{
    [SwaggerSchema("Users city of residance.", Nullable = false)]
    [SwaggerSchemaExample("NewPassword123!")]
    public string City { get; set; }
    [SwaggerSchema("Count of this city among subscribers.", Nullable = false)]
    [SwaggerSchemaExample("NewPassword123!")]
    public int SubscriberCount { get; set; }
}
public class StatisticsDTOExample : IExamplesProvider<StatisticsDTO>
{
    public StatisticsDTO GetExamples()
    {
        return new StatisticsDTO
        {
            SubscriberTypeStats = new List<SubscriberTypeStat>
            {
                new SubscriberTypeStat { Type = "Daily", Count = 150 },
                new SubscriberTypeStat { Type = "Weekly", Count = 320 }
            },
            CityStats = new List<CityStat>
            {
                new CityStat { City = "New York", SubscriberCount = 190 },
                new CityStat { City = "San Francisco", SubscriberCount = 280 }
            }
        };
    }
}