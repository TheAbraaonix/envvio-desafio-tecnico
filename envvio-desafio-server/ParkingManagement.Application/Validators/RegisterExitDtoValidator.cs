using FluentValidation;
using ParkingManagement.Application.DTOs;

namespace ParkingManagement.Application.Validators;

public class RegisterExitDtoValidator : AbstractValidator<RegisterExitDto>
{
    public RegisterExitDtoValidator()
    {
        RuleFor(x => x.SessionId)
            .GreaterThan(0).WithMessage("Session ID must be greater than 0");
    }
}
