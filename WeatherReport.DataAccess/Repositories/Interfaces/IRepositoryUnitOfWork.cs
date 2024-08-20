namespace WeatherReport.DataAccess.Repositories.Interfaces;

public interface IRepositoryUnitOfWork
{
    public ISubscriberRepository SubscriberRepository {get;}
    public IForecastRepository ForecastRepository {get;}
    public IReportRepository ReportRepository {get;}
    public IWeatherDetailRepository WeatherDetailRepository {get;}
}