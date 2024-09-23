namespace WeatherReport.Business.Settings;

public class JwtSettings
{
    public string AccessKey { get; set; }
    public int AccessExpireAt { get; set; }
    public string RefreshKey { get; set; }
    public int RefreshExpireAt { get; set; }
    public string EmailKey { get; set; }
    public int EmailExpireAt { get; set; }
}