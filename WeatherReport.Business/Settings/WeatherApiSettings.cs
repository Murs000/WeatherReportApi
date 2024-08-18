namespace WeatherReport.Business.Settings;

public class WeatherApiSettings
{
    public string WeatherApiBaseUrl { get; set; }
    public string ApiModeCurrent { get; set; }
    public string ApiModeForWeek { get; set; }
    public string ApiKey { get; set; }
    public string Units { get; set; }
}
