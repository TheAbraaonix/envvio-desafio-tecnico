using Microsoft.EntityFrameworkCore;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Interfaces;
using ParkingManagement.Infrastructure.Data;

namespace ParkingManagement.Infrastructure.Repositories;

public class ParkingSessionRepository : IParkingSessionRepository
{
    private readonly ApplicationDbContext _context;

    public ParkingSessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ParkingSession?> GetByIdAsync(int id)
    {
        return await _context.ParkingSessions
            .Include(ps => ps.Vehicle)
            .FirstOrDefaultAsync(ps => ps.Id == id);
    }

    public async Task<ParkingSession?> GetOpenSessionByVehicleIdAsync(int vehicleId)
    {
        return await _context.ParkingSessions
            .Include(ps => ps.Vehicle)
            .FirstOrDefaultAsync(ps => ps.VehicleId == vehicleId && ps.ExitTime == null);
    }

    public async Task<IEnumerable<ParkingSession>> GetAllOpenSessionsAsync()
    {
        return await _context.ParkingSessions
            .Include(ps => ps.Vehicle)
            .Where(ps => ps.ExitTime == null)
            .ToListAsync();
    }

    public async Task<IEnumerable<ParkingSession>> GetSessionsByVehicleIdAsync(int vehicleId)
    {
        return await _context.ParkingSessions
            .Include(ps => ps.Vehicle)
            .Where(ps => ps.VehicleId == vehicleId)
            .OrderByDescending(ps => ps.EntryTime)
            .ToListAsync();
    }

    public async Task<ParkingSession> AddAsync(ParkingSession session)
    {
        await _context.ParkingSessions.AddAsync(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task UpdateAsync(ParkingSession session)
    {
        _context.ParkingSessions.Update(session);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasOpenSessionAsync(int vehicleId)
    {
        return await _context.ParkingSessions
            .AnyAsync(ps => ps.VehicleId == vehicleId && ps.ExitTime == null);
    }
}
