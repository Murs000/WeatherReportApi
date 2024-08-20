namespace WeatherReport.DataAccess.Entities;

public class Report : BaseEntity
{
    public string DayOfWeek { get; set; }
    public string PartOfDay { get; set; }
    public List<WeatherDetail> WeatherDetails { get; set; }

    public int ForecastId { get; set; }
    public Forecast Forecast { get; set; }
    
}
