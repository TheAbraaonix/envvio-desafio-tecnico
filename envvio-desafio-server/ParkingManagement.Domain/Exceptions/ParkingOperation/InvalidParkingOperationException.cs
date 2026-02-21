using ParkingManagement.Domain.Exceptions.Base;

namespace ParkingManagement.Domain.Exceptions.ParkingOperation;

public class InvalidParkingOperationException : BadRequestException
{
    public InvalidParkingOperationException(string message) 
        : base(message)
    {
    }
}
