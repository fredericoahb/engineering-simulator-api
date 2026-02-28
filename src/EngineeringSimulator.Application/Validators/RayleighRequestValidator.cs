using EngineeringSimulator.Application.DTOs;
using FluentValidation;

namespace EngineeringSimulator.Application.Validators;

public class RayleighRequestValidator : AbstractValidator<RayleighRequest>
{
    public RayleighRequestValidator()
    {
        RuleFor(x => x.G)
            .GreaterThan(0)
            .WithMessage("Gravitational acceleration (g) must be greater than zero.");

        RuleFor(x => x.Beta)
            .GreaterThan(0)
            .WithMessage("Thermal expansion coefficient (beta) must be greater than zero.");

        RuleFor(x => x.DeltaT)
            .GreaterThan(0)
            .WithMessage("Temperature difference (deltaT) must be greater than zero.");

        RuleFor(x => x.L)
            .GreaterThan(0)
            .WithMessage("Characteristic length (L) must be greater than zero.");

        RuleFor(x => x.Nu)
            .GreaterThan(0)
            .WithMessage("Kinematic viscosity (nu) must be greater than zero.");

        RuleFor(x => x.Alpha)
            .GreaterThan(0)
            .WithMessage("Thermal diffusivity (alpha) must be greater than zero.");
    }
}
