using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Domain.Interfaces;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(int id);
    Task<Vehicle?> GetByPlateAsync(string plate);
    Task<IEnumerable<Vehicle>> GetAllAsync();
    Task<Vehicle> AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
    Task DeleteAsync(int id);
    Task<bool> ExistsByPlateAsync(string plate, int? excludeId = null);
}
