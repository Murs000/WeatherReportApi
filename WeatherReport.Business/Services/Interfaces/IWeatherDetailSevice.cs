using WeatherReport.Business.DTOs;

namespace WeatherReport.Business.Services.Interfaces;

public interface IWeatherDetailService
{
    Task<IEnumerable<WeatherDetailDTO>> GetAllAsync();
    Task<WeatherDetailDTO> GetByIdAsync(int id);
    Task<WeatherDetailDTO> AddAsync(WeatherDetailDTO weatherDetailDTO);
    Task<WeatherDetailDTO> UpdateAsync(WeatherDetailDTO weatherDetailDTO);
    Task<bool> DeleteAsync(int id);
}