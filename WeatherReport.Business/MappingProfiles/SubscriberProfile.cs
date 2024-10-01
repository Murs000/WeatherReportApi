using AutoMapper;
using WeatherReport.Business.DTOs;
using WeatherReport.DataAccess.Entities;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.Business.MappingProfiles;
public class SubscriberProfile : Profile
{
    public SubscriberProfile()
    {
        CreateMap<Subscriber, SubscriberDTO>().ReverseMap();
        CreateMap<SubscriberDTO,RegisterDTO>().ReverseMap();
        CreateMap<SubscriberDTO, UserDTO>()
            .ForMember(dest => dest.SubscriptionType, opt => opt.MapFrom(src => src.SubscriptionType.ToString()))
            .ReverseMap()
            .ForMember(dest => dest.SubscriptionType, opt => opt.MapFrom(src => Enum.Parse<SubscriptionType>(src.SubscriptionType)));
    }
}