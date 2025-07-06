using System.ComponentModel.DataAnnotations;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using ASP.NET_Core_Car_Rental_System_Project_F.Models.Enums;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;

public class CarWriteDto
{
    [Required] [Range(1950, int.MaxValue)] public int Year { get; set; }
    [Required] public CarColor Color { get; set; } = CarColor.Black;
    [Required] public FuelType FuelType { get; set; } = FuelType.Petrol;
    [Required] public TransmissionType TransmissionType { get; set; } = TransmissionType.Automatic;
    [Required] public int SeatingCapacity { get; set; } = 5;
    [Required] public decimal DailyRentalPrice { get; set; }
    [Required] public bool AvailabilityStatus { get; set; } = true;
    [Required] public string Description { get; set; } = string.Empty;
    [Required] public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required] public int CarModelId { get; set; }
}