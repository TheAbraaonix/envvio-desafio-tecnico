using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Domain.Entities;

public class Vehicle : BaseEntity
{
    public string Plate { get; private set; } = string.Empty;
    public string? Model { get; private set; }
    public string? Color { get; private set; }
    public VehicleType Type { get; private set; }

    private Vehicle() : base() { }

    public Vehicle(string plate, VehicleType type, string? model = null, string? color = null) : base()
    {
        if (string.IsNullOrWhiteSpace(plate))
            throw new ArgumentException("Plate is required", nameof(plate));

        Plate = plate.ToUpperInvariant().Trim();
        Type = type;
        Model = model?.Trim();
        Color = color?.Trim();
    }

    public void UpdateDetails(string? model, string? color, VehicleType type)
    {
        Model = model?.Trim();
        Color = color?.Trim();
        Type = type;
        UpdatedAt = DateTime.UtcNow;
    }
}
