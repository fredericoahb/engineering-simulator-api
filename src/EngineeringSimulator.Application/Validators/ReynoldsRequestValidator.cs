using EngineeringSimulator.Application.DTOs;
using FluentValidation;

namespace EngineeringSimulator.Application.Validators;

public class ReynoldsRequestValidator : AbstractValidator<ReynoldsRequest>
{
    public ReynoldsRequestValidator()
    {
        RuleFor(x => x.Rho)
            .GreaterThan(0)
            .WithMessage("Fluid density (rho) must be greater than zero.");

        RuleFor(x => x.Velocity)
            .GreaterThan(0)
            .WithMessage("Flow velocity must be greater than zero.");

        RuleFor(x => x.Diameter)
            .GreaterThan(0)
            .WithMessage("Characteristic diameter must be greater than zero.");

        RuleFor(x => x.Mu)
            .GreaterThan(0)
            .WithMessage("Dynamic viscosity (mu) must be greater than zero.");
    }
}
