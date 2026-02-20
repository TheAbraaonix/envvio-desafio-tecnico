namespace ParkingManagement.Domain.Entities;

public class ParkingSession : BaseEntity
{
    public int VehicleId { get; private set; }
    public Vehicle? Vehicle { get; private set; }
    public DateTime EntryTime { get; private set; }
    public DateTime? ExitTime { get; private set; }
    public decimal? AmountCharged { get; private set; }
    public bool IsOpen => ExitTime == null;

    private ParkingSession() : base() { }

    public ParkingSession(int vehicleId) : base()
    {
        VehicleId = vehicleId;
        EntryTime = DateTime.UtcNow;
    }

    public void RegisterExit(decimal amountCharged)
    {
        if (!IsOpen)
            throw new InvalidOperationException("Session is already closed");

        if (amountCharged < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amountCharged));

        ExitTime = DateTime.UtcNow;
        AmountCharged = amountCharged;
        UpdatedAt = DateTime.UtcNow;
    }

    public TimeSpan GetDuration()
    {
        var endTime = ExitTime ?? DateTime.UtcNow;
        return endTime - EntryTime;
    }
}
