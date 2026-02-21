using Microsoft.AspNetCore.Mvc;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Interfaces;
using ParkingManagement.Domain.Enums;

namespace ParkingManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<VehicleDto>>>> GetAll(
        [FromQuery] int? page = null,
        [FromQuery] int? pageSize = null,
        [FromQuery] string? plate = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = null)
    {
        // If pagination params provided, return paginated results
        if (page.HasValue)
        {
            var paginationParams = new PaginationParams
            {
                Page = page.Value,
                PageSize = pageSize ?? 20,
                SortBy = sortBy,
                SortOrder = sortOrder ?? "asc"
            };

            var paginatedVehicles = await _vehicleService.GetVehiclesPaginatedAsync(paginationParams, plate);
            return Ok(ApiResponse<PaginatedResult<VehicleDto>>.SuccessResponse(paginatedVehicles, "Vehicles retrieved successfully"));
        }

        // Otherwise return all vehicles (backward compatibility)
        var vehicles = await _vehicleService.GetAllVehiclesAsync();
        return Ok(ApiResponse<IEnumerable<VehicleDto>>.SuccessResponse(vehicles, "Vehicles retrieved successfully"));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<VehicleDto>>> GetById(int id)
    {
        var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
        
        if (vehicle == null)
            return NotFound(ApiResponse<VehicleDto>.ErrorResponse(
                $"Vehicle with ID {id} not found", 
                ErrorCode.VEHICLE_NOT_FOUND, 
                404));

        return Ok(ApiResponse<VehicleDto>.SuccessResponse(vehicle, "Vehicle retrieved successfully"));
    }

    [HttpGet("plate/{plate}")]
    public async Task<ActionResult<ApiResponse<VehicleDto>>> GetByPlate(string plate)
    {
        var vehicle = await _vehicleService.GetVehicleByPlateAsync(plate);
        
        if (vehicle == null)
            return NotFound(ApiResponse<VehicleDto>.ErrorResponse(
                $"Vehicle with plate {plate} not found", 
                ErrorCode.VEHICLE_NOT_FOUND, 
                404));

        return Ok(ApiResponse<VehicleDto>.SuccessResponse(vehicle, "Vehicle retrieved successfully"));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<VehicleDto>>> Create([FromBody] CreateVehicleDto dto)
    {
        var vehicle = await _vehicleService.CreateVehicleAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, 
            ApiResponse<VehicleDto>.SuccessResponse(vehicle, "Vehicle created successfully", 201));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<VehicleDto>>> Update(int id, [FromBody] UpdateVehicleDto dto)
    {
        var vehicle = await _vehicleService.UpdateVehicleAsync(id, dto);
        return Ok(ApiResponse<VehicleDto>.SuccessResponse(vehicle, "Vehicle updated successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        await _vehicleService.DeleteVehicleAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(null!, "Vehicle deleted successfully"));
    }
}
