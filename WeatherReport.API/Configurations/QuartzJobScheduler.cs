using Microsoft.Extensions.Options;
using Quartz;
using WeatherReport.Business.Jobs;
using WeatherReport.Business.Settings;

namespace WeatherReport.API.Configurations;

public class QuartzJobScheduler
{
    public static void ConfigureJobs(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scheduler = serviceProvider.GetRequiredService<ISchedulerFactory>().GetScheduler().Result;
        var quartzSettings = serviceProvider.GetRequiredService<IOptions<QuartzSettings>>().Value;

        // Schedule DailyEmailJob using the schedule from appsettings.json
        var dailyEmailJob = JobBuilder.Create<DailyEmailJob>()
            .WithIdentity("DailyEmailJob")
            .Build();

        var dailyEmailTrigger = TriggerBuilder.Create()
            .WithIdentity("DailyEmailTrigger")
            .StartNow()
            .WithCronSchedule(quartzSettings.DailyEmailJobSchedule)
            .Build();

        scheduler.ScheduleJob(dailyEmailJob, dailyEmailTrigger).Wait();

        // Schedule HourlyReportJob using the schedule from appsettings.json
        var hourlyReportJob = JobBuilder.Create<HourlyReportJob>()
            .WithIdentity("HourlyReportJob")
            .Build();

        var hourlyReportTrigger = TriggerBuilder.Create()
            .WithIdentity("HourlyReportTrigger")
            .StartNow()
            .WithCronSchedule(quartzSettings.HourlyReportJobSchedule)
            .Build();
        
        scheduler.ScheduleJob(hourlyReportJob, hourlyReportTrigger).Wait();

        // Schedule WeeklyEmailJob using the schedule from appsettings.json
        var weeklyEmailJob = JobBuilder.Create<WeeklyEmailJob>()
            .WithIdentity("WeeklyEmailJob")
            .Build();

        var weeklyEmailTrigger = TriggerBuilder.Create()
            .WithIdentity("WeeklyEmailTrigger")
            .StartNow()
            .WithCronSchedule(quartzSettings.WeeklyEmailJobSchedule)
            .Build();

        scheduler.ScheduleJob(weeklyEmailJob, weeklyEmailTrigger).Wait();
    }
}