using ASP.NET_Core_Car_Rental_System_Project_F.Data;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Repositories.ReservationRepository;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation?> BookCarAsync(Reservation reservation)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var car = await _context.Cars
                .FromSqlInterpolated($"SELECT * FROM Cars WITH (UPDLOCK) WHERE CarId = {reservation.CarId}")
                .FirstOrDefaultAsync();

            if (car == null)
                return null;

            var isOverlapping = await _context.Reservations.AnyAsync(r =>
                r.CarId == reservation.CarId &&
                (
                    (reservation.StartDate >= r.StartDate && reservation.EndDate <= r.EndDate) ||
                    (reservation.EndDate > r.StartDate && reservation.EndDate <= r.EndDate) ||
                    (reservation.StartDate <= r.StartDate && reservation.EndDate >= r.EndDate)
                )
            );

            if (isOverlapping)
                return null;

            var totalDays = (reservation.EndDate - reservation.StartDate).Days;
            reservation.TotalPrice = car.DailyRentalPrice * totalDays;

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return reservation;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task RemoveReservationAsync(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        _context.Reservations.Remove(reservation);

        await _context.SaveChangesAsync();
    }
}