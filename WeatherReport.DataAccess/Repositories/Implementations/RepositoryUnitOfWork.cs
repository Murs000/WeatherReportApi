using WeatherReport.DataAccess.Repositories.Interfaces;

namespace WeatherReport.DataAccess.Repositories.Implementations;

public class RepositoryUnitOfWork(WeatherReportDb context) : IRepositoryUnitOfWork
{
    public ISubscriberRepository SubscriberRepository => new SubscriberRepository(context);
    public IForecastRepository ForecastRepository => new ForecastRepository(context);
    public IReportRepository ReportRepository => new ReportRepository(context);
    public IWeatherDetailRepository WeatherDetailRepository => new WeatherDetailRepository(context);
}