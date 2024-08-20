using AutoMapper;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.DataAccess.Entities;
using WeatherReport.DataAccess.Repositories.Interfaces;

namespace WeatherReport.Business.Services.Implementations;
public class ForecastService(IRepositoryUnitOfWork repository, IMapper mapper) : IForecastService
{
    public async Task<IEnumerable<ForecastDTO>> GetAllAsync()
    {
        var reports = await repository.ForecastRepository.GetAllAsync();
        return mapper.Map<IEnumerable<ForecastDTO>>(reports);
    }

    public async Task<ForecastDTO> GetByIdAsync(int id)
    {
        var forecast = await repository.ForecastRepository.GetByIdAsync(id);
        return mapper.Map<ForecastDTO>(forecast);
    }

    public async Task<ForecastDTO> AddAsync(ForecastDTO forecastDTO)
    {
        var forecast = mapper.Map<Forecast>(forecastDTO);
        forecast.SetCredentials();
        await repository.ForecastRepository.AddAsync(forecast);
        return mapper.Map<ForecastDTO>(forecast);
    }

    public async Task<ForecastDTO> UpdateAsync(ForecastDTO forecastDTO)
    {
        var forecast = mapper.Map<Forecast>(forecastDTO);
        forecast.SetCredentials();
        await repository.ForecastRepository.UpdateAsync(forecast);
        return mapper.Map<ForecastDTO>(forecast);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.ForecastRepository.DeleteAsync(id);
    }
}