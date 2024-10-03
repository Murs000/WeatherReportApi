using WeatherReport.Business.DTOs;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.Business.Services.Interfaces;

public interface IUserService
{
    public Task<List<UserInfoDTO>> GetAllUsersAsync();
    public Task<UserInfoDTO> GetUserAsync(int id);
    public Task<bool> UpdateRole(int id, UserRole userRole);
    public Task<UserResponceDTO> LogIn(LoginDTO loginDTO);
    public Task Register(RegisterDTO registerDTO);
    public Task<bool> ConfirmOTP(string username, string token);
    public Task<UserResponceDTO> RefreshToken(RefreshTokenDTO dTO);
    public Task ForgetPassword(string username);
    public Task<bool> ResetPassword(ResetPasswordDTO resetPasswordDTO);
}