using Quartz;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.Business.Jobs;

public class WeeklyEmailJob(IJobService jobService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await jobService.SendWeeklyEmailAsync();
    }
}