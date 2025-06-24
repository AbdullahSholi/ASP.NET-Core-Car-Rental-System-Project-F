using ASP.NET_Core_Car_Rental_System_Project_F.Auth;
using ASP.NET_Core_Car_Rental_System_Project_F.Data;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Repositories.AuthRepository;

public class AuthRepository : IAuthRepository
{
    private readonly ApplicationDbContext _context;

    public AuthRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task<User?> Register(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }
}