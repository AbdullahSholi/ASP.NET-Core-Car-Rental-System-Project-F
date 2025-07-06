namespace ASP.NET_Core_Car_Rental_System_Project_F.Services.TokenBlacklistService;

public interface ITokenBlacklistService
{
    Task<bool> IsTokenBlacklistedAsync(string jti);
    Task AddTokenToBlacklistAsync(string jti, DateTime expiration);
}