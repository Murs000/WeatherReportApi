using Microsoft.EntityFrameworkCore;
using WeatherReport.DataAccess;
using WeatherReport.DataAccess.Entities;
using WeatherReport.DataAccess.Repositories.Interfaces;

namespace WeatherReport.DataAccess.Repositories.Implementations;
public class WeatherDetailRepository(WeatherReportDb context) : IWeatherDetailRepository
{
    public async Task<IEnumerable<WeatherDetail>> GetAllAsync()
    {
        return await context.WeatherDetails.ToListAsync();
    }
    
    public async Task<WeatherDetail> GetByIdAsync(int id)
    {
        return await context.WeatherDetails.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AddAsync(WeatherDetail weatherDetail)
    {
        await context.WeatherDetails.AddAsync(weatherDetail);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(WeatherDetail weatherDetail)
    {
        context.WeatherDetails.Update(weatherDetail);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var weatherDetail = await context.WeatherDetails.FindAsync(id);
        if (weatherDetail == null || weatherDetail.IsDeleted)
        {
            return false;
        }

        weatherDetail.IsDeleted = true;
        await context.SaveChangesAsync();
        return true;
    }
}