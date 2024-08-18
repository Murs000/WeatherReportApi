namespace WeatherReport.Business.Settings;

public class EmailSettings
{
    public string FromEmail { get; set; }
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }
}