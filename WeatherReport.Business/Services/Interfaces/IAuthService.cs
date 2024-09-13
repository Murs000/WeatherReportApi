namespace WeatherReport.Business.Services.Interfaces;

public interface IAuthService
{
    public (string passwordHash, string passwordSalt) HashPassword(string password);
    public bool VerifyPassword(string password, string storedHash, string storedSalt);
    public string GenerateJwtToken(int userId, string username, string role);
    public string GenerateRefreshToken();
    public bool ValidateRefreshToken(string token);
    public string GenerateEmailToken();
    public bool ValidateEmailToken(string token);
}