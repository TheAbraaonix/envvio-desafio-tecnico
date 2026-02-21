using ParkingManagement.Domain.Exceptions.Base;

namespace ParkingManagement.Domain.Exceptions.Vehicle;

public class VehicleAlreadyExistsException : ConflictException
{
    public VehicleAlreadyExistsException(string plate) 
        : base($"Vehicle with plate {plate} already exists")
    {
    }
}
