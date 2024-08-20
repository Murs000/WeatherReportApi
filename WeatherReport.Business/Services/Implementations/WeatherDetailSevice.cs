using AutoMapper;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.DataAccess.Entities;
using WeatherReport.DataAccess.Repositories.Interfaces;

namespace WeatherReport.Business.Services.Implementations;
public class WeatherDetailService(IRepositoryUnitOfWork repository, IMapper mapper) : IWeatherDetailService
{

    public async Task<IEnumerable<WeatherDetailDTO>> GetAllAsync()
    {
        var weatherDetails = await repository.WeatherDetailRepository.GetAllAsync();
        return mapper.Map<IEnumerable<WeatherDetailDTO>>(weatherDetails);
    }

    public async Task<WeatherDetailDTO> GetByIdAsync(int id)
    {
        var weatherDetail = await repository.WeatherDetailRepository.GetByIdAsync(id);
        return mapper.Map<WeatherDetailDTO>(weatherDetail);
    }

    public async Task<WeatherDetailDTO> AddAsync(WeatherDetailDTO weatherDetailDTO)
    {
        var weatherDetail = mapper.Map<WeatherDetail>(weatherDetailDTO);
        weatherDetail.SetCredentials();
        await repository.WeatherDetailRepository.AddAsync(weatherDetail);
        return mapper.Map<WeatherDetailDTO>(weatherDetail);
    }

    public async Task<WeatherDetailDTO> UpdateAsync(WeatherDetailDTO weatherDetailDTO)
    {
        var weatherDetail = mapper.Map<WeatherDetail>(weatherDetailDTO);
        weatherDetail.SetCredentials();
        await repository.WeatherDetailRepository.UpdateAsync(weatherDetail);
        return mapper.Map<WeatherDetailDTO>(weatherDetail);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.WeatherDetailRepository.DeleteAsync(id);
    }
}