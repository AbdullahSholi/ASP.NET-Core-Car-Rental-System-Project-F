namespace ASP.NET_Core_Car_Rental_System_Project_F.Models;

public class OtpRecord
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Code { get; set; }
    public DateTime Expiration { get; set; }
}