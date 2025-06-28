using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.QueryDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using ASP.NET_Core_Car_Rental_System_Project_F.Repositories.ReservationRepository;
using AutoMapper;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Services.ReservationService;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;

    public ReservationService(IReservationRepository reservationRepository, IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _mapper = mapper;
    }

    public async Task<ReservationReadDto?> BookCarAsync(ReservationWriteDto dto)
    {
        var reservation = _mapper.Map<Reservation>(dto);
        await _reservationRepository.BookCarAsync(reservation);
        var reservationReadDto = _mapper.Map<ReservationReadDto>(reservation);
        reservationReadDto.TotalPrice = reservation.TotalPrice;
        reservationReadDto.ReservationId = reservation.ReservationId;

        return reservationReadDto;
    }

    public async Task RemoveReservationAsync(int id)
    {
        await _reservationRepository.RemoveReservationAsync(id);
    }

    public async Task<List<ReservationReadDto?>> GetReservationsAsync(int id)
    {
        var reservations = await _reservationRepository.GetReservationsAsync(id);
        var reservationsReadDto = _mapper.Map<List<ReservationReadDto>>(reservations);
        
        return reservationsReadDto;
    }
}