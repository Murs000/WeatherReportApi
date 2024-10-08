namespace WeatherReport.Business.DTOs;

public class ReportDTO
{
    public int Id { get; set; }
    public string DayOfWeek { get; set; }
    public string PartOfDay { get; set; }
    public IEnumerable<WeatherDetailDTO> WeatherDetails { get; set; }
    public int ForecastId { get; set; }
}