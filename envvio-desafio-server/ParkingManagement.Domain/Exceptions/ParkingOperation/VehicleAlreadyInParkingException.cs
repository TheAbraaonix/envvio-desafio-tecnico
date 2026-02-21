using ParkingManagement.Domain.Exceptions.Base;

namespace ParkingManagement.Domain.Exceptions.ParkingOperation;

public class VehicleAlreadyInParkingException : ConflictException
{
    public VehicleAlreadyInParkingException(string plate) 
        : base($"Vehicle with plate {plate} already has an open parking session.")
    {
    }
}
