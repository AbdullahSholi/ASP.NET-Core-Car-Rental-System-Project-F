using ASP.NET_Core_Car_Rental_System_Project_F.Data;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using ASP.NET_Core_Car_Rental_System_Project_F.Utils;
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

    public async Task<User?> RegisterUserAsync(User user)
    {
        user.Password = PasswordHasher.HashPassword(user.Password);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task SaveOtpAsync(OtpRecord otpRecord)
    {
        await _context.OtpRecords.AddAsync(otpRecord);
        await _context.SaveChangesAsync();
    }

    public async Task<OtpRecord?> GetOtpRecordAsync(string email, string otp)
    {
        var record = await _context.OtpRecords
            .Where(r => r.Email == email && r.Code == otp)
            .OrderByDescending(r => r.Expiration)
            .FirstOrDefaultAsync();

        return record;
    }

    public async Task InvalidateOtpAsync(OtpRecord otpRecord)
    {
        _context.OtpRecords.Remove(otpRecord);
        await _context.SaveChangesAsync();
    }

    public async Task HashAndSavePasswordAsync(User? user, string newPassword)
    {
        user.Password = PasswordHasher.HashPassword(newPassword);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAndSaveOtpAsync(OtpRecord record)
    {
        _context.OtpRecords.Remove(record);
        await _context.SaveChangesAsync();
    }
}