using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WeatherReport.DataAccess.Helpers;
using System.Reflection;

public class SwaggerSchemaExampleFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // Handle enum types with custom examples
        if (context.Type.IsEnum)
        {
            ApplyEnumSchemaExamples(schema, context.Type);
        }
        else if (context.MemberInfo != null)
        {
            // Handle normal properties or classes
            var schemaAttribute = context.MemberInfo.GetCustomAttributes<SwaggerSchemaExampleAttribute>().FirstOrDefault();
            if (schemaAttribute != null)
            {
                ApplySchemaAttribute(schema, schemaAttribute);
            }
        }
    }

    private void ApplyEnumSchemaExamples(OpenApiSchema schema, Type enumType)
    {
        schema.Enum.Clear(); // Clear the default enum representation

        foreach (var field in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var value = (int)field.GetValue(null); // Get the numeric value of the enum
            var name = field.Name; // Get the name of the enum member

            // Get the SwaggerSchemaExampleAttribute, if applied
            var exampleAttr = field.GetCustomAttribute<SwaggerSchemaExampleAttribute>();
            var exampleText = exampleAttr != null ? exampleAttr.Example.ToString() : name;

            // Create the custom description format: e.g., "0 (None): No subscription"
            var formattedDescription = $"{value} ({name}): {exampleText}";

            // Add this formatted description to the schema's description
            if (string.IsNullOrEmpty(schema.Description))
            {
                schema.Description = formattedDescription;
            }
            else
            {
                schema.Description += $", {formattedDescription}";
            }

            // Keep the original enum value for Swagger UI, not the custom example
            schema.Enum.Add(new OpenApiString($"{value}"));
        }
    }

    private void ApplySchemaAttribute(OpenApiSchema schema, SwaggerSchemaExampleAttribute schemaAttribute)
    {
        if (schemaAttribute.Example != null)
        {
            var exampleValue = schemaAttribute.Example;

            // Handle different types of examples
            schema.Example = exampleValue switch
            {
                string strValue => new OpenApiString(strValue),
                int intValue => new OpenApiInteger(intValue),
                double doubleValue => new OpenApiDouble(doubleValue),
                bool boolValue => new OpenApiBoolean(boolValue),
                _ => new OpenApiString(exampleValue.ToString()),// Fallback to string representation for unsupported types
            };
        }
    }
}