using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Repositories.CarRepository;

public interface ICarRepository
{
    public Task<List<Car?>> GetCarsAsync();
    public Task<Car?> GetCarAsync(int id);
    public Task<Car?> AddCarAsync(Car car);
    public Task<Car?> UpdateCarAsync(Car car);
    public Task DeleteCarAsync(int id);
}