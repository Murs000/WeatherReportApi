namespace WeatherReport.Business.Settings;

public class QuartzSettings
{
    public string DailyEmailJobSchedule { get; set; }
    public string WeeklyEmailJobSchedule { get; set; }
    public string HourlyReportJobSchedule { get; set; }
}