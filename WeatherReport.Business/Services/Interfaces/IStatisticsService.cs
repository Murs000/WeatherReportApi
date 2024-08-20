using WeatherReport.Business.DTOs;

namespace WeatherReport.Business.Services.Interfaces;

public interface IStatisticsService
{
    public Task<IEnumerable<StatsDTO>> GetCityStats();
    public Task<Dictionary<string,int>> GetEmailStats();
    public Task<IEnumerable<StatsDTO>> GetSubsciptionsStats();
}