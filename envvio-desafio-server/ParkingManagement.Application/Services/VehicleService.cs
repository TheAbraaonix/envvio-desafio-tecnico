using AutoMapper;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Interfaces;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Interfaces;

namespace ParkingManagement.Application.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMapper _mapper;

    public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto)
    {
        if (await _vehicleRepository.ExistsByPlateAsync(dto.Plate))
            throw new InvalidParkingOperationException($"Vehicle with plate {dto.Plate} already exists");

        var vehicle = _mapper.Map<Vehicle>(dto);
        var created = await _vehicleRepository.AddAsync(vehicle);

        return _mapper.Map<VehicleDto>(created);
    }

    public async Task<VehicleDto> UpdateVehicleAsync(int id, UpdateVehicleDto dto)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id)
            ?? throw new VehicleNotFoundException(id);

        vehicle.UpdateDetails(dto.Model, dto.Color, dto.Type);
        await _vehicleRepository.UpdateAsync(vehicle);

        return _mapper.Map<VehicleDto>(vehicle);
    }

    public async Task<VehicleDto?> GetVehicleByIdAsync(int id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        return vehicle != null ? _mapper.Map<VehicleDto>(vehicle) : null;
    }

    public async Task<VehicleDto?> GetVehicleByPlateAsync(string plate)
    {
        var vehicle = await _vehicleRepository.GetByPlateAsync(plate);
        return vehicle != null ? _mapper.Map<VehicleDto>(vehicle) : null;
    }

    public async Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync()
    {
        var vehicles = await _vehicleRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
    }

    public async Task DeleteVehicleAsync(int id)
    {
        await _vehicleRepository.DeleteAsync(id);
    }
}
