using WeatherReport.Business.DTOs;

namespace WeatherReport.Business.Services.Interfaces;

public interface IForecastService
{
    Task<IEnumerable<ForecastDTO>> GetAllAsync();
    Task<ForecastDTO> GetByIdAsync(int id);
    Task<ForecastDTO> AddAsync(ForecastDTO forecastDTO);
    Task<ForecastDTO> UpdateAsync(ForecastDTO forecastDTO);
    Task<bool> DeleteAsync(int id);
}
