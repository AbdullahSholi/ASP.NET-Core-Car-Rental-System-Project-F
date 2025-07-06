using ASP.NET_Core_Car_Rental_System_Project_F.Data;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.QueryDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Repositories.CarRepository;

public class CarRepository : ICarRepository
{
    private readonly ApplicationDbContext _context;

    public CarRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Car?>> GetCarsAsync()
    {
        var cars = await _context.Cars.ToListAsync();
        return cars;
    }

    public async Task<Car?> GetCarAsync(int id)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == id);
        return car;
    }

    public async Task<Car?> AddCarAsync(Car car)
    {
        _context.Add(car);
        await _context.SaveChangesAsync();

        return car;
    }

    public async Task<Car?> UpdateCarAsync(Car car)
    {
        var carToUpdate = _context.Cars.FirstOrDefault(c => c.CarId == car.CarId);
        if (carToUpdate == null)
            return null;

        carToUpdate.Year = car.Year;
        carToUpdate.Color = car.Color;
        carToUpdate.FuelType = car.FuelType;
        carToUpdate.TransmissionType = car.TransmissionType;
        carToUpdate.SeatingCapacity = car.SeatingCapacity;
        carToUpdate.DailyRentalPrice = car.DailyRentalPrice;
        carToUpdate.AvailabilityStatus = car.AvailabilityStatus;
        carToUpdate.Description = car.Description;
        carToUpdate.CarModelId = car.CarModelId;

        await _context.SaveChangesAsync();
        return car;
    }

    public async Task DeleteCarAsync(int id)
    {
        var carToDelete = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == id);
        if (carToDelete == null)
            return;
        _context.Cars.Remove(carToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Car?>> SearchAvailableCarAsync(CarQueryDto dto)
    {
        var query = _context.Cars.AsQueryable();

        if (dto.Color.HasValue)
            query = query.Where(c => c.Color == dto.Color.Value);

        if (dto.FuelType.HasValue)
            query = query.Where(c => c.FuelType == dto.FuelType.Value);

        if (dto.TransmissionType.HasValue)
            query = query.Where(c => c.TransmissionType == dto.TransmissionType.Value);

        if (dto.Year.HasValue)
            query = query.Where(c => c.Year == dto.Year.Value);

        var cars = await query.ToListAsync();
        var availableCars = cars.Where(c => c.AvailabilityStatus).ToList();

        return availableCars;
    }
}