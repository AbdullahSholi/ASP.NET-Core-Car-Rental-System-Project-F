namespace ASP.NET_Core_Car_Rental_System_Project_F.Dtos;

public class UserReadDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
}
