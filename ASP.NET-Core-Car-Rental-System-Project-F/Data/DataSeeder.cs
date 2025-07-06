using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using ASP.NET_Core_Car_Rental_System_Project_F.Models.Enums;
using ASP.NET_Core_Car_Rental_System_Project_F.Utils;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Data;

public static class DataSeeder
{
    public static void SeedDatabase(WebApplication app)
    {
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
                        DateOfBirth = new DateTime(2002, 08, 06), Address1 = "Asira", Address2 = "Asira",
                        City = "Nablus",
                        Country = "Palestine", DriverLicense = "None", Role = "Admin"
                    },
                    new User
                    {
                        FirstName = "Ahmed", LastName = "Sholi", Email = "groupgroup060@gmail.com",
                        Password = PasswordHasher.HashPassword("Sholi@971"), PhoneNumber = "+970592659066",
                        DateOfBirth = new DateTime(2002, 08, 06), Address1 = "Asira", Address2 = "Asira",
                        City = "Nablus",
                        Country = "Palestine", DriverLicense = "None", Role = "User"
                    }
                );
                db.SaveChanges();
            }

            if (!db.CarBrands.Any())
            {
                db.CarBrands.AddRange(
                    new CarBrand { Name = "Toyota" },
                    new CarBrand { Name = "Hyundai" }
                );
                db.SaveChanges();
            }

            if (!db.CarModels.Any())
            {
                var toyotaId = db.CarBrands.First(b => b.Name == "Toyota").CarBrandId;
                var hyundaiId = db.CarBrands.First(b => b.Name == "Hyundai").CarBrandId;

                db.CarModels.AddRange(
                    new CarModel { Name = "Corolla", CarBrandId = toyotaId },
                    new CarModel { Name = "Elantra", CarBrandId = hyundaiId }
                );
                db.SaveChanges();
            }

            if (!db.Cars.Any())
            {
                var corollaId = db.CarModels.First(m => m.Name == "Corolla").CarModelId;
                var elantraId = db.CarModels.First(m => m.Name == "Elantra").CarModelId;

                db.Cars.AddRange(
                    new Car
                    {
                        Year = 2020,
                        Color = CarColor.Blue,
                        FuelType = FuelType.Petrol,
                        TransmissionType = TransmissionType.Automatic,
                        SeatingCapacity = 5,
                        DailyRentalPrice = 150.00m,
                        AvailabilityStatus = true,
                        Description = "Reliable sedan with good fuel economy.",
                        CreatedAt = DateTime.Now,
                        CarModelId = corollaId
                    },
                    new Car
                    {
                        Year = 2021,
                        Color = CarColor.Black,
                        FuelType = FuelType.Diesel,
                        TransmissionType = TransmissionType.Manual,
                        SeatingCapacity = 4,
                        DailyRentalPrice = 140.00m,
                        AvailabilityStatus = true,
                        Description = "Comfortable car for daily use.",
                        CreatedAt = DateTime.Now,
                        CarModelId = elantraId
                    }
                );
                db.SaveChanges();
            }
        }
    }
}