using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Domain.Interfaces;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(int id);
    Task<Vehicle?> GetByPlateAsync(string plate);
    Task<IEnumerable<Vehicle>> GetAllAsync();
    Task<(IEnumerable<Vehicle> Vehicles, int TotalCount)> GetAllPaginatedAsync(
        int skip, 
        int take, 
        string? plateFilter = null,
        string? sortBy = null,
        string sortOrder = "asc");
    Task<Vehicle> AddAsync(Vehicle vehicle);
    Task<Vehicle> UpdateAsync(Vehicle vehicle);
    Task DeleteAsync(int id);
    Task<bool> ExistsByPlateAsync(string plate, int? excludeId = null);
}
