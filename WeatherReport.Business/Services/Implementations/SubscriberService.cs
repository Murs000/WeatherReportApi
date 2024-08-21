using AutoMapper;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.DataAccess.Entities;
using WeatherReport.DataAccess.Enums;
using WeatherReport.DataAccess.Repositories.Interfaces;

namespace WeatherReport.Business.Services.Implementations;

public class SubscriberService(IRepositoryUnitOfWork repository, IMapper mapper) : ISubscriberService
{
    public async Task<IEnumerable<SubscriberDTO>> GetAllAsync(SubscriptionType? subscriptionType = null, string? city = null)
    {
        var subscribers = await repository.SubscriberRepository.GetAllAsync();
        
        if(subscriptionType != null)
            subscribers = subscribers.Where(s=>s.SubscriptionType == subscriptionType);
        
        if(city != null)
            subscribers = subscribers.Where(s=> s.CityOfResidence == city);

        return mapper.Map<IEnumerable<SubscriberDTO>>(subscribers);
    }
    public async Task<SubscriberDTO> GetByIdAsync(int id)
    {
        var subscriber = await repository.SubscriberRepository.GetByIdAsync(id);
        return mapper.Map<SubscriberDTO>(subscriber);
    }
    public async Task<SubscriberDTO> AddAsync(SubscriberDTO subscriberDto)
    {
        var subscriber = mapper.Map<Subscriber>(subscriberDto);
        subscriber.SetCredentials(); // Setting the creation date
        await repository.SubscriberRepository.AddAsync(subscriber);
        return mapper.Map<SubscriberDTO>(subscriber);
    }
    public async Task<SubscriberDTO> UpdateAsync(SubscriberDTO subscriberDto)
    {
        var subscriber = mapper.Map<Subscriber>(subscriberDto);
        subscriber.SetCredentials(); // Setting the modify date
        await repository.SubscriberRepository.UpdateAsync(subscriber);
        return mapper.Map<SubscriberDTO>(subscriber);
    }
    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.SubscriberRepository.DeleteAsync(id);
    }
}