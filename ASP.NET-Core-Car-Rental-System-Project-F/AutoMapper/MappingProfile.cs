using ASP.NET_Core_Car_Rental_System_Project_F.Dtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.ReadDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Dtos.WriteDtos;
using ASP.NET_Core_Car_Rental_System_Project_F.Models;
using AutoMapper;

namespace ASP.NET_Core_Car_Rental_System_Project_F.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterWriteDto, User>();
        CreateMap<LoginWriteDto, User>();
        CreateMap<User, UserReadDto>();
    }
}