namespace WeatherReport.Business.DTOs;

public class ForecastDTO
{
    public int Id { get; set; }
    public IEnumerable<ReportDTO> Reports { get; set; }
    public int SubscriberId { get; set; }
}