namespace ParkingManagement.Application.Interfaces;

public interface IPricingService
{
    decimal CalculateParkingFee(TimeSpan duration);
}
