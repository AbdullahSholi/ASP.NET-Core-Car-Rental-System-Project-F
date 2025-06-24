using ASP.NET_Core_Car_Rental_System_Project_F.Auth;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Repositories.AuthRepository;

public interface IAuthRepository
{
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<User?> Register(User user);
}