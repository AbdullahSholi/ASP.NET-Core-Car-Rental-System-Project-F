using ASP.NET_Core_Car_Rental_System_Project_F.Auth;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Repositories.AuthRepository;
using ASP.NET_Core_Car_Rental_System_Project_F.Services.AuthService;
using ASP.NET_Core_Car_Rental_System_Project_F.Utils;
using AutoMapper;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Repository;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;

    public AuthService(IAuthRepository authRepository, JwtTokenGenerator jwtTokenGenerator, IMapper mapper)
    {
        _authRepository = authRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
    }

    public async Task<string?> Login(string email, string password)
    {
        var user = await _authRepository.GetUserByEmailAsync(email);

        if (user == null || !PasswordHasher.VerifyPassword(password, user.Password))
            return null;

        var token = _jwtTokenGenerator.GenerateToken(user.Email, user.Role);

        return token;
    }

    public async Task<UserReadDto?> Register(RegisterWriteDto registerDto)
    {
        var user = _mapper.Map<User>(registerDto);
        if (user == null)
            return null;
        
        await _authRepository.Register(user);
        var userReadDto = _mapper.Map<UserReadDto>(user);

        return userReadDto;
    }
}