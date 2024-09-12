using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherReport.DataAccess.Repositories.Implementations;
using WeatherReport.DataAccess.Repositories.Interfaces;

namespace WeatherReport.DataAccess;

public static class ServiceRegistration
{
    public static void AddAppDB(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddDbContext<WeatherReportDb>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
    public static void AddAppRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryUnitOfWork, RepositoryUnitOfWork>();
    }
}