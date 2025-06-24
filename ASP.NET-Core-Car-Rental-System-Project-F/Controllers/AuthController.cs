using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Services.AuthService;
using ASP.NET_Core_Car_Rental_System_Project_F.Utils;
using Microsoft.AspNetCore.Mvc;


namespace ASP.NET_Core_Car_Rental_System_Project_F.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginWriteDto dto)
    {
        var token = await _authService.LoginAsync(dto.Email, dto.Password);
        if (token == null)
            return Unauthorized(new { Message = CustomMessages.InvalidCredentials });

        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterWriteDto dto)
    {
        try
        {
            var user = await _authService.RegisterAsync(dto);
            if (user == null)
                return Unauthorized(new { Message = CustomMessages.InvalidCredentials });

            return Ok(new
            {
                User = user
            });
        }
        catch (Exception e)
        {
            return BadRequest(new { e.Message });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] EmailReadDto readDto)
    {
        var isSent = await _authService.SendOtpToEmailAsync(readDto.Email);
        if (!isSent) return NotFound(CustomMessages.InvalidEmailAddress);

        return Ok(new { Message = CustomMessages.EmailSentSuccessfully });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordReadDto readDto)
    {
        var success = await _authService.ResetPasswordAsync(readDto);
        if (!success) return BadRequest(CustomMessages.InvalidOtp);
        return Ok(new { Message = CustomMessages.PasswordResetSuccessfully });
    }
}