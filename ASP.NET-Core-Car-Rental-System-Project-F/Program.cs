using System.Security.Claims;
using System.Text;
using ASP.NET_Core_Car_Rental_System_Project_F.AutoMapper;
using ASP.NET_Core_Car_Rental_System_Project_F.Data;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using ASP.NET_Core_Car_Rental_System_Project_F.Repositories.AuthRepository;
using ASP.NET_Core_Car_Rental_System_Project_F.Services.AuthService;
using ASP.NET_Core_Car_Rental_System_Project_F.Utils;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddOpenApi();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

var jwtSettingsSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);
var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY") ??
                throw new InvalidOperationException(CustomMessages.UnSetSecretKey);

builder.Services.AddSingleton<JwtTokenGenerator>(
    sp =>
    {
        var jwtSettings = sp.GetRequiredService<IOptions<JwtSettings>>();
        return new JwtTokenGenerator(jwtSettings, secretKey);
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User
            {
                FirstName = "Abdullah", LastName = "Sholi", Email = "abdullah.ghassan.sholi@gmail.com",
                Password = PasswordHasher.HashPassword("Sholi@971"), PhoneNumber = "+970592659066",
                DateOfBirth = new DateTime(2002, 08, 06), Address1 = "Asira", Address2 = "Asira", City = "Nablus",
                Country = "Palestine", DriverLicense = "None", Role = "Admin"
            },
            new User
            {
                FirstName = "Ahmed", LastName = "Sholi", Email = "groupgroup060@gmail.com",
                Password = PasswordHasher.HashPassword("Sholi@971"), PhoneNumber = "+970592659066",
                DateOfBirth = new DateTime(2002, 08, 06), Address1 = "Asira", Address2 = "Asira", City = "Nablus",
                Country = "Palestine", DriverLicense = "None", Role = "User"
            }
        );
        db.SaveChanges();
    }
}

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