namespace ParkingManagement.Domain.Exceptions;

public class VehicleAlreadyInParkingException : ConflictException
{
    public VehicleAlreadyInParkingException(string plate) 
        : base($"Vehicle with plate {plate} already has an open parking session.")
    {
    }
}
