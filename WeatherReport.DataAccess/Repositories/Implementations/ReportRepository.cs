using Microsoft.EntityFrameworkCore;
using WeatherReport.DataAccess.Entities;
using WeatherReport.DataAccess.Repositories.Interfaces;

namespace WeatherReport.DataAccess.Repositories.Implementations;

public class ReportRepository(WeatherReportDb context) : IReportRepository
{

    public async Task<IEnumerable<Report>> GetAllAsync()
    {
        return await context.Report.ToListAsync();
    }

    public async Task<Report> GetByIdAsync(int id)
    {
        return await context.Report.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AddAsync(Report report)
    {
        await context.Report.AddAsync(report);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Report report)
    {
        context.Report.Update(report);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var report = await context.Report.FindAsync(id);
        if (report == null || report.IsDeleted)
        {
            return false;
        }

        report.IsDeleted = true;
        await context.SaveChangesAsync();
        return true;
    }
}