using WeatherReport.Business.DTOs;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.Business.Services.Interfaces;
public interface ISubscriberService
{
    Task<IEnumerable<SubscriberDTO>> GetAllAsync(SubscriptionType? subscriptionType = null ,string? city = null);
    Task<SubscriberDTO> GetByIdAsync(int id);
    Task AddAsync(SubscriberDTO subscriberDto);
    Task UpdateAsync(SubscriberDTO subscriberDto);
    Task DeleteAsync(int id);
}
