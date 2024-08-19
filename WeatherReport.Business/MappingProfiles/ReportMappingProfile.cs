using AutoMapper;
using WeatherReport.Business.DTOs;
using WeatherReport.DataAccess.Entities;

namespace WeatherReport.Business.MappingProfiles;

public class ReportProfile : Profile
{
    public ReportProfile()
    {
        CreateMap<Report, ReportDTO>().ReverseMap();
        
        CreateMap<Forecast, ForecastDTO>().ReverseMap(); 
    }
}