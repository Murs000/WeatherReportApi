using WeatherReport.DataAccess.Enums;

namespace WeatherReport.Business.DTOs;

public class SubscriberDTO
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt{ get; set; }
    public string? RefreshToken { get; set; }
    public bool IsActivated { get; set; }
    public string UserRole { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string CityOfResidence { get; set; }
    public SubscriptionType SubscriptionType { get; set; } 
}