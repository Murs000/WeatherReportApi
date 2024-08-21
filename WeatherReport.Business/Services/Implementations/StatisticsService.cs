using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;

namespace WeatherReport.Business.Services.Implementations;

public class StatisticsService(IServiceUnitOfWork service) : IStatisticsService
{
    public async Task<IEnumerable<StatsDTO>> GetCityStats()
    {
        var subscribers = await service.SubscriberService.GetAllAsync();
        var totalSubscribers = subscribers.Count();

        // Group by city and calculate stats
        var stats = subscribers
            .GroupBy(s => s.CityOfResidence)
            .Select(group => new StatsDTO
            {
                Category = group.Key, // City name
                Number = group.Count(), // Count of subscribers in this city
                Percentage = $"{(double)group.Count() / totalSubscribers * 100:F2}%" // Percentage formatted to 2 decimal places
            })
            .ToList();

        return stats;
    }
    public async Task<Dictionary<string, int>> GetEmailStats()
    {
        var subscribers = await service.SubscriberService.GetAllAsync();
        var forecasts = await service.ForecastService.GetAllAsync();

        var stats = subscribers.ToDictionary(
            subscriber => $"{subscriber.Id} {subscriber.Name} {subscriber.Surname}",
            subscriber => forecasts.Count(forecast => forecast.SubscriberId == subscriber.Id)
        );
        return stats;
    }
    public async Task<IEnumerable<StatsDTO>> GetSubsciptionsStats()
    {
        var subscribers = await service.SubscriberService.GetAllAsync();
        var totalSubscribers = subscribers.Count();

        // Group by city and calculate stats
        var stats = subscribers
            .GroupBy(s => s.SubscriptionType)
            .Select(group => new StatsDTO
            {
                Category = group.Key.ToString(), // City name
                Number = group.Count(), // Count of subscribers in this city
                Percentage = $"{(double)group.Count() / totalSubscribers * 100:F2}%" // Percentage formatted to 2 decimal places
            })
            .ToList();

        return stats;
    }
    #region OdlCode
    // public async Task<IEnumerable<StatsDTO>> GetCityStats()
    // {
    //     var subscribers = await service.SubscriberService.GetAllAsync();

    //     var cities = subscribers.Select(s=>s.CityOfResidence);

    //     var uniqueCities = subscribers.Select(s=>s.CityOfResidence).Distinct();

    //     var stats = new List<StatsDTO>();
    //     foreach(var city in uniqueCities)
    //     {
    //         var stat = new StatsDTO
    //         {
    //             Category = city,
    //             Number = cities.Where(c=>c == city).ToList().Count,
    //             Percentage = $"{((double)cities.Where(c=>c == city).ToList().Count / cities.ToList().Count)*100}%"
    //         };
    //         stats.Add(stat);
    //     }
    //     return stats;
    // }

    // public async Task<Dictionary<string, int>> GetCityStats()
    // {
    //     var subscribers = await service.SubscriberService.GetAllAsync();

    //     // Group by city and count occurrences of each city
    //     var stats = subscribers
    //         .GroupBy(s => s.CityOfResidence)
    //         .ToDictionary(g => g.Key, g => g.Count());

    //     return stats;
    // }
    #endregion
}