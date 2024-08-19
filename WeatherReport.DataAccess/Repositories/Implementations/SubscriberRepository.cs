using Microsoft.EntityFrameworkCore;
using WeatherReport.DataAccess;
using WeatherReport.DataAccess.Entities;
using WeatherReport.DataAccess.Repositories.Interfaces;

namespace WeatherReport.DataAccess.Repositories.Implementations;
public class SubscriberRepository(WeatherReportDb context) : ISubscriberRepository
{
    public async Task<IEnumerable<Subscriber>> GetAllAsync()
    {
        return await context.Subscribers.ToListAsync();
    }
    
    public async Task<Subscriber> GetByIdAsync(int id)
    {
        return await context.Subscribers.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AddAsync(Subscriber subscriber)
    {
        await context.Subscribers.AddAsync(subscriber);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Subscriber subscriber)
    {
        context.Subscribers.Update(subscriber);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var subscriber = await context.Subscribers.FindAsync(id);
        if (subscriber != null)
        {
            subscriber.IsDeleted = true;
            context.Subscribers.Update(subscriber);
            await context.SaveChangesAsync();
        }
    }
}