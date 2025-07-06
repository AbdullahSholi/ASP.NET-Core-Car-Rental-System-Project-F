namespace ASP.NET_Core_Car_Rental_System_Project_F.Models;

public class CarBrand
{
    public int CarBrandId { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<CarModel>? Models { get; set; }
}