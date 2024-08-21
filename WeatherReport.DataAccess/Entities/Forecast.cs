namespace WeatherReport.DataAccess.Entities;

public class Forecast : BaseEntity
{
    public List<Report> Reports { get; set; }

    public int SubscriberId { get; set; }
    public Subscriber Subscriber { get; set; }
}