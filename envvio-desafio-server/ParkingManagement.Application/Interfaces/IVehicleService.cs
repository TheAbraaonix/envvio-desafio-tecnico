using ParkingManagement.Application.DTOs;

namespace ParkingManagement.Application.Interfaces;

public interface IVehicleService
{
    Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto);
    Task<VehicleDto> UpdateVehicleAsync(int id, UpdateVehicleDto dto);
    Task<VehicleDto?> GetVehicleByIdAsync(int id);
    Task<VehicleDto?> GetVehicleByPlateAsync(string plate);
    Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync();
    Task DeleteVehicleAsync(int id);
}
