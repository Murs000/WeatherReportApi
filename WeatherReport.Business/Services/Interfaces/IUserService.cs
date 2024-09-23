using WeatherReport.Business.DTOs;

namespace WeatherReport.Business.Services.Interfaces;

public interface IUserService
{
    public Task<UserResponceDTO> LogIn(LoginDTO loginDTO);
    public Task Register(RegisterDTO registerDTO);
    public Task<bool> ConfirmOTP(string username, string token);
    public Task<UserResponceDTO> RefreshToken(RefreshTokenDTO dTO);
    public Task ForgetPassword(string username);
    public Task<bool> ResetPassword(ResetPasswordDTO resetPasswordDTO);
}