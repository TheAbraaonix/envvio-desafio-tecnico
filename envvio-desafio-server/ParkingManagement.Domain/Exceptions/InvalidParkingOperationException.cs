namespace ParkingManagement.Domain.Exceptions;

public class InvalidParkingOperationException : BadRequestException
{
    public InvalidParkingOperationException(string message) 
        : base(message)
    {
    }
}
