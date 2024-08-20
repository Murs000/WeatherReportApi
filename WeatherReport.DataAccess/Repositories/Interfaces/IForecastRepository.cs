using WeatherReport.DataAccess.Entities;

namespace WeatherReport.DataAccess.Repositories.Interfaces;

public interface IForecastRepository
{
    Task<IEnumerable<Forecast>> GetAllAsync();
    Task<Forecast> GetByIdAsync(int id);
    Task AddAsync(Forecast forecast);
    Task UpdateAsync(Forecast forecast);
    Task<bool> DeleteAsync(int id);
}