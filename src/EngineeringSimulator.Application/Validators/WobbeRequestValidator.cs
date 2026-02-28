using EngineeringSimulator.Application.DTOs;
using FluentValidation;

namespace EngineeringSimulator.Application.Validators;

public class WobbeRequestValidator : AbstractValidator<WobbeRequest>
{
    public WobbeRequestValidator()
    {
        RuleFor(x => x.Pcs)
            .GreaterThan(0)
            .WithMessage("PCS (Higher Heating Value) must be greater than zero.");

        RuleFor(x => x.RelativeDensity)
            .GreaterThan(0)
            .WithMessage("Relative density must be greater than zero.");
    }
}
