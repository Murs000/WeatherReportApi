using System.Diagnostics;
using System.Runtime.CompilerServices;
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
    var subscribers = await subscriberService.GetAllAsync(SubscriptionType.Daily);

    foreach (var subscriber in subscribers)
    {
        var report = await weatherApiService.GetCurrentWeatherDataAsync(subscriber.CityOfResidence);

        var emailBody = await SetBody(report,subscriber);

        // Define the email details
        var emailRequest = new EmailRequestDTO
        {
            ToEmail = subscriber.Email,
            Subject = $"Daily Report for {subscriber.Name} {subscriber.Surname}",
            Body = emailBody
        };

        await emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);

        report.SubscriberId = subscriber.Id;
        await reportService.CreateReportAsync(report);
    }
}
    private async Task<string> SetBody(ReportDTO report,SubscriberDTO subscriber)
    {
        // Build the HTML body
        var weatherDescription = string.Join("<br/>", report.Forecasts.Select(f=> f.Description));
        var weatherIconUrls = report.Forecasts.Select(r => $"http://openweathermap.org/img/wn/{r.Icon}.png");
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
                <h1>Good {report.PartOfDay}, {subscriber.Name}!</h1>
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
        return emailBody;
    }
    public async Task SendWeeklyEmailAsync()
{
    var subscribers = await subscriberService.GetAllAsync(SubscriptionType.Weekly);

    foreach (var subscriber in subscribers)
    {
        var reports = await weatherApiService.GetForWeekWeatherDataAsync(subscriber.CityOfResidence);
        
        var emailBody = await SetBody(reports,subscriber);

        // Define the email details
        var emailRequest = new EmailRequestDTO
        {
            ToEmail = subscriber.Email,
            Subject = $"Weekly Report for {subscriber.Name} {subscriber.Surname}",
            Body = emailBody
        };

        await emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);

        foreach(var report in reports)
        {
            report.SubscriberId = subscriber.Id;
            await reportService.CreateReportAsync(report);
        }
    }
}
    private async Task<string> SetBody(IEnumerable<ReportDTO> reports, SubscriberDTO subscriber)
    {
        // TODO : Correct HTML Repeating
        // Build the HTML body
        var emailBody = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Weekly Weather Report</title>
            <style>
                body {{
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                    background-color: #f4f4f9;
                    margin: 0;
                    padding: 20px;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    border-radius: 10px;
                    overflow: hidden;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    background-color: #ff6f00; /* Orange background */
                    color: white;
                    padding: 20px;
                    text-align: center;
                }}
                .header h1 {{
                    margin: 0;
                    font-size: 24px;
                }}
                .header p {{
                    margin: 5px 0 0;
                    font-size: 16px;
                }}
                .report {{
                    padding: 20px;
                    background-color: #f0f0f0; /* Light grey background */
                }}
                .report-day {{
                    font-weight: bold;
                    font-size: 20px;
                    color: #ff6f00; /* Orange text */
                    margin-top: 20px;
                    border-bottom: 2px solid #ff6f00;
                    padding-bottom: 10px;
                }}
                .report-part {{
                    font-weight: bold;
                    font-size: 18px;
                    margin-top: 10px;
                }}
                .report-description {{
                    background-color: #e0e0e0; /* Grey background for description */
                    padding: 10px;
                    border-radius: 5px;
                    margin-top: 10px;
                    font-size: 16px;
                }}
                .report-icon {{
                    width: 24px;
                    height: 24px;
                    margin-right: 8px;
                    vertical-align: middle;
                }}
                .footer {{
                    background-color: #ff6f00; /* Orange background */
                    color: white;
                    padding: 10px;
                    text-align: center;
                    font-size: 14px;
                }}
                .footer a {{
                    color: #ffffff;
                    text-decoration: none;
                    font-weight: bold;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Weekly Weather Report</h1>
                    <p>For {subscriber.Name} {subscriber.Surname}</p>
                </div>
                <div class='report'>";

        string dayOfWeek = string.Empty;
        string partOfDay = string.Empty;
        foreach (var report in reports)
        {
            if (dayOfWeek != report.DayOfWeek)
            {
                dayOfWeek = report.DayOfWeek;
                emailBody += $"<div class='report-day'>{report.DayOfWeek}</div>";
            }
            if (partOfDay != report.PartOfDay)
            {
                partOfDay = report.PartOfDay;
                emailBody += $"<div class='report-part'>{report.PartOfDay}</div>";
            }
            emailBody += "<div class='report-description'>";
            var descriptions = report.Forecasts.Select(f=>f.Description).ToList();
            var icons = report.Forecasts.Select(f=>f.Icon).ToList();

            for (int i = 0; i < descriptions.Count; i++)
            {
                // Ensure that there are icons available and align with descriptions
                string weatherIcon = i < icons.Count ? $"<img class='report-icon' src='http://openweathermap.org/img/wn/{icons[i]}.png' alt='Weather Icon'/>" : string.Empty;

                emailBody += $"{weatherIcon} {descriptions[i]}<br/>";
            }
            emailBody += "</div>";
        }

        emailBody += @"
                </div>
                <div class='footer'>
                    <p>Stay prepared! Check the latest forecasts anytime.</p>
                    <p>Visit our <a href='https://weatherwebsite.com'>website</a> for more details.</p>
                </div>
            </div>
        </body>
        </html>";
        return emailBody;
    }
    public async Task SaveHourlyReportAsync()
    {
        // Example of how you might take a report, depending on your implementation
        var report = await weatherApiService.GetCurrentWeatherDataAsync("Baku");

    
            await reportService.CreateReportAsync(report);
        
    }
}