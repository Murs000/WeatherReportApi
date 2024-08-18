using WeatherReport.DataAccess.Entities;

namespace WeatherReport.DataAccess.Repositories.Interfaces;

public interface IReportRepository
{
    Task<IEnumerable<Report>> GetAllAsync();
    Task<Report> GetByIdAsync(int id);
    Task AddAsync(Report report);
    Task UpdateAsync(Report report);
    Task<bool> DeleteAsync(int id);
}