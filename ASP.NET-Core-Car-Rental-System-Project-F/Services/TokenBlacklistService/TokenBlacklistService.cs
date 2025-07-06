using System.Collections.Concurrent;
using ASP.NET_Core_Car_Rental_System_Project_F.Data;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using ASP.NET_Core_Car_Rental_System_Project_F.Repositories.TokenBlacklistedRepository;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Services.TokenBlacklistService;

public class TokenBlacklistService : ITokenBlacklistService
{
    private readonly ITokenBlacklistedRepository _tokenBlacklistedRepository;

    public TokenBlacklistService(ITokenBlacklistedRepository tokenBlacklistedRepository)
    {
        _tokenBlacklistedRepository = tokenBlacklistedRepository;
    }

    public async Task<bool> IsTokenBlacklistedAsync(string jti)
    {
        return await _tokenBlacklistedRepository.CheckIfTokenBlacklistedAsync(jti);
    }

    public async Task AddTokenToBlacklistAsync(string jti, DateTime expiration)
    {
        var token = new BlacklistedToken
        {
            Jti = jti,
            Expiration = expiration
        };

        await _tokenBlacklistedRepository.AddTokenToBlacklistAsync(token);
    }
}