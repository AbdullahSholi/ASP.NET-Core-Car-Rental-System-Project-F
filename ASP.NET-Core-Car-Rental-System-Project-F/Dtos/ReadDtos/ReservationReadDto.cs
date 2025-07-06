namespace ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;

public class ReservationReadDto
{
    public int ReservationId { get; set; }
    public int UserId { get; set; }
    public int CarId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
}