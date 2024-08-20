namespace WeatherReport.Business.Services.Interfaces;
public interface IServiceUnitOfWork
{
    public ISubscriberService SubscriberService {get;}
    public IForecastService ForecastService {get;}
    public IReportService ReportService {get;}
    public IWeatherDetailService WeatherDetailService {get;}
}
