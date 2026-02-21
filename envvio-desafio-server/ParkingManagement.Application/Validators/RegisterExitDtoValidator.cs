using FluentValidation;
using ParkingManagement.Application.DTOs;

namespace ParkingManagement.Application.Validators;

public class RegisterExitDtoValidator : AbstractValidator<RegisterExitDto>
{
    public RegisterExitDtoValidator()
    {
        RuleFor(x => x.Plate)
            .NotEmpty().WithMessage("Plate is required")
            .Length(7).WithMessage("Plate must be exactly 7 characters")
            .Matches("^[A-Z0-9]+$").WithMessage("Plate must contain only uppercase letters and numbers");
    }
}
