using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.Business.Settings;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.Business.Services.Implementations;


public class CustomerService(IServiceUnitOfWork service,IWeatherApiService weatherService, IMapper mapper,IHttpContextAccessor httpContextAccessor) : ICustomerService
{
    public async Task<UserDTO> GetCurrent()
    {
        var userId = httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return null;
        }

        return mapper.Map<UserDTO>(await service.SubscriberService.GetByIdAsync(Convert.ToInt32(userId)));
    }
    public async Task<CurrentWeatherDTO> GetCurrentWeather()
    {
        var userId = httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return null;
        }

        var subscriberDTO = await service.SubscriberService.GetByIdAsync(Convert.ToInt32(userId));

        var forecast = await weatherService.GetCurrentWeatherDataAsync(subscriberDTO.CityOfResidence);

        return new CurrentWeatherDTO
        {
            Description = forecast.Reports.First().WeatherDetails.First().Description,
            Icon = $"http://openweathermap.org/img/wn/{forecast.Reports.First().WeatherDetails.First().Icon}.png"
        };
    }

    public async Task<bool> ChangeSubscription(SubscriptionType? type)
    {
        var userId = httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return false;
        }

        var subscriberDTO = await service.SubscriberService.GetByIdAsync(Convert.ToInt32(userId));

        if (type != null)
            subscriberDTO.SubscriptionType = (SubscriptionType)type;

        await service.SubscriberService.UpdateAsync(subscriberDTO);

        return true;
    }    
    public async Task<bool> ChangeCity(string? city)
    {
        var userId = httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return false;
        }

        var subscriberDTO = await service.SubscriberService.GetByIdAsync(Convert.ToInt32(userId));

        if (city != null)
            subscriberDTO.CityOfResidence = city;

        await service.SubscriberService.UpdateAsync(subscriberDTO);

        return true;
    }
    public async Task<bool> ChangeCridentials(string? name, string? surname)
    {
        var userId = httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return false;
        }

        var subscriberDTO = await service.SubscriberService.GetByIdAsync(Convert.ToInt32(userId));

        if (name != null)
            subscriberDTO.Name = name;
        if (surname != null)
            subscriberDTO.Surname = surname;

        await service.SubscriberService.UpdateAsync(subscriberDTO);

        return true;
    }
}