using AutoMapper;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;

namespace WeatherReport.Business.Services.Implementations;

public class UserService(IEmailService emailService, IServiceUnitOfWork service,IAuthService authService, IMapper mapper) : IUserService
{
    // for v2
    public async Task<List<UserDTO>> GetAllUsersAsync()
    {
        return mapper.Map<List<UserDTO>>(await service.SubscriberService.GetAllAsync());
    }
    public async Task<UserResponceDTO> LogIn(LoginDTO loginDTO)
    {
        var users = await service.SubscriberService.GetAllAsync();
        var user = users.First(u => u.Username == loginDTO.Username && u.IsActivated == true);
        var refreshToken = authService.GenerateRefreshToken();
        if (user != null)
        {
            user.RefreshToken = refreshToken;
            await service.SubscriberService.UpdateAsync(user);
        }
        if (authService.VerifyPassword(loginDTO.Password, user.PasswordHash, user.PasswordSalt))
        {
            return new UserResponceDTO()
            {
                UserName = user.Username,
                AuthToken = authService.GenerateJwtToken(user.Id, user.Username, user.UserRole),
                RefreshToken = refreshToken
            };
        }
        return null;
    }
    public async Task Register(RegisterDTO registerDTO)
    {
        var subscriberDTO = mapper.Map<SubscriberDTO>(registerDTO);
        (var passwordHash, var passwordSalt) = authService.HashPassword(registerDTO.Password);

        subscriberDTO.PasswordHash = passwordHash;
        subscriberDTO.PasswordSalt = passwordSalt;

        subscriberDTO.RefreshToken = authService.GenerateEmailToken();

        await service.SubscriberService.AddAsync(subscriberDTO);
        await SendConfirmationEmailAsync(subscriberDTO);
    }
    private async Task SendConfirmationEmailAsync(SubscriberDTO user)
    {
        var encodedToken = Uri.EscapeDataString(user.RefreshToken);
        await emailService.SendEmailAsync(user.Email, "OTP email", $"your OTP : {encodedToken}");
    }
    public async Task<bool> ConfirmOTP(string username, string token)
    {
        var users = await service.SubscriberService.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Username == username);

        var decodedToken = Uri.UnescapeDataString(token);

        if (user.RefreshToken != decodedToken && !authService.ValidateEmailToken(token))
        {
            return false;
        }

        user.IsActivated = true;
        user.RefreshToken = null;  // Clear token after confirmation

        await service.SubscriberService.UpdateAsync(user);

        return true;
    }
    public async Task<UserResponceDTO> RefreshToken(RefreshTokenDTO dTO)
    {
        var isValid = authService.ValidateRefreshToken(dTO.RefreshToken);
        if (!isValid)
        {
            return null;
        }

        var users = await service.SubscriberService.GetAllAsync();
        var user = users.First(u => u.Username == dTO.Username);
        if (user == null || user.RefreshToken != dTO.RefreshToken)
        {
            return null;
        }

        var newAccessToken = authService.GenerateJwtToken(user.Id, user.Username, user.UserRole);
        var newRefreshToken = authService.GenerateRefreshToken();

        // Update the user with the new refresh token
        user.RefreshToken = newRefreshToken;
        await service.SubscriberService.UpdateAsync(user);

        return new UserResponceDTO()
        {
            UserName = user.Username,
            AuthToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
    public async Task ForgetPassword(string username)
    {
        var users = await service.SubscriberService.GetAllAsync();
        var user = users.First(u=>u.Username == username);

        await SendConfirmationEmailAsync(user);
    }
    public async Task<bool> ResetPassword(ResetPasswordDTO resetPasswordDTO)
    {
        var users = await service.SubscriberService.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Username == resetPasswordDTO.Username);

        (string passwordHash, string passwordSalt) = authService.HashPassword(resetPasswordDTO.Password);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await service.SubscriberService.UpdateAsync(user);

        return true;
    }
}