namespace WeatherReport.Business.DTOs;

public class RegisterDTO
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string CityOfResidence { get; set; }
}