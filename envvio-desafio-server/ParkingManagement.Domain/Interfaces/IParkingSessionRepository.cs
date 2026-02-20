using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Domain.Interfaces;

public interface IParkingSessionRepository
{
    Task<ParkingSession?> GetByIdAsync(int id);
    Task<ParkingSession?> GetOpenSessionByVehicleIdAsync(int vehicleId);
    Task<IEnumerable<ParkingSession>> GetAllOpenSessionsAsync();
    Task<IEnumerable<ParkingSession>> GetSessionsByVehicleIdAsync(int vehicleId);
    Task<ParkingSession> AddAsync(ParkingSession session);
    Task UpdateAsync(ParkingSession session);
    Task<bool> HasOpenSessionAsync(int vehicleId);
}
