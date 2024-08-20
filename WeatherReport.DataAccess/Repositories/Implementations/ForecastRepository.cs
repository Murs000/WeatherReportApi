using Microsoft.EntityFrameworkCore;
using WeatherReport.DataAccess.Entities;
using WeatherReport.DataAccess.Repositories.Interfaces;

namespace WeatherReport.DataAccess.Repositories.Implementations;

public class ForecastRepository(WeatherReportDb context) : IForecastRepository
{

    public async Task<IEnumerable<Forecast>> GetAllAsync()
    {
        return await context.Forecasts.ToListAsync();
    }

    public async Task<Forecast> GetByIdAsync(int id)
    {
        return await context.Forecasts.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AddAsync(Forecast forecast)
    {
        await context.Forecasts.AddAsync(forecast);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Forecast forecast)
    {
        context.Forecasts.Update(forecast);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var forecast = await context.Forecasts.FindAsync(id);
        if (forecast == null || forecast.IsDeleted)
        {
            return false;
        }

        forecast.IsDeleted = true;
        await context.SaveChangesAsync();
        return true;
    }
}