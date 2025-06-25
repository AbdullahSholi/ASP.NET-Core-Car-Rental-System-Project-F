using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.QueryDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using ASP.NET_Core_Car_Rental_System_Project_F.Repositories.CarRepository;
using AutoMapper;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Services.CarService;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;

    public CarService(ICarRepository carRepository, IMapper mapper)
    {
        _carRepository = carRepository;
        _mapper = mapper;
    }

    public async Task<List<CarReadDto?>> GetCarsAsync()
    {
        var cars = await _carRepository.GetCarsAsync();
        var carReadDtos = cars.Select(_mapper.Map<CarReadDto>).ToList();

        return carReadDtos;
    }

    public async Task<CarReadDto?> GetCarAsync(int id)
    {
        var car = await _carRepository.GetCarAsync(id);
        var carReadDto = _mapper.Map<CarReadDto>(car);

        return carReadDto;
    }

    public async Task<CarReadDto?> AddCarAsync(CarWriteDto dto)
    {
        var car = _mapper.Map<Car>(dto);
        await _carRepository.AddCarAsync(car);
        var carReadDto = _mapper.Map<CarReadDto>(car);

        return carReadDto;
    }

    public async Task<CarReadDto?> UpdateCarAsync(int id, CarWriteDto dto)
    {
        var car = await _carRepository.GetCarAsync(id);
        if (car == null)
            return null;
        _mapper.Map(dto, car);
        await _carRepository.UpdateCarAsync(car);

        var carReadDto = _mapper.Map<CarReadDto>(car);

        return carReadDto;
    }

    public async Task DeleteCarAsync(int id)
    {
        await _carRepository.DeleteCarAsync(id);
    }

    public async Task<List<CarReadDto?>> SearchAvailableCarAsync(CarQueryDto dto)
    {
        var cars = await _carRepository.SearchAvailableCarAsync(dto);
        var carsReadDto = cars.Select(car => _mapper.Map<CarReadDto>(car)).ToList();

        return carsReadDto;
    }
}