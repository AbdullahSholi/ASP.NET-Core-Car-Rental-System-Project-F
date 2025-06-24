using ASP.NET_Core_Car_Rental_System_Project_F.Auth;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Services.AuthService;

public interface IAuthService
{
    public Task<string?> Login(string email, string password);
    public Task<UserReadDto?> Register(RegisterWriteDto registerDto);
}