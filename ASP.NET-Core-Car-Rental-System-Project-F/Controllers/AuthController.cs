using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ASP.NET_Core_Car_Rental_System_Project_F.Auth;
using ASP.NET_Core_Car_Rental_System_Project_F.Data;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using ASP.NET_Core_Car_Rental_System_Project_F.Repository;
using ASP.NET_Core_Car_Rental_System_Project_F.Services.AuthService;
using ASP.NET_Core_Car_Rental_System_Project_F.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;

    public AuthController(ApplicationDbContext context,
        JwtTokenGenerator jwtTokenGenerator, IMapper mapper, IAuthService authService)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginWriteDto dto)
    {
        var token = await _authService.Login(dto.Email, dto.Password);
        if (token == null)
            return Unauthorized("Invalid credentials.");

        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterWriteDto dto)
    {
        var user = await _authService.Register(dto);
        if (user == null)
            return Unauthorized("Invalid credentials.");

        return Ok(new
        {
            User = user
        });
    }
}