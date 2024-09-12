using System.ComponentModel.DataAnnotations;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.DataAccess.Entities;

public class Subscriber : BaseEntity
{
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string CityOfResidence { get; set; }
    public UserRole UserRole { get; set; }
    public SubscriptionType SubscriptionType { get; set; }
    public DateTime SubscriptionDate { get; set;}
    public List<Forecast> Forecasts { get; set; }
}