using Microsoft.AspNetCore.Mvc;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Interfaces;

namespace ParkingManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingOperationsController : ControllerBase
{
    private readonly IParkingOperationService _parkingOperationService;

    public ParkingOperationsController(IParkingOperationService parkingOperationService)
    {
        _parkingOperationService = parkingOperationService;
    }

    [HttpGet("open-sessions")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ParkingSessionDto>>>> GetOpenSessions()
    {
        var sessions = await _parkingOperationService.GetAllOpenSessionsAsync();
        return Ok(ApiResponse<IEnumerable<ParkingSessionDto>>.SuccessResponse(sessions, "Open sessions retrieved successfully"));
    }

    [HttpGet("sessions/{id}")]
    public async Task<ActionResult<ApiResponse<ParkingSessionDto>>> GetSessionById(int id)
    {
        var session = await _parkingOperationService.GetSessionByIdAsync(id);
        
        if (session == null)
            return NotFound(ApiResponse<ParkingSessionDto>.ErrorResponse($"Session with ID {id} not found", 404));

        return Ok(ApiResponse<ParkingSessionDto>.SuccessResponse(session, "Session retrieved successfully"));
    }

    [HttpPost("entry")]
    public async Task<ActionResult<ApiResponse<ParkingSessionDto>>> RegisterEntry([FromBody] RegisterEntryDto dto)
    {
        var session = await _parkingOperationService.RegisterEntryAsync(dto);
        return CreatedAtAction(nameof(GetSessionById), new { id = session.Id }, 
            ApiResponse<ParkingSessionDto>.SuccessResponse(session, "Vehicle entry registered successfully", 201));
    }

    [HttpGet("exit-preview/{sessionId}")]
    public async Task<ActionResult<ApiResponse<ExitPreviewDto>>> PreviewExit(int sessionId)
    {
        var preview = await _parkingOperationService.PreviewExitAsync(sessionId);
        return Ok(ApiResponse<ExitPreviewDto>.SuccessResponse(preview, "Exit preview generated successfully"));
    }

    [HttpPost("exit")]
    public async Task<ActionResult<ApiResponse<ParkingSessionDto>>> RegisterExit([FromBody] RegisterExitDto dto)
    {
        var session = await _parkingOperationService.RegisterExitAsync(dto);
        return Ok(ApiResponse<ParkingSessionDto>.SuccessResponse(session, "Vehicle exit registered successfully"));
    }
}
