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

    foreach (var subscriber in subscribers)
    {
        var report = await weatherApiService.GetCurrentWeatherDataAsync(subscriber.CityOfResidence);

        // Build the HTML body
        var emailBody = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Daily Weather Report</title>
            <style>
                body {{
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                    background-color: #f4f4f9;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    max-width: 600px;
                    margin: 20px auto;
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
                .weather-card {{
                    display: flex;
                    align-items: center;
                    margin-bottom: 15px;
                }}
                .weather-icon {{
                    width: 50px;
                    height: 50px;
                    margin-right: 15px;
                    vertical-align: middle;
                }}
                .weather-text {{
                    font-size: 16px;
                    color: #333;
                    line-height: 1.6;
                }}
                .footer {{
                    background-color: #ff6f00; /* Orange background */
                    color: white;
                    padding: 15px;
                    text-align: center;
                    font-size: 14px;
                    border-top: 1px solid #ff6f00;
                }}
                .footer p {{
                    margin: 0;
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
                    <h1>Daily Weather Report</h1>
                    <p>For {subscriber.Name} {subscriber.Surname}</p>
                </div>
                <div class='report'>";

        var descriptions = report.Descriptions.ToList();
        var icons = report.Icons.ToList();

        for (int i = 0; i < descriptions.Count; i++)
        {
            string weatherIcon = i < icons.Count ? $"<img class='weather-icon' src='http://openweathermap.org/img/wn/{icons[i]}.png' alt='Weather Icon'/>" : string.Empty;

            emailBody += $@"
            <div class='weather-card'>
                {weatherIcon}
                <div class='weather-text'>{descriptions[i]}</div>
            </div>";
        }

        emailBody += @"
                </div>
                <div class='footer'>
                    <p>Have a fantastic day ahead! 🌟</p>
                    <p>Stay prepared! Check the latest forecasts anytime.</p>
                    <p>Visit our <a href='https://weatherwebsite.com'>website</a> for more details.</p>
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
        foreach (var weeklyReport in weeklyReports)
        {
            if (dayOfWeek != weeklyReport.DayOfWeek)
            {
                dayOfWeek = weeklyReport.DayOfWeek;
                emailBody += $"<div class='report-day'>{weeklyReport.DayOfWeek}</div>";
            }
            emailBody += $"<div class='report-part'>{weeklyReport.PartOfDay}</div>";

            emailBody += "<div class='report-description'>";
            var descriptions = weeklyReport.Descriptions.ToList();
            var icons = weeklyReport.Icons.ToList();

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
        var report = await weatherApiService.GetCurrentWeatherDataAsync("Baku");

    
            await reportService.CreateReportAsync(report);
        
    }
}