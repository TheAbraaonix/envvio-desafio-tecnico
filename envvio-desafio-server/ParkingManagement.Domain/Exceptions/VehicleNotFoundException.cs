namespace ParkingManagement.Domain.Exceptions;

public class VehicleNotFoundException : NotFoundException
{
    public VehicleNotFoundException(int vehicleId) 
        : base($"Vehicle with ID {vehicleId} was not found.")
    {
    }
}
