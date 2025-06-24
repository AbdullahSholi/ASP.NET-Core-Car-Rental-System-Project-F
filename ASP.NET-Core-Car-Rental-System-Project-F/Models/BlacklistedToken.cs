namespace ASP.NET_Core_Car_Rental_System_Project_F.Models;

public class BlacklistedToken
{
    public int Id { get; set; }
    public string Jti { get; set; }
    public DateTime Expiration { get; set; }
}