namespace WeatherReport.Business.Services.Interfaces;

public interface IJobService
{
    public Task SendDailyEmailAsync();
    public Task SendWeeklyEmailAsync();
    public Task SaveHourlyReportAsync();
}
