namespace WeatherReport.DataAccess.Entities;

public class WeatherDetail : BaseEntity
{
    public string Description { get; set; }
    public string Icon { get; set; }

    public int ReportId { get; set; }
    public Report Report { get; set; }
}