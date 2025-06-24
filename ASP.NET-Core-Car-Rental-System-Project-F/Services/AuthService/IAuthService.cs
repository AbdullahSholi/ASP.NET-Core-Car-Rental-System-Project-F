using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Services.AuthService;

public interface IAuthService
{
    public Task<string?> LoginAsync(string email, string password);
    public Task<UserReadDto?> RegisterAsync(RegisterWriteDto registerDto);
    public Task<bool> SendOtpToEmailAsync(string email);
    public Task<bool> ResetPasswordAsync(ResetPasswordReadDto readDto);
}