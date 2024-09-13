namespace WeatherReport.Business.DTOs;
public class ResetPasswordDTO
{
    public string Username { get; set; }
    public string Token { get; set; }
    public string Password { get; set; }
}