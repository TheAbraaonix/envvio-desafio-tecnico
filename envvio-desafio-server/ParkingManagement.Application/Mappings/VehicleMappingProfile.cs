using AutoMapper;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Application.Mappings;

public class VehicleMappingProfile : Profile
{
    public VehicleMappingProfile()
    {
        CreateMap<Vehicle, VehicleDto>();
        
        CreateMap<CreateVehicleDto, Vehicle>()
            .ConstructUsing(dto => new Vehicle(dto.Plate, dto.Type, dto.Model, dto.Color));
    }
}
