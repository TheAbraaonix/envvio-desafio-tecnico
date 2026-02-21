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
            .AsNoTracking()
            .Include(ps => ps.Vehicle)
            .FirstOrDefaultAsync(ps => ps.VehicleId == vehicleId && ps.ExitTime == null);
    }

    public async Task<IEnumerable<ParkingSession>> GetAllOpenSessionsAsync()
    {
        return await _context.ParkingSessions
            .AsNoTracking()
            .Include(ps => ps.Vehicle)
            .Where(ps => ps.ExitTime == null)
            .ToListAsync();
    }

    public async Task<IEnumerable<ParkingSession>> GetAllOpenSessionsAsync(string? plateFilter = null)
    {
        var query = _context.ParkingSessions
            .AsNoTracking()
            .Include(ps => ps.Vehicle)
            .Where(ps => ps.ExitTime == null);

        if (!string.IsNullOrWhiteSpace(plateFilter))
        {
            var normalizedFilter = plateFilter.ToUpperInvariant().Trim();
            query = query.Where(ps => ps.Vehicle!.Plate.Contains(normalizedFilter));
        }

        return await query
            .OrderBy(ps => ps.EntryTime) // Oldest first = Longest duration first
            .ToListAsync();
    }

    public async Task<(IEnumerable<ParkingSession> Sessions, int TotalCount)> GetAllOpenSessionsPaginatedAsync(
        int skip,
        int take,
        string? plateFilter = null,
        string? sortBy = null,
        string sortOrder = "asc")
    {
        var query = _context.ParkingSessions
            .AsNoTracking()
            .Include(ps => ps.Vehicle)
            .Where(ps => ps.ExitTime == null);

        // Apply plate filter
        if (!string.IsNullOrWhiteSpace(plateFilter))
        {
            var normalizedFilter = plateFilter.ToUpperInvariant().Trim();
            query = query.Where(ps => ps.Vehicle!.Plate.Contains(normalizedFilter));
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply sorting
        query = ApplySorting(query, sortBy, sortOrder);

        // Apply pagination
        var sessions = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return (sessions, totalCount);
    }

    private IQueryable<ParkingSession> ApplySorting(IQueryable<ParkingSession> query, string? sortBy, string sortOrder)
    {
        var isDescending = sortOrder.ToLower() == "desc";

        return sortBy?.ToLower() switch
        {
            "plate" => isDescending 
                ? query.OrderByDescending(ps => ps.Vehicle!.Plate)
                : query.OrderBy(ps => ps.Vehicle!.Plate),
            "entrytime" => isDescending
                ? query.OrderByDescending(ps => ps.EntryTime)
                : query.OrderBy(ps => ps.EntryTime),
            "model" => isDescending
                ? query.OrderByDescending(ps => ps.Vehicle!.Model)
                : query.OrderBy(ps => ps.Vehicle!.Model),
            "type" => isDescending
                ? query.OrderByDescending(ps => ps.Vehicle!.Type)
                : query.OrderBy(ps => ps.Vehicle!.Type),
            _ => query.OrderBy(ps => ps.EntryTime) // Default: oldest first
        };
    }

    public async Task<IEnumerable<ParkingSession>> GetSessionsByVehicleIdAsync(int vehicleId)
    {
        return await _context.ParkingSessions
            .AsNoTracking()
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
            .AsNoTracking()
            .AnyAsync(ps => ps.VehicleId == vehicleId && ps.ExitTime == null);
    }
}
