using WeatherReport.Business.DTOs;

namespace WeatherReport.Business.Services.Interfaces;

public interface IWeatherApiService
{
    public Task<ReportDTO> GetCurrentWeatherDataAsync(string cityName);
    public Task<IEnumerable<ReportDTO>> GetForWeekWeatherDataAsync(string cityName);
}