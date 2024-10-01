namespace WeatherReport.DataAccess.Helpers;

[AttributeUsage(
    AttributeTargets.Class |
    AttributeTargets. Struct |
    AttributeTargets. Parameter | 
    AttributeTargets. Property |
    AttributeTargets.Enum |
    AttributeTargets.Field,
    AllowMultiple = false)]
public class SwaggerSchemaExampleAttribute : Attribute
{
    public SwaggerSchemaExampleAttribute(object example)
    {
        Example = example;
    }
    public object Example { get; set; }
}
