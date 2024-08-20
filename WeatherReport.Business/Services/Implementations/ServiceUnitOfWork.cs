using AutoMapper;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.DataAccess.Repositories.Interfaces;

namespace WeatherReport.Business.Services.Implementations;
public class ServiceUnitOfWork(IRepositoryUnitOfWork repository, IMapper mapper) : IServiceUnitOfWork
{
    public ISubscriberService SubscriberService => new SubscriberService( repository, mapper);
    public IForecastService ForecastService => new ForecastService( repository, mapper);
    public IReportService ReportService => new ReportService( repository, mapper);
    public IWeatherDetailService WeatherDetailService => new WeatherDetailService( repository, mapper);
}