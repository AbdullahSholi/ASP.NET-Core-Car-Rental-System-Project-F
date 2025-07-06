using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;

public class ReservationWriteDto
{
    [Required] public int UserId { get; set; }
    [Required] public int CarId { get; set; }
    [Required] public DateTime StartDate { get; set; }
    [Required] public DateTime EndDate { get; set; }
}