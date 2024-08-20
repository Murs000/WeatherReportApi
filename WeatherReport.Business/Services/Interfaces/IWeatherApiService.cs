using WeatherReport.Business.DTOs;

namespace WeatherReport.Business.Services.Interfaces;

public interface IWeatherApiService
{
    public Task<ForecastDTO> GetCurrentWeatherDataAsync(string cityName);
    public Task<ForecastDTO> GetForWeekWeatherDataAsync(string cityName);
}