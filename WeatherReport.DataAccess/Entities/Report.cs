namespace WeatherReport.DataAccess.Entities;

public class Report : BaseEntity
{
    public string DayOfWeek { get; set; }
    public string PartOfDay { get; set; }
    public List<Forecast> Forecasts { get; set; }
    
    public int SubscriberId { get; set; }
    public Subscriber Subscriber { get; set; }
}
