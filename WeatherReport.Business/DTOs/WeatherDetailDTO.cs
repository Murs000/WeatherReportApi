namespace WeatherReport.Business.DTOs;

public class WeatherDetailDTO
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public int ReportId { get; set; }
}