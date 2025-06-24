using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Services.AuthService;
using ASP.NET_Core_Car_Rental_System_Project_F.Services.TokenBlacklistService;
using ASP.NET_Core_Car_Rental_System_Project_F.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;


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

    [Authorize(Roles = "User,Admin")]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] EmailReadDto readDto)
    {
        var isSent = await _authService.SendOtpToEmailAsync(readDto.Email);
        if (!isSent) return NotFound(CustomMessages.InvalidEmailAddress);

        return Ok(new { Message = CustomMessages.EmailSentSuccessfully });
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordReadDto readDto)
    {
        var success = await _authService.ResetPasswordAsync(readDto);
        if (!success) return BadRequest(CustomMessages.InvalidOtp);
        return Ok(new { Message = CustomMessages.PasswordResetSuccessfully });
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromServices] ITokenBlacklistService blacklistService)
    {
        var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        var exp = User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;

        if (jti == null || exp == null)
            return BadRequest(new { Message = CustomMessages.InvalidToken });

        var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp)).UtcDateTime;

        await blacklistService.AddTokenToBlacklistAsync(jti, expirationTime);

        return Ok(new { Message = CustomMessages.LoggedOutSuccessfully });
    }
}