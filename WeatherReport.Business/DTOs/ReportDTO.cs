namespace WeatherReport.Business.DTOs;

public class ReportDTO
{
    public int Id { get; set; }
    public IEnumerable<string> Descriptions { get; set; }
    public IEnumerable<string> Icons { get; set; }
}