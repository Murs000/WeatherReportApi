namespace WeatherReport.Business.DTOs;

public class WeatherResponse
{
    public Weather[] Weather { get; set; }
    public Main Main { get; set; }
    public Wind Wind { get; set; }
    public Clouds Clouds { get; set; }
}
public class Weather
{
    public int Id { get; set; }
    public string Main { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
}
public class Main
{
    public float Temp { get; set; }
    public float FeelsLike { get; set; }
    public float TempMin { get; set; }
    public float TempMax { get; set; }
    public float Pressure { get; set; }
    public float Humidity { get; set; }
    public float? SeaLevel { get; set; }
    public float? GrndLevel { get; set; }
}
public class Wind
{
    public float Speed { get; set; }
    public float Degree { get; set; }
    public float? Gust { get; set; }
}
public class Clouds
{
    public float All { get; set; }
}