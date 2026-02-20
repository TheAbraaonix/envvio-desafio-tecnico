using ParkingManagement.Application.Interfaces;

namespace ParkingManagement.Application.Services;

public class PricingService : IPricingService
{
    private const decimal FirstHourRate = 5.00m;
    private const decimal AdditionalHourRate = 3.00m;

    public decimal CalculateParkingFee(TimeSpan duration)
    {
        if (duration.TotalHours <= 0)
            return 0;

        if (duration.TotalHours <= 1)
            return FirstHourRate;

        var additionalHours = duration.TotalHours - 1;
        var additionalCost = (decimal)additionalHours * AdditionalHourRate;

        return FirstHourRate + additionalCost;
    }
}
