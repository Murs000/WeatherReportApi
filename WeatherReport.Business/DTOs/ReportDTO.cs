namespace WeatherReport.Business.DTOs;

public class ReportDTO
{
    public int Id { get; set; }
    public string DayOfWeek { get; set; }
    public string PartOfDay { get; set; }
    public IEnumerable<ForecastDTO> Forecasts { get; set; }
    
}