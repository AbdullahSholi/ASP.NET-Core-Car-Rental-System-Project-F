using ASP.NET_Core_Car_Rental_System_Project_F.Models;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Repositories.ReservationRepository;

public interface IReservationRepository
{
    public Task<Reservation?> BookCarAsync(Reservation reservation);
    public Task RemoveReservationAsync(int id);
}