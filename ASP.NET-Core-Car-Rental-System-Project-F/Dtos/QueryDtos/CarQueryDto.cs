using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using ASP.NET_Core_Car_Rental_System_Project_F.Models.Enums;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Dtos.QueryDtos;

public class CarQueryDto
{
    public CarColor? Color { get; set; }
    public int? Year { get; set; }
    public FuelType? FuelType { get; set; }
    public TransmissionType? TransmissionType { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}