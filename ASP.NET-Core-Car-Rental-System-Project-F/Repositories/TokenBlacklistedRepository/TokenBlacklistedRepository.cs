using ASP.NET_Core_Car_Rental_System_Project_F.Data;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Repositories.TokenBlacklistedRepository;

public class TokenBlacklistedRepository : ITokenBlacklistedRepository
{
    private readonly ApplicationDbContext _context;

    public TokenBlacklistedRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CheckIfTokenBlacklistedAsync(string jti)
    {
        var exists = await _context.BlacklistedTokens
            .AnyAsync(t => t.Jti == jti && t.Expiration > DateTime.UtcNow);

        return exists;
    }

    public async Task AddTokenToBlacklistAsync(BlacklistedToken token)
    {
        _context.BlacklistedTokens.Add(token);
        await _context.SaveChangesAsync();
    }
}