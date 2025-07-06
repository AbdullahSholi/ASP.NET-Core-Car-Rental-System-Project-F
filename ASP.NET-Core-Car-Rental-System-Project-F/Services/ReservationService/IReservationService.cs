using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.QueryDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Services.ReservationService;

public interface IReservationService
{
    public Task<ReservationReadDto?> BookCarAsync(ReservationWriteDto dto);
    public Task RemoveReservationAsync(int id);
    public Task<List<ReservationReadDto?>> GetReservationsAsync(int id);
}