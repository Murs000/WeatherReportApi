namespace WeatherReport.Business.DTOs;

public class WeeklyReportDTO
{
    public int Id { get; set; }
    public string DayOfWeek { get; set; }
    public string PartOfDay { get; set; }
    public IEnumerable<string> Descriptions { get; set; }
    
}