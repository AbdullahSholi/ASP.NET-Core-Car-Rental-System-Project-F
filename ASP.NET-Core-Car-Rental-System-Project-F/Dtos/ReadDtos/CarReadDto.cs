using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using ASP.NET_Core_Car_Rental_System_Project_F.Models.Enums;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;

public class CarReadDto
{
    public int CarId { get; set; }
    public int Year { get; set; }
    public CarColor Color { get; set; } = CarColor.Black;
    public FuelType FuelType { get; set; } = FuelType.Petrol;
    public TransmissionType TransmissionType { get; set; } = TransmissionType.Automatic;
    public int SeatingCapacity { get; set; } = 5;
    public decimal DailyRentalPrice { get; set; }
    public bool AvailabilityStatus { get; set; } = true;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int CarModelId { get; set; }
}