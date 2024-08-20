using AutoMapper;
using WeatherReport.Business.DTOs;
using WeatherReport.DataAccess.Entities;

namespace WeatherReport.Business.MappingProfiles;

public class ForecastProfile : Profile
{
    public ForecastProfile()
    {   
        CreateMap<Forecast, ForecastDTO>().ReverseMap(); 
    }
}