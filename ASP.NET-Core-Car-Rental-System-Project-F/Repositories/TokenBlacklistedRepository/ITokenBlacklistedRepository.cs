using ASP.NET_Core_Car_Rental_System_Project_F.Models;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Repositories.TokenBlacklistedRepository;

public interface ITokenBlacklistedRepository
{
    public Task<bool> CheckIfTokenBlacklistedAsync(string jti);
    public Task AddTokenToBlacklistAsync(BlacklistedToken token);
}