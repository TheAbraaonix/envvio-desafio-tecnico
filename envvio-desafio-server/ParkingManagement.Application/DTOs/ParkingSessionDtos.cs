namespace ParkingManagement.Application.DTOs;

public record ParkingSessionDto(
    int Id,
    int VehicleId,
    VehicleDto? Vehicle,
    DateTime EntryTime,
    DateTime? ExitTime,
    decimal? AmountCharged,
    bool IsOpen,
    TimeSpan? Duration
);

public record RegisterEntryDto(
    string Plate
);

public record RegisterExitDto(
    string Plate
);

public record ExitPreviewDto(
    int SessionId,
    DateTime EntryTime,
    DateTime ExitTime,
    TimeSpan Duration,
    decimal AmountCharged,
    VehicleDto? Vehicle
);
