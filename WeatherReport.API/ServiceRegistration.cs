using Swashbuckle.AspNetCore.Filters;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Settings;

namespace WeatherReport.API;

public static class ServiceRegistration
{
    public static void AddAppSettings(this IServiceCollection services,  IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<QuartzSettings>(configuration.GetSection("QuartzSettings"));
        services.Configure<WeatherApiSettings>(configuration.GetSection("ExternalApi"));
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
    }
}