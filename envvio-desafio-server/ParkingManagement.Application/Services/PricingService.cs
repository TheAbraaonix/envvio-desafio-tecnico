using ParkingManagement.Application.Interfaces;

namespace ParkingManagement.Application.Services;

public class PricingService : IPricingService
{
    private const decimal FirstHourRate = 5.00m;
    private const decimal AdditionalHourRate = 3.00m;
    private const int GracePeriodMinutes = 15;

    public decimal CalculateParkingFee(TimeSpan duration)
    {
        // Negative duration should return 0
        if (duration.TotalMinutes <= 0)
            return 0;

        // Grace period: 15 minutes or less = free
        if (duration.TotalMinutes <= GracePeriodMinutes)
            return 0;

        // Above 15 minutes up to 1 hour = first hour rate
        if (duration.TotalHours <= 1)
            return FirstHourRate;

        // Above 1 hour = first hour + proportional additional hours
        var additionalHours = duration.TotalHours - 1;
        var additionalCost = (decimal)additionalHours * AdditionalHourRate;

        return FirstHourRate + additionalCost;
    }
}
