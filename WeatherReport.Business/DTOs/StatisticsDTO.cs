public class StatisticsDTO
{
    public IEnumerable<SubscriberTypeStat> SubscriberTypeStats { get; set; }
    public IEnumerable<CityStat> CityStats { get; set; }
}

public class SubscriberTypeStat
{
    public string Type { get; set; }
    public int Count { get; set; } 
}

public class CityStat
{
    public string City { get; set; }
    public int SubscriberCount { get; set; }
}