using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using WeatherReport.Business.Jobs;
using WeatherReport.Business.MappingProfiles;
using WeatherReport.Business.Services.Implementations;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.Business.Settings;
using WeatherReport.DataAccess.Repositories.Implementations;
using WeatherReport.DataAccess.Repositories.Interfaces;

namespace WeatherReport.Business;

public static class ServiceRegistration
{
    public static void AddQuartzJobs(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.UseSimpleTypeLoader();
            q.UseInMemoryStore();

            // Register jobs with schedules from appsettings.json
            var serviceProvider = services.BuildServiceProvider();
            var quartzSettings = serviceProvider.GetRequiredService<IOptions<QuartzSettings>>().Value;

            // Daily Email Job
            q.ScheduleJob<DailyEmailJob>(trigger => trigger
                .WithIdentity("DailyEmailTrigger")
                .StartNow()
                .WithCronSchedule(quartzSettings.DailyEmailJobSchedule)
            );

            // Hourly Report Job
            q.ScheduleJob<HourlyReportJob>(trigger => trigger
                .WithIdentity("HourlyReportTrigger")
                .StartNow()
                .WithCronSchedule(quartzSettings.HourlyReportJobSchedule)
            );

            // Weekly Email Job
            q.ScheduleJob<WeeklyEmailJob>(trigger => trigger
                .WithIdentity("WeeklyEmailTrigger")
                .StartNow()
                .WithCronSchedule(quartzSettings.WeeklyEmailJobSchedule)
            );
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        // Register the jobs as services
        services.AddScoped<DailyEmailJob>();
        services.AddScoped<HourlyReportJob>();
        services.AddScoped<WeeklyEmailJob>();
    }

    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();

        services.AddScoped<IServiceUnitOfWork, ServiceUnitOfWork>();

        services.AddScoped<IStatisticsService, StatisticsService>();

        services.AddScoped<IJobService, JobService>();
    }

    public static void AddAppMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(SubscriberProfile));
        services.AddAutoMapper(typeof(ReportProfile));
        services.AddAutoMapper(typeof(WeatherDetailProfile));
        services.AddAutoMapper(typeof(ForecastProfile));
    }
}