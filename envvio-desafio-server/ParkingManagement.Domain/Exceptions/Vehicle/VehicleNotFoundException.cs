using ParkingManagement.Domain.Exceptions.Base;

namespace ParkingManagement.Domain.Exceptions.Vehicle;

public class VehicleNotFoundException : NotFoundException
{
    public VehicleNotFoundException(int vehicleId) 
        : base($"Vehicle with ID {vehicleId} was not found.")
    {
    }

    public VehicleNotFoundException(string message) 
        : base(message)
    {
    }
}
