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

    public async Task<Vehicle> AddAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }

    public async Task UpdateAsync(Vehicle vehicle)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync();
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
