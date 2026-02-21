using Microsoft.AspNetCore.Mvc;
using ParkingManagement.Application.Common;
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
    public async Task<ActionResult<ApiResponse<IEnumerable<RevenueByDayDto>>>> GetRevenueByDay([FromQuery] int days = 7)
    {
        if (days <= 0 || days > 365)
            return BadRequest(ApiResponse<IEnumerable<RevenueByDayDto>>.ErrorResponse("Days must be between 1 and 365"));

        var revenue = await _reportService.GetRevenueByDayAsync(days);
        return Ok(ApiResponse<IEnumerable<RevenueByDayDto>>.SuccessResponse(revenue, "Revenue report generated successfully"));
    }

    [HttpGet("top-vehicles-by-parking-time")]
    public async Task<ActionResult<ApiResponse<IEnumerable<VehicleParkingTimeDto>>>> GetTopVehiclesByParkingTime(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int top = 10)
    {
        if (top <= 0 || top > 100)
            return BadRequest(ApiResponse<IEnumerable<VehicleParkingTimeDto>>.ErrorResponse("Top must be between 1 and 100"));

        var start = startDate ?? DateTime.UtcNow.AddDays(-30);
        var end = endDate ?? DateTime.UtcNow;

        if (start > end)
            return BadRequest(ApiResponse<IEnumerable<VehicleParkingTimeDto>>.ErrorResponse("Start date must be before end date"));

        var topVehicles = await _reportService.GetTopVehiclesByParkingTimeAsync(start, end, top);
        return Ok(ApiResponse<IEnumerable<VehicleParkingTimeDto>>.SuccessResponse(topVehicles, "Top vehicles report generated successfully"));
    }

    [HttpGet("occupancy-by-hour")]
    public async Task<ActionResult<ApiResponse<IEnumerable<OccupancyByHourDto>>>> GetOccupancyByHour(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(-1).Date;
        var end = endDate ?? DateTime.UtcNow;

        if (start > end)
            return BadRequest(ApiResponse<IEnumerable<OccupancyByHourDto>>.ErrorResponse("Start date must be before end date"));

        var diffInDays = (end - start).TotalDays;
        if (diffInDays > 31)
            return BadRequest(ApiResponse<IEnumerable<OccupancyByHourDto>>.ErrorResponse("Date range cannot exceed 31 days"));

        var occupancy = await _reportService.GetOccupancyByHourAsync(start, end);
        return Ok(ApiResponse<IEnumerable<OccupancyByHourDto>>.SuccessResponse(occupancy, "Occupancy report generated successfully"));
    }
}
