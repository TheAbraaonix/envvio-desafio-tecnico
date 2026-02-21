using Microsoft.EntityFrameworkCore;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Interfaces;
using ParkingManagement.Infrastructure.Data;

namespace ParkingManagement.Infrastructure.Repositories;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RevenueByDayDto>> GetRevenueByDayAsync(int days)
    {
        var startDate = DateTime.UtcNow.Date.AddDays(-days);

        var revenueByDay = await _context.ParkingSessions
            .Where(ps => ps.ExitTime != null && ps.ExitTime >= startDate)
            .GroupBy(ps => ps.ExitTime!.Value.Date)
            .Select(g => new RevenueByDayDto(
                g.Key,
                g.Sum(ps => ps.AmountCharged ?? 0),
                g.Count()
            ))
            .OrderBy(r => r.Date)
            .ToListAsync();

        return revenueByDay;
    }

    public async Task<IEnumerable<VehicleParkingTimeDto>> GetTopVehiclesByParkingTimeAsync(
        DateTime startDate, 
        DateTime endDate, 
        int topCount = 10)
    {
        var topVehicles = await _context.ParkingSessions
            .Include(ps => ps.Vehicle)
            .Where(ps => ps.ExitTime != null && 
                        ps.EntryTime >= startDate && 
                        ps.ExitTime <= endDate)
            .GroupBy(ps => new { ps.VehicleId, ps.Vehicle!.Plate, ps.Vehicle.Model })
            .Select(g => new VehicleParkingTimeDto(
                g.Key.VehicleId,
                g.Key.Plate,
                g.Key.Model,
                TimeSpan.FromTicks(g.Sum(ps => (ps.ExitTime!.Value - ps.EntryTime).Ticks)),
                g.Count()
            ))
            .OrderByDescending(v => v.TotalParkingTime)
            .Take(topCount)
            .ToListAsync();

        return topVehicles;
    }

    public async Task<IEnumerable<OccupancyByHourDto>> GetOccupancyByHourAsync(
        DateTime startDate, 
        DateTime endDate)
    {
        var occupancyData = new List<OccupancyByHourDto>();

        // Round start to the beginning of the hour
        var currentHour = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, 0, 0);
        var endHour = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, 0, 0);

        // Get all sessions that overlap with the date range
        var sessions = await _context.ParkingSessions
            .Where(ps => ps.EntryTime <= endDate && (ps.ExitTime == null || ps.ExitTime >= startDate))
            .ToListAsync();

        // For each hour in the range, count how many vehicles were in the parking lot
        while (currentHour <= endHour)
        {
            var nextHour = currentHour.AddHours(1);
            
            var count = sessions.Count(ps => 
                ps.EntryTime < nextHour && 
                (ps.ExitTime == null || ps.ExitTime >= currentHour));

            occupancyData.Add(new OccupancyByHourDto(currentHour, count));
            
            currentHour = nextHour;
        }

        return occupancyData;
    }
}
