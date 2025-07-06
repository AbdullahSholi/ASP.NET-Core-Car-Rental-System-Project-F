using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;

public class LoginWriteDto
{
    [Required] [EmailAddress] public string Email { get; set; } = "";

    [Required] [MinLength(8)] public string Password { get; set; } = "";
}