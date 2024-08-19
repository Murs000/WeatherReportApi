using AutoMapper;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.DataAccess.Entities;
using WeatherReport.DataAccess.Enums;
using WeatherReport.DataAccess.Repositories.Interfaces;

namespace WeatherReport.Business.Services.Implementations;

public class SubscriberService : ISubscriberService
{
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly IMapper _mapper;

    public SubscriberService(ISubscriberRepository subscriberRepository, IMapper mapper)
    {
        _subscriberRepository = subscriberRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<SubscriberDTO>> GetAllAsync(SubscriptionType? subscriptionType = null, string? city = null)
    {
        var subscribers = await _subscriberRepository.GetAllAsync();
        
        if(subscriptionType != null)
            subscribers = subscribers.Where(s=>s.SubscriptionType == subscriptionType);
        
        if(city != null)
            subscribers = subscribers.Where(s=> s.CityOfResidence == city);

        return _mapper.Map<IEnumerable<SubscriberDTO>>(subscribers);
    }

    public async Task<SubscriberDTO> GetByIdAsync(int id)
    {
        var subscriber = await _subscriberRepository.GetByIdAsync(id);
        return _mapper.Map<SubscriberDTO>(subscriber);
    }

    public async Task AddAsync(SubscriberDTO subscriberDto)
    {
        var subscriber = _mapper.Map<Subscriber>(subscriberDto);
        subscriber.SetCredentials(); // Setting the creation date
        await _subscriberRepository.AddAsync(subscriber);
    }
    public async Task UpdateAsync(SubscriberDTO subscriberDto)
    {
        var subscriber = _mapper.Map<Subscriber>(subscriberDto);
        await _subscriberRepository.UpdateAsync(subscriber);
    }

    public async Task DeleteAsync(int id)
    {
        await _subscriberRepository.DeleteAsync(id);
    }
}