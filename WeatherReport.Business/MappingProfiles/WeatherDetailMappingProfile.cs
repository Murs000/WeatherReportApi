using AutoMapper;
using WeatherReport.Business.DTOs;
using WeatherReport.DataAccess.Entities;

namespace WeatherReport.Business.MappingProfiles;

public class WeatherDetailProfile : Profile
{
    public WeatherDetailProfile()
    {
        CreateMap<WeatherDetail, WeatherDetailDTO>().ReverseMap();
    }
}