using FluentValidation;
using ParkingManagement.Application.DTOs;

namespace ParkingManagement.Application.Validators;

public class CreateVehicleDtoValidator : AbstractValidator<CreateVehicleDto>
{
    public CreateVehicleDtoValidator()
    {
        RuleFor(x => x.Plate)
            .NotEmpty().WithMessage("Plate is required")
            .Length(7).WithMessage("Plate must be exactly 7 characters")
            .Matches("^[A-Z0-9]+$").WithMessage("Plate must contain only uppercase letters and numbers");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required")
            .MaximumLength(100).WithMessage("Model cannot exceed 100 characters");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Vehicle type must be Car or Motorcycle");
    }
}
