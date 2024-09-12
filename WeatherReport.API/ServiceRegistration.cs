using WeatherReport.Business.Settings;

namespace WeatherReport.API;

public static class ServiceRegistration
{
    public static void AddAppSettings(this IServiceCollection services,  IConfigurationManager configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<QuartzSettings>(configuration.GetSection("QuartzSettings"));
        services.Configure<WeatherApiSettings>(configuration.GetSection("ExternalApi"));
    }
}