using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.Business.Services.Implementations;

public class JobService(IEmailService emailService,
                        IWeatherApiService weatherApiService,
                        ISubscriberService subscriberService,
                        IReportService reportService) : IJobService
{
    public async Task SendDailyEmailAsync()
    {
        var subscribers = await subscriberService.GetBySubscriptionTypeAsync(SubscriptionType.Daily);
        
        // TODO: WriteDb;
        // TODO: Middleware exception handling;

        foreach(var subscriber in subscribers)
        {
            var reports = await weatherApiService.GetCurrentWeatherDataAsync(subscriber.CityOfResidence);

            // Build the HTML body
        var weatherDescription = string.Join("<br/>", reports.Select(r => r.Description));
        var weatherIconUrls = reports.Select(r => $"http://openweathermap.org/img/wn/{r.Icon}.png");
        var weatherIcons = string.Join("<br/>", weatherIconUrls.Select(url => $"<img src='{url}' alt='Weather Icon' style='width: 50px; height: 50px;' />"));

        var emailBody = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Daily Weather Snapshot</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    margin: 0;
                    padding: 0;
                    background-color: #f9f9f9;
                    color: #333;
                }}
                .container {{
                    width: 90%;
                    max-width: 600px;
                    margin: 20px auto;
                    background: #fff;
                    padding: 20px;
                    border-radius: 10px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                    text-align: center;
                }}
                h1 {{
                    color: #ff5722;
                    font-size: 24px;
                    font-family: 'Roboto', sans-serif;
                }}
                .weather-card {{
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    background: #e3f2fd;
                    border-radius: 8px;
                    padding: 20px;
                    margin-top: 20px;
                    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                    text-align: center;
                }}
                .weather-icon {{
                    margin-right: 15px;
                }}
                .weather-details {{
                    text-align: center;
                    max-width: 300px;
                    margin: 0 auto;
                }}
                .weather-description {{
                    font-size: 20px;
                    font-weight: bold;
                    color: #333;
                    margin: 10px 0;
                    line-height: 1.4;
                }}
                .footer {{
                    margin-top: 20px;
                    font-size: 14px;
                    color: #777;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>Good Morning, {subscriber.Name}!</h1>
                <p>Here's your weather snapshot for today:</p>
                <div class='weather-card'>
                    <div class='weather-icon'>
                        {weatherIcons}
                    </div>
                    <div class='weather-details'>
                        <div class='weather-description'>
                            {weatherDescription}
                        </div>
                    </div>
                </div>
                <div class='footer'>
                    Have a fantastic day ahead! 🌟
                </div>
            </div>
        </body>
        </html>";


            // Define the email details
            var emailRequest = new EmailRequestDTO
            {
                ToEmail = subscriber.Email,
                Subject = $"Daily Report for {subscriber.Name} {subscriber.Surname}",
                Body = emailBody
            };

            await emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);
        }
    }
    public async Task SendWeeklyEmailAsync()
{
    var subscribers = await subscriberService.GetBySubscriptionTypeAsync(SubscriptionType.Weekly);
    
    foreach (var subscriber in subscribers)
    {
        var weeklyReports = await weatherApiService.GetForWeekWeatherDataAsync(subscriber.CityOfResidence);

        string emailBody = "<!DOCTYPE html>";
        emailBody += "<html lang='en'>";
        emailBody += "<head>";
        emailBody += "<meta charset='UTF-8'>";
        emailBody += "<meta name='viewport' content='width=device-width, initial-scale=1.0'>";
        emailBody += "<style>";
        emailBody += "body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #e0e0e0; padding: 20px; margin: 0; }";
        emailBody += ".container { max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); }";
        emailBody += ".header { background-color: #007BFF; color: white; padding: 20px; text-align: center; }";
        emailBody += ".header h1 { margin: 0; font-size: 24px; }";
        emailBody += ".report { padding: 20px; }";
        emailBody += ".report-day { font-weight: bold; font-size: 18px; margin-top: 20px; border-bottom: 2px solid #007BFF; padding-bottom: 10px; }";
        emailBody += ".report-part { margin-left: 20px; font-size: 16px; margin-top: 10px; }";
        emailBody += ".report-icon { width: 20px; height: 20px; margin-right: 10px; vertical-align: middle; }";
        emailBody += ".footer { background-color: #007BFF; color: white; padding: 10px; text-align: center; font-size: 14px; }";
        emailBody += ".footer a { color: #ffffff; text-decoration: none; font-weight: bold; }";
        emailBody += "</style>";
        emailBody += "</head>";
        emailBody += "<body>";
        emailBody += "<div class='container'>";
        emailBody += $"<div class='header'><h1>Weekly Weather Report</h1><p>For {subscriber.Name} {subscriber.Surname}</p></div>";
        emailBody += "<div class='report'>";

        string dayOfWeek = string.Empty;
        foreach (var weeklyReport in weeklyReports)
        {
            if (dayOfWeek != weeklyReport.DayOfWeek)
            {
                dayOfWeek = weeklyReport.DayOfWeek;
                emailBody += $"<div class='report-day'>{weeklyReport.DayOfWeek}</div>";
            }
            emailBody += "<div class='report-part'>";

            // Example icons for different weather conditions (replace with actual URLs or base64 data)
            string weatherIcon = "<img class='report-icon' src='https://via.placeholder.com/20' alt='Weather Icon'/>";

            emailBody += $"{weatherIcon} {weeklyReport.PartOfDay}: ";
            foreach (var description in weeklyReport.Descriptions)
            {
                emailBody += $"{description}, ";
            }
            emailBody = emailBody[..^2]; // Remove the last comma
            emailBody += "</div>";
        }

        emailBody += "</div>"; // Close report div
        emailBody += "<div class='footer'>";
        emailBody += "<p>Stay prepared! Check the latest forecasts anytime.</p>";
        emailBody += "<p>Visit our <a href='https://weatherwebsite.com'>website</a> for more details.</p>";
        emailBody += "</div>";
        emailBody += "</div>"; // Close container div
        emailBody += "</body>";
        emailBody += "</html>";

        // Define the email details
        var emailRequest = new EmailRequestDTO
        {
            ToEmail = subscriber.Email,
            Subject = $"Weekly Report for {subscriber.Name} {subscriber.Surname}",
            Body = emailBody
        };

        await emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);
    }
}
    public async Task SaveHourlyReportAsync()
    {
        // Example of how you might take a report, depending on your implementation
        var reports = await weatherApiService.GetCurrentWeatherDataAsync("Baku");

        foreach(var report in reports)
        {
            await reportService.CreateReportAsync(report);
        }
    }
}