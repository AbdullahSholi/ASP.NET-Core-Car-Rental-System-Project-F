using ASP.NET_Core_Car_Rental_System_Project_F.Models;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Repositories.AuthRepository;

public interface IAuthRepository
{
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<User?> RegisterUserAsync(User user);
    public Task<bool> EmailExistsAsync(string email);
    public Task SaveOtpAsync(OtpRecord otpRecord);
    public Task<OtpRecord?> GetOtpRecordAsync(string email, string otp);
    public Task InvalidateOtpAsync(OtpRecord otpRecord);
    public Task HashAndSavePasswordAsync(User? user, string newPassword);
    public Task RemoveAndSaveOtpAsync(OtpRecord record);
}