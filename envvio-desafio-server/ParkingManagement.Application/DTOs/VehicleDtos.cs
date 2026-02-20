using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Application.DTOs;

public record VehicleDto(
    int Id,
    string Plate,
    string? Model,
    string? Color,
    VehicleType Type,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateVehicleDto(
    string Plate,
    string? Model,
    string? Color,
    VehicleType Type
);

public record UpdateVehicleDto(
    string? Model,
    string? Color,
    VehicleType Type
);
