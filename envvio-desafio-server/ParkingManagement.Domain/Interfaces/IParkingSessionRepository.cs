using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Domain.Interfaces;

public interface IParkingSessionRepository
{
    Task<ParkingSession?> GetByIdAsync(int id);
    Task<ParkingSession?> GetOpenSessionByVehicleIdAsync(int vehicleId);
    Task<IEnumerable<ParkingSession>> GetAllOpenSessionsAsync();
    Task<IEnumerable<ParkingSession>> GetAllOpenSessionsAsync(string? plateFilter = null);
    Task<(IEnumerable<ParkingSession> Sessions, int TotalCount)> GetAllOpenSessionsPaginatedAsync(
        int skip, 
        int take, 
        string? plateFilter = null, 
        string? sortBy = null, 
        string sortOrder = "asc");
    Task<IEnumerable<ParkingSession>> GetSessionsByVehicleIdAsync(int vehicleId);
    Task<ParkingSession> AddAsync(ParkingSession session);
    Task UpdateAsync(ParkingSession session);
    Task<bool> HasOpenSessionAsync(int vehicleId);
}
