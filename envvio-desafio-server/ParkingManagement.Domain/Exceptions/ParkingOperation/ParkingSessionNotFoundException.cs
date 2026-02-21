using ParkingManagement.Domain.Exceptions.Base;

namespace ParkingManagement.Domain.Exceptions.ParkingOperation;

public class ParkingSessionNotFoundException : NotFoundException
{
    public ParkingSessionNotFoundException(int sessionId) 
        : base($"Parking session with ID {sessionId} was not found.")
    {
    }

    public ParkingSessionNotFoundException(string message) 
        : base(message)
    {
    }
}
