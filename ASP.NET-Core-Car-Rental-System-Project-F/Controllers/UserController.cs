using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Services.CarService;
using ASP.NET_Core_Car_Rental_System_Project_F.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ICarService _carService;
    private readonly ILogger<UserController> _logger;

    public UserController(ICarService carService, ILogger<UserController> logger)
    {
        _carService = carService;
        _logger = logger;
    }

    [Authorize(Roles = "User,Admin")]
    [HttpGet("cars")]
    public async Task<IActionResult> GetCars()
    {
        try
        {
            var cars = await _carService.GetCarsAsync();
            return Ok(cars);
        }
        catch (Exception e)
        {
            _logger.LogError(e, CustomMessages.ListingCarsError);
            return StatusCode(500, new { message = CustomMessages.InternalServerError });
        }
    }

    [Authorize(Roles = "User,Admin")]
    [HttpGet("car/{id:int}")]
    public async Task<IActionResult> GetCar([FromRoute] int id)
    {
        try
        {
            var car = await _carService.GetCarAsync(id);
            if (car == null)
                return BadRequest(new { message = CustomMessages.CarNotFound });
            return Ok(new { car });
        }
        catch (Exception e)
        {
            _logger.LogError(e, CustomMessages.GettingCarError, id);
            return StatusCode(500, new { message = CustomMessages.InternalServerError });
        }
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost("reservation")]
    public async Task<IActionResult> BookCar([FromBody] ReservationWriteDto dto)
    {
        try
        {
            var reservation = await _carService.BookCarAsync(dto);
            if (reservation == null)
                return BadRequest(new { message = CustomMessages.FailedToBookCar });
            return Ok(new { reservation });
        }
        catch (Exception e)
        {
            _logger.LogError(e, CustomMessages.BookCarError, dto.CarId);
            return StatusCode(500, new { message = CustomMessages.InternalServerError });
        }
    }
}