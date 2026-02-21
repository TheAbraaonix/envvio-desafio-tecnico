using Microsoft.EntityFrameworkCore;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Interfaces;
using ParkingManagement.Infrastructure.Data;

namespace ParkingManagement.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationDbContext _context;

    public VehicleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Vehicle?> GetByIdAsync(int id)
    {
        return await _context.Vehicles.FindAsync(id);
    }

    public async Task<Vehicle?> GetByPlateAsync(string plate)
    {
        return await _context.Vehicles
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Plate == plate.ToUpperInvariant());
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync()
    {
        return await _context.Vehicles
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<(IEnumerable<Vehicle> Vehicles, int TotalCount)> GetAllPaginatedAsync(
        int skip,
        int take,
        string? plateFilter = null,
        string? sortBy = null,
        string sortOrder = "asc")
    {
        var query = _context.Vehicles.AsNoTracking();

        // Apply plate filter
        if (!string.IsNullOrWhiteSpace(plateFilter))
        {
            var normalizedFilter = plateFilter.ToUpperInvariant().Trim();
            query = query.Where(v => v.Plate.Contains(normalizedFilter));
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply sorting
        query = ApplySorting(query, sortBy, sortOrder);

        // Apply pagination
        var vehicles = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return (vehicles, totalCount);
    }

    private IQueryable<Vehicle> ApplySorting(IQueryable<Vehicle> query, string? sortBy, string sortOrder)
    {
        var isDescending = sortOrder.ToLower() == "desc";

        return sortBy?.ToLower() switch
        {
            "plate" => isDescending
                ? query.OrderByDescending(v => v.Plate)
                : query.OrderBy(v => v.Plate),
            "model" => isDescending
                ? query.OrderByDescending(v => v.Model)
                : query.OrderBy(v => v.Model),
            "color" => isDescending
                ? query.OrderByDescending(v => v.Color)
                : query.OrderBy(v => v.Color),
            "type" => isDescending
                ? query.OrderByDescending(v => v.Type)
                : query.OrderBy(v => v.Type),
            "createdat" => isDescending
                ? query.OrderByDescending(v => v.CreatedAt)
                : query.OrderBy(v => v.CreatedAt),
            _ => query.OrderBy(v => v.Plate) // Default: alphabetical by plate
        };
    }

    public async Task<Vehicle> AddAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }

    public async Task<Vehicle> UpdateAsync(Vehicle vehicle)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }

    public async Task DeleteAsync(int id)
    {
        var vehicle = await GetByIdAsync(id);
        if (vehicle != null)
        {
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByPlateAsync(string plate, int? excludeId = null)
    {
        var query = _context.Vehicles
            .AsNoTracking()
            .Where(v => v.Plate == plate.ToUpperInvariant());
        
        if (excludeId.HasValue)
        {
            query = query.Where(v => v.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}
