using WeatherReport.Business.DTOs;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.Business.Services.Interfaces;

public interface ICustomerService
{
    public Task<UserDTO> GetCurrent();
    public Task<CurrentWeatherDTO> GetCurrentWeather();
    public Task<bool> ChangeSubscription(SubscriptionType? type);
    public Task<bool> ChangeCity(string? city);
    public Task<bool> ChangeCridentials(string? name, string? surname);
}