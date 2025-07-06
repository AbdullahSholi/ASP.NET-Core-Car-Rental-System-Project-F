namespace ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;

public class ResetPasswordReadDto
{
    public string Email { get; } = string.Empty;
    public string Otp { get; } = string.Empty;
    public string NewPassword { get; } = string.Empty;
}