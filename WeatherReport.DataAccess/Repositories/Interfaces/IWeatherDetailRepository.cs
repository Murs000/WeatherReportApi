using WeatherReport.DataAccess.Entities;

namespace WeatherReport.DataAccess.Repositories.Interfaces;
public interface IWeatherDetailRepository
{
    Task<WeatherDetail> GetByIdAsync(int id);
    Task<IEnumerable<WeatherDetail>> GetAllAsync();
    Task AddAsync(WeatherDetail weatherDetail);
    Task UpdateAsync(WeatherDetail weatherDetail);
    Task<bool> DeleteAsync(int id);
}
