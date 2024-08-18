using WeatherReport.Business.DTOs;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.Business.Services.Interfaces;
public interface ISubscriberService
{
    Task<IEnumerable<SubscriberDTO>> GetAllAsync();
    Task<SubscriberDTO> GetByIdAsync(int id);
    Task<IEnumerable<SubscriberDTO>> GetBySubscriptionTypeAsync(SubscriptionType subscriptionType);
    Task AddAsync(SubscriberDTO subscriberDto);
    Task UpdateAsync(SubscriberDTO subscriberDto);
    Task DeleteAsync(int id);
}
