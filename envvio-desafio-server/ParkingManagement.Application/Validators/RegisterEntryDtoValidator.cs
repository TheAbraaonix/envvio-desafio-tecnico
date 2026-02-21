using FluentValidation;
using ParkingManagement.Application.DTOs;

namespace ParkingManagement.Application.Validators;

public class RegisterEntryDtoValidator : AbstractValidator<RegisterEntryDto>
{
    public RegisterEntryDtoValidator()
    {
        RuleFor(x => x.VehicleId)
            .GreaterThan(0).WithMessage("Vehicle ID must be greater than 0");
    }
}
