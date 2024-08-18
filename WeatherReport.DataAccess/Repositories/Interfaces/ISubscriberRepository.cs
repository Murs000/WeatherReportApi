using WeatherReport.DataAccess.Entities;

namespace WeatherReport.DataAccess.Repositories.Interfaces;
public interface ISubscriberRepository
{
    Task<Subscriber> GetByIdAsync(int id);
    Task<IEnumerable<Subscriber>> GetAllAsync();
    Task AddAsync(Subscriber subscriber);
    Task UpdateAsync(Subscriber subscriber);
    Task DeleteAsync(int id);
}
