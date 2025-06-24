using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;

public class EmailReadDto
{
    [Required] public string Email { get; set; } = string.Empty;
}