using AutoMapper;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Application.Mappings;

public class ParkingSessionMappingProfile : Profile
{
    public ParkingSessionMappingProfile()
    {
        CreateMap<ParkingSession, ParkingSessionDto>()
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.GetDuration()));
    }
}
