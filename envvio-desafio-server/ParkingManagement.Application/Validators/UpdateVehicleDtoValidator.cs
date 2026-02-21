using FluentValidation;
using ParkingManagement.Application.DTOs;

namespace ParkingManagement.Application.Validators;

public class UpdateVehicleDtoValidator : AbstractValidator<UpdateVehicleDto>
{
    public UpdateVehicleDtoValidator()
    {
        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required")
            .MaximumLength(100).WithMessage("Model cannot exceed 100 characters");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Vehicle type must be Car or Motorcycle");
    }
}
