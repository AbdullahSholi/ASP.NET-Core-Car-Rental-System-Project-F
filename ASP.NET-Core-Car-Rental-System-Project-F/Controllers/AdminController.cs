using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Services.CarService;
using ASP.NET_Core_Car_Rental_System_Project_F.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly ICarService _carService;
    private readonly ILogger<UserController> _logger;

    public AdminController(ICarService carService, ILogger<UserController> logger)
    {
        _carService = carService;
        _logger = logger;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("car")]
    public async Task<IActionResult> CreateCar([FromBody] CarWriteDto dto)
    {
        try
        {
            var car = await _carService.AddCarAsync(dto);
            if (car == null)
                return BadRequest(new { message = CustomMessages.InvalidCarInformation });

            return CreatedAtAction(
                "GetCar",
                "User",
                new { id = car.CarId },
                car
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, CustomMessages.AddingNewCarError);
            return StatusCode(500, new { message = CustomMessages.InternalServerError });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("car/{id:int}")]
    public async Task<IActionResult> UpdateCar(int id, [FromBody] CarWriteDto dto)
    {
        try
        {
            var car = await _carService.UpdateCarAsync(id, dto);
            if (car == null)
                return BadRequest(new { message = CustomMessages.InvalidCarInformation });

            return Ok(
                car
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, CustomMessages.UpdatingCarError, id);
            return StatusCode(500, new { message = CustomMessages.InternalServerError });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("car/{id:int}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        try
        {
            await _carService.DeleteCarAsync(id);

            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, CustomMessages.DeletingCarError, id);
            return StatusCode(500, new { message = CustomMessages.InternalServerError });
        }
    }
}