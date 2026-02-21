using ParkingManagement.Application.DTOs;

namespace ParkingManagement.Application.Interfaces;

public interface IParkingOperationService
{
    Task<ParkingSessionDto> RegisterEntryAsync(RegisterEntryDto dto);
    Task<ExitPreviewDto> PreviewExitByPlateAsync(string plate);
    Task<ParkingSessionDto> RegisterExitAsync(RegisterExitDto dto);
    Task<IEnumerable<ParkingSessionDto>> GetAllOpenSessionsAsync();
    Task<ParkingSessionDto?> GetSessionByIdAsync(int id);
}
