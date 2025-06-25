using System.Security.Claims;
using System.Text;
using ASP.NET_Core_Car_Rental_System_Project_F.AutoMapper;
using ASP.NET_Core_Car_Rental_System_Project_F.Data;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using ASP.NET_Core_Car_Rental_System_Project_F.Repositories.AuthRepository;
using ASP.NET_Core_Car_Rental_System_Project_F.Repositories.CarRepository;
using ASP.NET_Core_Car_Rental_System_Project_F.Repositories.TokenBlacklistedRepository;
using ASP.NET_Core_Car_Rental_System_Project_F.Services.AuthService;
using ASP.NET_Core_Car_Rental_System_Project_F.Services.CarService;
using ASP.NET_Core_Car_Rental_System_Project_F.Services.TokenBlacklistService;
using ASP.NET_Core_Car_Rental_System_Project_F.Utils;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddOpenApi();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenBlacklistedRepository, TokenBlacklistedRepository>();
builder.Services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

var jwtSettingsSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);
var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY") ??
                throw new InvalidOperationException(CustomMessages.UnSetSecretKey);
var connectionString = Environment.GetEnvironmentVariable("CAR_RENTAL_CONNECTION_STRING") ??
                       throw new InvalidOperationException(CustomMessages.UnSetConnectionString);

builder.Services.AddSingleton<JwtTokenGenerator>(
    sp =>
    {
        var jwtSettings = sp.GetRequiredService<IOptions<JwtSettings>>();
        return new JwtTokenGenerator(jwtSettings, secretKey);
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async (context) =>
        {
            var jti = context.Principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            var blacklistService = context.HttpContext.RequestServices.GetService<ITokenBlacklistService>();

            if (jti != null && await blacklistService.IsTokenBlacklistedAsync(jti))
                context.Fail(CustomMessages.TokenIsBlacklisted);
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

var app = builder.Build();

DataSeeder.SeedDatabase(app);

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors();
app.UseHsts();
app.UseXContentTypeOptions();
app.UseReferrerPolicy(opts => opts.NoReferrer());
app.UseXXssProtection(options => options.EnabledWithBlockMode());
app.UseXfo(options => options.Deny());
app.UseIpRateLimiting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();