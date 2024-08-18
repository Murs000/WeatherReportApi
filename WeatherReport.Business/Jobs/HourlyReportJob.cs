using Quartz;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.DataAccess.Entities;

namespace WeatherReport.Business.Jobs;
public class HourlyReportJob(IJobService jobService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await jobService.SaveHourlyReportAsync();
    }
}