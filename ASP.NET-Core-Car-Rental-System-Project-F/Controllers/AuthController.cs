using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ASP.NET_Core_Car_Rental_System_Project_F.Auth;
using ASP.NET_Core_Car_Rental_System_Project_F.Data;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using ASP.NET_Core_Car_Rental_System_Project_F.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IOptions<JwtSettings> _jwtSettings;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public AuthController(ApplicationDbContext context, IOptions<JwtSettings> jwtSettings,
        JwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _jwtSettings = jwtSettings;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var user = _context.Users.SingleOrDefault(u => u.Email == dto.Email);
        if (user == null && !PasswordHasher.VerifyPassword(dto.Password, user.Password))
            return Unauthorized("Invalid credentials.");

        var token = _jwtTokenGenerator.GenerateToken(user.Email, user.Role);

        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = new User();
        user.Email = dto.Email;
        user.Password = dto.Password;
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Password = PasswordHasher.HashPassword(dto.Password);
        user.PhoneNumber = dto.PhoneNumber;
        user.DateOfBirth = dto.DateOfBirth;
        user.Address1 = dto.Address1;
        user.Address2 = dto.Address2;
        user.City = dto.City;
        user.Country = dto.Country;
        user.DriverLicense = dto.DriverLicense;

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return Ok(new
        {
            User = user
        });
    }
}