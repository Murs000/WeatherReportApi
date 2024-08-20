using System.Globalization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.Business.Settings;
using WeatherReport.DataAccess.Entities;

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

    public async Task<ForecastDTO> GetCurrentWeatherDataAsync(string cityName)
    {
        var requestUri = $"{_apiSettings.WeatherApiBaseUrl}{_apiSettings.ApiModeCurrent}?q={cityName}&appid={_apiSettings.ApiKey}&units={_apiSettings.Units}";
        var response = await _httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(content);

        var weatherDetails = new List<WeatherDetailDTO>();
        foreach(var weather in weatherResponse.Weather)
        {
            var weatherDetail = new WeatherDetailDTO
            {
                Description = weather.Description,
                Icon = weather.Icon
            };
            weatherDetails.Add(weatherDetail);
        }
        var report = new ReportDTO
        {
            DayOfWeek = await GetDayOfWeek(DateTime.Now),
            PartOfDay = await GetPartOfDay(DateTime.Now),
            WeatherDetails = weatherDetails
        };
        return new ForecastDTO
        {
            Reports = [report]
        };
    }
    public async Task<ForecastDTO> GetForWeekWeatherDataAsync(string cityName)
    {
        var requestUri = $"{_apiSettings.WeatherApiBaseUrl}{_apiSettings.ApiModeForWeek}?q={cityName}&appid={_apiSettings.ApiKey}&units={_apiSettings.Units}";
        var response = await _httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var weatherResponse = JsonConvert.DeserializeObject<WeatherForecastResponse>(content);

        var reports = new List<ReportDTO>();

        foreach(var weathers in weatherResponse.List)
        {
            var weatherDetails = new List<WeatherDetailDTO>();
            foreach(var weather in weathers.Weather)
            {
                var weatherDetail = new WeatherDetailDTO
                {
                    Description = weather.Description,
                    Icon = weather.Icon
                };
                weatherDetails.Add(weatherDetail);
            }
            var report = new ReportDTO
            {
                DayOfWeek = await GetDayOfWeek(DateTimeOffset.FromUnixTimeSeconds(weathers.Dt).DateTime),
                PartOfDay = await GetPartOfDay(DateTimeOffset.FromUnixTimeSeconds(weathers.Dt).DateTime),
                WeatherDetails = weatherDetails
            };
            reports.Add(report);
        }

        return new ForecastDTO
        {
            Reports = reports
        };
    }
    private async Task<string> GetPartOfDay(DateTime dateTime)
    {
        var morningStart = new TimeSpan(0, 0, 0);  // Midnight
        var afternoonStart = new TimeSpan(12, 0, 0); // Noon
        var eveningStart = new TimeSpan(18, 0, 0);  // 6 PM

        var timeOfDay = dateTime.TimeOfDay;
                
        var partOfDay = timeOfDay >= morningStart && timeOfDay < afternoonStart ? "Morning" :
                        timeOfDay >= afternoonStart && timeOfDay < eveningStart ? "Afternoon" :
                        "Evening";

        return partOfDay;
    }
    private async Task<string> GetDayOfWeek(DateTime dateTime)
    {
        var dayOfWeek = dateTime.Date.ToString("dddd", CultureInfo.InvariantCulture);

        return dayOfWeek;
    }
}