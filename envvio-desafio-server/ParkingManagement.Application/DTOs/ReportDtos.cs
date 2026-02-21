namespace ParkingManagement.Application.DTOs;

public record RevenueByDayDto(
    DateTime Date,
    decimal TotalRevenue,
    int SessionCount
);

public record VehicleParkingTimeDto(
    int VehicleId,
    string Plate,
    string? Model,
    TimeSpan TotalParkingTime,
    int SessionCount
);

public record OccupancyByHourDto(
    DateTime Hour,
    int VehicleCount
);
