namespace ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;

public class UserReadDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = "User";
}