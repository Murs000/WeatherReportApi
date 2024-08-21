using System.Diagnostics;
using System.Runtime.CompilerServices;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.Business.Services.Implementations;

public class JobService(IEmailService emailService,
                        IWeatherApiService weatherApiService,
                        IServiceUnitOfWork service) : IJobService
{
    public async Task SendDailyEmailAsync()
{
    var subscribers = await service.SubscriberService.GetAllAsync(SubscriptionType.Daily);

    foreach (var subscriber in subscribers)
    {
        var forecast = await weatherApiService.GetCurrentWeatherDataAsync(subscriber.CityOfResidence);

        var emailBody = await SetBody(forecast.Reports.First(),subscriber);

        // Define the email details
        var emailRequest = new EmailRequestDTO
        {
            ToEmail = subscriber.Email,
            Subject = $"Daily Report for {subscriber.Name} {subscriber.Surname}",
            Body = emailBody
        };

        await emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);

        forecast.SubscriberId = subscriber.Id;
        await service.ForecastService.AddAsync(forecast);
    }
}
    private async Task<string> SetBody(ReportDTO report, SubscriberDTO subscriber)
{
    // Build the HTML body
    var weatherDescription = string.Join("<br/>", report.WeatherDetails.Select(f => f.Description));
    var weatherIconUrls = report.WeatherDetails.Select(r => $"http://openweathermap.org/img/wn/{r.Icon}.png");
    var weatherItems = string.Join("", report.WeatherDetails.Select((details, index) =>
    {
        var iconUrl = $"http://openweathermap.org/img/wn/{details.Icon}.png";
        return $@"
            <div class='weather-item'>
                <img src='{iconUrl}' alt='Weather Icon' class='weather-icon' />
                <div class='weather-description'>{details.Description}</div>
            </div>";
    }));

    var emailBody = $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Daily Weather Snapshot</title>
        <style>
            body {{
                font-family: 'Helvetica Neue', Arial, sans-serif;
                margin: 0;
                padding: 0;
                background-color: #f5f5f5;
                color: #333;
            }}
            .container {{
                width: 90%;
                max-width: 650px;
                margin: 30px auto;
                background: #ffffff;
                padding: 25px;
                border-radius: 12px;
                box-shadow: 0 6px 12px rgba(0, 0, 0, 0.1);
                text-align: center;
            }}
            h1 {{
                color: #ff5722;
                font-size: 28px;
                font-family: 'Roboto', sans-serif;
                margin-bottom: 15px;
            }}
            .weather-card {{
                background: #e1f5fe;
                border-radius: 10px;
                padding: 15px;
                margin-top: 20px;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                text-align: left;
                display: flex;
                flex-direction: column;
                align-items: center;
                justify-content: center;
            }}
            .weather-item {{
                display: flex;
                align-items: center;
                padding: 10px;
                margin-bottom: 10px;
            }}
            .weather-icon {{
                width: 50px;
                height: 50px;
                margin-right: 15px;
            }}
            .weather-description {{
                font-size: 18px;
                color: #333;
            }}
            .footer {{
                margin-top: 25px;
                font-size: 14px;
                color: #888;
                text-align: center;
            }}
            .footer a {{
                color: #ff5722;
                text-decoration: none;
                font-weight: bold;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <h1>Good {report.PartOfDay}, {subscriber.Name}!</h1>
            <p>Here's your weather snapshot for today:</p>
            <div class='weather-card'>
                {weatherItems}
            </div>
            <div class='footer'>
                Have a fantastic day ahead! 🌟 <br/>
                <a href='https://weatherwebsite.com'>Visit our website</a> for more details.
            </div>
        </div>
    </body>
    </html>";

    return emailBody;
}
    public async Task SendWeeklyEmailAsync()
{
    var subscribers = await service.SubscriberService.GetAllAsync(SubscriptionType.Weekly);

    foreach (var subscriber in subscribers)
    {
        var forecast = await weatherApiService.GetForWeekWeatherDataAsync(subscriber.CityOfResidence);
        
        var emailBody = await SetBody(forecast.Reports,subscriber);

        // Define the email details
        var emailRequest = new EmailRequestDTO
        {
            ToEmail = subscriber.Email,
            Subject = $"Weekly Report for {subscriber.Name} {subscriber.Surname}",
            Body = emailBody
        };

        await emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);

        forecast.SubscriberId = subscriber.Id;
        await service.ForecastService.AddAsync(forecast);
    }
}
    private async Task<string> SetBody(IEnumerable<ReportDTO> reports, SubscriberDTO subscriber)
{
    // Group reports by DayOfWeek and PartOfDay
    var groupedReports = reports
        .GroupBy(report => new { report.DayOfWeek, report.PartOfDay })
        .Select(group => new
        {
            DayOfWeek = group.Key.DayOfWeek,
            PartOfDay = group.Key.PartOfDay,
            // Ensure that weather descriptions are unique, but retain those with different icons
            WeatherDetails = group.SelectMany(g => g.WeatherDetails)
                                  .GroupBy(detail => new { detail.Description, detail.Icon }) // Group by both description and icon
                                  .Select(g => g.First()) // Take the first unique pair
                                  .ToList()
        })
        .ToList();

    // Begin building the email body
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
                color: #333;
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
                background-color: #ff6f00;
                color: white;
                padding: 20px;
                text-align: center;
            }}
            .header h1 {{
                margin: 0;
                font-size: 26px;
            }}
            .header p {{
                margin: 8px 0 0;
                font-size: 16px;
            }}
            .report {{
                padding: 20px;
            }}
            .report-day {{
                font-weight: bold;
                font-size: 22px;
                color: #ff6f00;
                margin-top: 20px;
                border-bottom: 2px solid #ff6f00;
                padding-bottom: 5px;
            }}
            .report-part {{
                font-weight: bold;
                font-size: 18px;
                margin-top: 10px;
                color: #333;
            }}
            .report-description {{
                background-color: #f9f9f9;
                padding: 15px;
                border-radius: 5px;
                margin-top: 10px;
                font-size: 16px;
                border: 1px solid #ddd;
                box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.05);
            }}
            .report-icon {{
                width: 30px;
                height: 30px;
                margin-right: 10px;
                vertical-align: middle;
            }}
            .footer {{
                background-color: #ff6f00;
                color: white;
                padding: 15px;
                text-align: center;
                font-size: 14px;
            }}
            .footer a {{
                color: #ffffff;
                text-decoration: underline;
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

    // Variables to track the current day and part of the day
    string currentDayOfWeek = string.Empty;
    string currentPartOfDay = string.Empty;

    // Loop through the grouped reports and build the HTML content
    foreach (var group in groupedReports)
    {
        // Render the day of the week (ignore repetition)
        if (currentDayOfWeek != group.DayOfWeek)
        {
            currentDayOfWeek = group.DayOfWeek;
            emailBody += $"<div class='report-day'>{currentDayOfWeek}</div>";
        }

        // Render the part of the day (ignore repetition)
        if (currentPartOfDay != group.PartOfDay)
        {
            currentPartOfDay = group.PartOfDay;
            emailBody += $"<div class='report-part'>{currentPartOfDay}</div>";
        }

        // Render the weather details for the current part of the day
        emailBody += "<div class='report-description'>";
        foreach (var detail in group.WeatherDetails)
        {
            string weatherIcon = !string.IsNullOrEmpty(detail.Icon) 
                ? $"<img class='report-icon' src='http://openweathermap.org/img/wn/{detail.Icon}.png' alt='Weather Icon'/>" 
                : string.Empty;

            emailBody += $"{weatherIcon} {detail.Description}<br/>";
        }
        emailBody += "</div>";
    }

    // Finalize the HTML email body
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
        var subscribers = await service.SubscriberService.GetAllAsync();
        // Not take admins city
        var cities = subscribers.Where(s=>s.Id != 1).Select(s=>s.CityOfResidence).Distinct();
        foreach (var city in cities)
        {
            var forecast = await weatherApiService.GetCurrentWeatherDataAsync(city);
            forecast.SubscriberId = 1;
            await service.ForecastService.AddAsync(forecast);
        }
    }
}