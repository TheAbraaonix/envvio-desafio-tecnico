namespace ParkingManagement.Domain.Exceptions;

public class ParkingSessionNotFoundException : NotFoundException
{
    public ParkingSessionNotFoundException(int sessionId) 
        : base($"Parking session with ID {sessionId} was not found.")
    {
    }
}
