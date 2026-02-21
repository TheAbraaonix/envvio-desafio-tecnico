using ParkingManagement.Domain.Exceptions.Base;

namespace ParkingManagement.Domain.Exceptions.ParkingOperation;

public class SessionAlreadyClosedException : BadRequestException
{
    public SessionAlreadyClosedException() 
        : base("Session is already closed")
    {
    }
}
