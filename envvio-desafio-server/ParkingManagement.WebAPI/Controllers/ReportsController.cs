using Microsoft.AspNetCore.Mvc;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Interfaces;

namespace ParkingManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("revenue-by-day")]
    public async Task<ActionResult<IEnumerable<RevenueByDayDto>>> GetRevenueByDay([FromQuery] int days = 7)
    {
        if (days <= 0 || days > 365)
            return BadRequest(new { message = "Days must be between 1 and 365" });

        var revenue = await _reportService.GetRevenueByDayAsync(days);
        return Ok(revenue);
    }

    [HttpGet("top-vehicles-by-parking-time")]
    public async Task<ActionResult<IEnumerable<VehicleParkingTimeDto>>> GetTopVehiclesByParkingTime(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int top = 10)
    {
        if (top <= 0 || top > 100)
            return BadRequest(new { message = "Top must be between 1 and 100" });

        var start = startDate ?? DateTime.UtcNow.AddDays(-30);
        var end = endDate ?? DateTime.UtcNow;

        if (start > end)
            return BadRequest(new { message = "Start date must be before end date" });

        var topVehicles = await _reportService.GetTopVehiclesByParkingTimeAsync(start, end, top);
        return Ok(topVehicles);
    }

    [HttpGet("occupancy-by-hour")]
    public async Task<ActionResult<IEnumerable<OccupancyByHourDto>>> GetOccupancyByHour(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(-1).Date;
        var end = endDate ?? DateTime.UtcNow;

        if (start > end)
            return BadRequest(new { message = "Start date must be before end date" });

        var diffInDays = (end - start).TotalDays;
        if (diffInDays > 31)
            return BadRequest(new { message = "Date range cannot exceed 31 days" });

        var occupancy = await _reportService.GetOccupancyByHourAsync(start, end);
        return Ok(occupancy);
    }
}
