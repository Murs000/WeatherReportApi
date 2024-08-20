using WeatherReport.Business.DTOs;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.Business.Services.Interfaces;
public interface ISubscriberService
{
    Task<IEnumerable<SubscriberDTO>> GetAllAsync(SubscriptionType? subscriptionType = null ,string? city = null);
    Task<SubscriberDTO> GetByIdAsync(int id);
    Task<SubscriberDTO> AddAsync(SubscriberDTO subscriberDto);
    Task<SubscriberDTO> UpdateAsync(SubscriberDTO subscriberDto);
    Task<bool> DeleteAsync(int id);
}
