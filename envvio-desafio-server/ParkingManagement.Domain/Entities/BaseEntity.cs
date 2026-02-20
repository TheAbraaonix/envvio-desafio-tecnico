namespace ParkingManagement.Domain.Entities;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    protected BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }
}
