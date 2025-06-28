using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.QueryDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using ASP.NET_Core_Car_Rental_System_Project_F.Models.Enums;
using ASP.NET_Core_Car_Rental_System_Project_F.Services.CarService;
using ASP.NET_Core_Car_Rental_System_Project_F.Services.ReservationService;
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
    private readonly IReservationService _reservationService;

    public UserController(ICarService carService, ILogger<UserController> logger,
        IReservationService reservationService)
    {
        _carService = carService;
        _logger = logger;
        _reservationService = reservationService;
    }

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
            var reservation = await _reservationService.BookCarAsync(dto);
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

    [Authorize(Roles = "User,Admin")]
    [HttpDelete("reservation/{id:int}")]
    public async Task<IActionResult> BookCar([FromRoute] int id)
    {
        try
        {
            await _reservationService.RemoveReservationAsync(id);

            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, CustomMessages.FailedToRemoveReservation);
            return StatusCode(500, new { message = CustomMessages.InternalServerError });
        }
    }

    [Authorize(Roles = "User,Admin")]
    [HttpGet("search")]
    public async Task<IActionResult> SearchAvailableCar([FromQuery] CarQueryDto dto)
    {
        try
        {
            var cars = await _carService.SearchAvailableCarAsync(dto);

            return Ok(
                cars
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, CustomMessages.FailedToDisplayAvailableCars);
            return StatusCode(500, new { message = CustomMessages.InternalServerError });
        }
    }
    
    [HttpGet("reservations/{id:int}")]
    public async Task<IActionResult> GetUserReservations([FromRoute] int id)
    {
        try
        {
            var reservations = await _reservationService.GetReservationsAsync(id);
            return Ok(reservations);
        }
        catch (Exception e)
        {
            _logger.LogError(e, CustomMessages.ListingReservationsError);
            return StatusCode(500, new { message = CustomMessages.InternalServerError });
        }
    }
}