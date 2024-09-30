using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using WeatherReport.DataAccess.Helpers;

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
            var exampleText = exampleAttr != null ? exampleAttr.Example : name;

            // Create the custom example format: e.g., "0 (None): No subscription"
            var formattedExample = $"{value} ({name}): {exampleText}";

            // Add this formatted example to the schema's enum list
            schema.Enum.Add(new Microsoft.OpenApi.Any.OpenApiString(formattedExample));
        }
    }

    private void ApplySchemaAttribute(OpenApiSchema schema, SwaggerSchemaExampleAttribute schemaAttribute)
    {
        if (!string.IsNullOrEmpty(schemaAttribute.Example))
        {
            schema.Example = new Microsoft.OpenApi.Any.OpenApiString(schemaAttribute.Example);
        }
    }
}