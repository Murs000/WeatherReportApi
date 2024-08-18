using System.Globalization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.Business.Settings;

namespace WeatherReport.Business.Services.Implementations;

public class WeatherApiService : IWeatherApiService
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiSettings _apiSettings;

    public WeatherApiService(HttpClient httpClient, IOptions<WeatherApiSettings> apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings.Value;
    }

    public async Task<IEnumerable<ReportDTO>> GetCurrentWeatherDataAsync(string cityName)
    {
        var requestUri = $"{_apiSettings.WeatherApiBaseUrl}{_apiSettings.ApiModeCurrent}?q={cityName}&appid={_apiSettings.ApiKey}&units={_apiSettings.Units}";
        var response = await _httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(content);

        List<ReportDTO> reports = [];
        foreach(var weather in weatherResponse.Weather)
        {
            var report = new ReportDTO
            {
                Description = weather.Description,
                Icon = weather.Icon
            };
            reports.Add(report);
        }

        return reports;
    }
    public async Task<IEnumerable<WeeklyReportDTO>> GetForWeekWeatherDataAsync(string cityName)
    {
        var requestUri = $"{_apiSettings.WeatherApiBaseUrl}{_apiSettings.ApiModeForWeek}?q={cityName}&appid={_apiSettings.ApiKey}&units={_apiSettings.Units}";
        var response = await _httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var weatherResponse = JsonConvert.DeserializeObject<WeatherForecastResponse>(content);

        return await TakeWeeklyWeatherAsync(weatherResponse);
    }
    private async Task<IEnumerable<WeeklyReportDTO>> TakeWeeklyWeatherAsync(WeatherForecastResponse response)
    {
        var morningStart = new TimeSpan(0, 0, 0);  // Midnight
        var afternoonStart = new TimeSpan(12, 0, 0); // Noon
        var eveningStart = new TimeSpan(18, 0, 0);  // 6 PM

        // Group the forecasts by day and time of day
        var groupedByDayAndTime = response.List
            .GroupBy(f =>
            {
                var date = DateTimeOffset.FromUnixTimeSeconds(f.Dt).DateTime;
                var timeOfDay = date.TimeOfDay;
                
                var partOfDay = timeOfDay >= morningStart && timeOfDay < afternoonStart ? "Morning" :
                                 timeOfDay >= afternoonStart && timeOfDay < eveningStart ? "Afternoon" :
                                 "Evening";

                return new { Day = date.Date.ToString("dddd", CultureInfo.InvariantCulture), PartOfDay = partOfDay };
            })
            .OrderBy(g => g.Key.Day)
            .ThenBy(g => g.Key.PartOfDay);

        var reports = new List<WeeklyReportDTO>();

        foreach (var group in groupedByDayAndTime)
        {
            var dayOfWeek = group.Key.Day;
            var partOfDay = group.Key.PartOfDay;

            var descriptions = group.SelectMany(f => f.Weather)
                .Select(w => w.Description)
                .Distinct(); // Optionally, remove duplicate descriptions

            var report = new WeeklyReportDTO
            {
                DayOfWeek = dayOfWeek,
                PartOfDay = partOfDay,
                Descriptions = descriptions
            };

            reports.Add(report);
        }

        return reports;
    }
}