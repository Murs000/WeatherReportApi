using WeatherReport.API.Middlewares;
using WeatherReport.Business;
using WeatherReport.DataAccess;
using WeatherReport.API;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.Business.Services.Implementations;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Reflection;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:AccessKey"]))
        };
    });

builder.Services.AddSwaggerGen(c=>
{
    // Generate Swagger documents for different API versions
    var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in provider.ApiVersionDescriptions)
    {
        c.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = $"Weather API {description.ApiVersion}",
            Version = description.ApiVersion.ToString(),
            Description = $"A simple Weather API - Version {description.ApiVersion}"
        });
    }

    c.EnableAnnotations();
    c.ExampleFilters(); // Enable example filters

    // Optional: Include XML comments if you have them for better Swagger documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddSwaggerExamples();

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(2, 0); // default version: v1.0
    options.ReportApiVersions = true; // shows available versions in the response headers
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // This formats the version as "v1", "v2", etc.
    options.SubstituteApiVersionInUrl = true;  // Replace version in URL if needed
});

builder.Services.AddAppSettings(builder.Configuration);

builder.Services.AddAppMappers();

builder.Services.AddAppDB(builder.Configuration);

builder.Services.AddHttpClient<IWeatherApiService, WeatherApiService>();

builder.Services.AddQuartzJobs();

builder.Services.AddAppRepositories();

builder.Services.AddAppServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        options.SwaggerEndpoint($"/swagger/v2/swagger.json", "Weather API V2");

        // Add other versions if needed
        foreach (var description in provider.ApiVersionDescriptions)
        {
            if (description.GroupName != "v2") // Ensure we don't add v2 again
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Weather API {description.GroupName.ToUpperInvariant()}");
            }
        }
    });

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<WeatherReportDb>();
    db.Database.EnsureCreated();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();