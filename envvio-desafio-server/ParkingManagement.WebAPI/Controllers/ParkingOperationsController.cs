using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<IEnumerable<ParkingSessionDto>>> GetOpenSessions()
    {
        var sessions = await _parkingOperationService.GetAllOpenSessionsAsync();
        return Ok(sessions);
    }

    [HttpGet("sessions/{id}")]
    public async Task<ActionResult<ParkingSessionDto>> GetSessionById(int id)
    {
        var session = await _parkingOperationService.GetSessionByIdAsync(id);
        
        if (session == null)
            return NotFound(new { message = $"Session with ID {id} not found" });

        return Ok(session);
    }

    [HttpPost("entry")]
    public async Task<ActionResult<ParkingSessionDto>> RegisterEntry([FromBody] RegisterEntryDto dto)
    {
        try
        {
            var session = await _parkingOperationService.RegisterEntryAsync(dto);
            return CreatedAtAction(nameof(GetSessionById), new { id = session.Id }, session);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("exit-preview/{sessionId}")]
    public async Task<ActionResult<ExitPreviewDto>> PreviewExit(int sessionId)
    {
        try
        {
            var preview = await _parkingOperationService.PreviewExitAsync(sessionId);
            return Ok(preview);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("exit")]
    public async Task<ActionResult<ParkingSessionDto>> RegisterExit([FromBody] RegisterExitDto dto)
    {
        try
        {
            var session = await _parkingOperationService.RegisterExitAsync(dto);
            return Ok(session);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
