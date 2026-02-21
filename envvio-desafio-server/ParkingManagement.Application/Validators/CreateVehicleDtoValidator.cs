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
            // Brazilian plate patterns: LLLNNNN (old) or LLLNLNN (Mercosul)
            .Matches("^[A-Z]{3}([0-9]{4}|[0-9][A-J][0-9]{2})$")
            .WithMessage("Plate must follow Brazilian format: ABC1234 or ABC1D23");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required")
            .MaximumLength(100).WithMessage("Model cannot exceed 100 characters");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Vehicle type must be Car or Motorcycle");
    }
}
