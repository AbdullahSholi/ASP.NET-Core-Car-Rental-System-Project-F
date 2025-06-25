using ASP.NET_Core_Car_Rental_System_Project_F.Models.Enums;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Models;

public class Reservation
{
    public int ReservationId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int CarId { get; set; }
    public Car Car { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public decimal TotalPrice { get; set; }

    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
}