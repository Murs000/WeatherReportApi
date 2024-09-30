using WeatherReport.DataAccess.Helpers;

namespace WeatherReport.DataAccess.Enums;

public enum SubscriptionType
{
    // Gaps between for possibly adding types
    [SwaggerSchemaExample("No subscription")]
    None = 0,
    [SwaggerSchemaExample("Daily subscription")]
    Daily = 10,
    [SwaggerSchemaExample("Weekly subscription")]
    Weekly = 20,
}