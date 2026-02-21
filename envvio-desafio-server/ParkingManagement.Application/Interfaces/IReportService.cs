using ParkingManagement.Application.DTOs;

namespace ParkingManagement.Application.Interfaces;

public interface IReportService
{
    Task<IEnumerable<RevenueByDayDto>> GetRevenueByDayAsync(int days);
    Task<IEnumerable<VehicleParkingTimeDto>> GetTopVehiclesByParkingTimeAsync(DateTime startDate, DateTime endDate, int topCount = 10);
    Task<IEnumerable<OccupancyByHourDto>> GetOccupancyByHourAsync(DateTime startDate, DateTime endDate);
}
