using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Services.CarService;

public interface ICarService
{
    public Task<List<CarReadDto?>> GetCarsAsync();
    public Task<CarReadDto?> GetCarAsync(int id);
    public Task<CarReadDto?> AddCarAsync(CarWriteDto dto);
    public Task<CarReadDto?> UpdateCarAsync(int id, CarWriteDto dto);
    public Task DeleteCarAsync(int id);
}