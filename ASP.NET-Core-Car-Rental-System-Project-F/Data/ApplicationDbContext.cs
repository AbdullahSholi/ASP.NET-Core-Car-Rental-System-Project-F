using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<OtpRecord> OtpRecords => Set<OtpRecord>();
    public DbSet<BlacklistedToken> BlacklistedTokens => Set<BlacklistedToken>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<CarModel> CarModels => Set<CarModel>();
    public DbSet<CarBrand> CarBrands => Set<CarBrand>();
    
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<Car>()
            .Property(c => c.DailyRentalPrice)
            .HasPrecision(10, 2);
        modelBuilder.Entity<Reservation>()
            .Property(r => r.TotalPrice)
            .HasPrecision(10, 2);
    }
}