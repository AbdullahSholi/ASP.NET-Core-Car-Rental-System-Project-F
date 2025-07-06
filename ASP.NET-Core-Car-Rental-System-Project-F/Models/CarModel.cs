namespace ASP.NET_Core_Car_Rental_System_Project_F.Models;

public class CarModel
{
    public int CarModelId { get; set; }
    public string Name { get; set; } = string.Empty;

    public int CarBrandId { get; set; }

    public CarBrand? CarBrand { get; set; }

    public ICollection<Car>? Cars { get; set; }
}