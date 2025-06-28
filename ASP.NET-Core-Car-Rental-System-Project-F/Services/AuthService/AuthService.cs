using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using ASP.NET_Core_Car_Rental_System_Project_F.Repositories.AuthRepository;
using ASP.NET_Core_Car_Rental_System_Project_F.Utils;
using AutoMapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;
    private readonly EmailSettings _emailSettings;

    public AuthService(IAuthRepository authRepository, JwtTokenGenerator jwtTokenGenerator, IMapper mapper,
        IOptions<EmailSettings> emailSettings)
    {
        _authRepository = authRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
        _emailSettings = emailSettings.Value;
    }

    public async Task<LoginReadDto?> LoginAsync(string email, string password)
    {
        var user = await _authRepository.GetUserByEmailAsync(email);

        if (user == null || !PasswordHasher.VerifyPassword(password, user.Password))
            return null;

        var token = _jwtTokenGenerator.GenerateToken(user.Email, user.Role);

        var loginReadDto = new LoginReadDto { Token = token, userId = user.UserId };

        return loginReadDto;
    }

    public async Task<UserReadDto?> RegisterAsync(RegisterWriteDto registerDto)
    {
        var isEmailExist = await _authRepository.EmailExistsAsync(registerDto.Email);
        if (isEmailExist)
            throw new Exception(CustomMessages.DuplicatedEmail);

        var user = _mapper.Map<User>(registerDto);

        await _authRepository.RegisterUserAsync(user);
        var userReadDto = _mapper.Map<UserReadDto>(user);

        return userReadDto;
    }

    public async Task<bool> SendOtpToEmailAsync(string email)
    {
        var user = await _authRepository.GetUserByEmailAsync(email);
        if (user == null) return false;

        var otp = OtpGenerator.GenerateOtp();
        var otpRecord = new OtpRecord
        {
            Email = email,
            Code = otp,
            Expiration = DateTime.Now.AddMinutes(Constants.OtpExpirationMinutes)
        };

        await _authRepository.SaveOtpAsync(otpRecord);
        await SendOtpAsync(email, otpRecord.Code);
        return true;
    }

    public async Task<bool> VerifyOtpAsync(string email, string otp)
    {
        var record = await _authRepository.GetOtpRecordAsync(email, otp);
        if (record == null || record.Expiration < DateTime.UtcNow) return false;

        await _authRepository.InvalidateOtpAsync(record);

        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordReadDto readDto)
    {
        var record = await _authRepository.GetOtpRecordAsync(readDto.Email, readDto.Otp);
        if (record == null || record.Expiration < DateTime.UtcNow)
            return false;

        var user = await _authRepository.GetUserByEmailAsync(readDto.Email);
        if (user == null)
            return false;

        await _authRepository.HashAndSavePasswordAsync(user, readDto.NewPassword);

        await _authRepository.RemoveAndSaveOtpAsync(record);

        return true;
    }

    private async Task SendOtpAsync(string toEmail, string otp)
    {
        var appPassword = Environment.GetEnvironmentVariable("APP_PASSWORD") ??
                          throw new InvalidOperationException(CustomMessages.UnSetAppPassword);
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = CustomMessages.YourOtpCode;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            { Text = $"<h3>Your OTP Code is: <b>{otp}</b></h3><p>This code expires in 5 minutes.</p>" };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port,
            SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailSettings.Username, appPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}