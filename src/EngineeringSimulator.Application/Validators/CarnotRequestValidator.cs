using EngineeringSimulator.Application.DTOs;
using FluentValidation;

namespace EngineeringSimulator.Application.Validators;

public class CarnotRequestValidator : AbstractValidator<CarnotRequest>
{
    public CarnotRequestValidator()
    {
        RuleFor(x => x.Th)
            .GreaterThan(0)
            .WithMessage("Hot reservoir temperature (Th) must be greater than zero.");

        RuleFor(x => x.Tc)
            .GreaterThan(0)
            .WithMessage("Cold reservoir temperature (Tc) must be greater than zero.");

        RuleFor(x => x)
            .Must(x => x.Th > x.Tc)
            .When(x => x.Th > 0 && x.Tc > 0)
            .WithMessage("Hot reservoir temperature (Th) must be strictly greater than cold reservoir temperature (Tc).");
    }
}
