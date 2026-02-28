using EngineeringSimulator.Application.DTOs;
using EngineeringSimulator.Domain.Calculations;

namespace EngineeringSimulator.Application.Services;

/// <summary>
/// Application service that orchestrates engineering calculations.
/// Assumes input has already been validated by FluentValidation.
/// </summary>
public class EngineeringCalculationService
{
    public CalculationResponse CalculateWobbe(WobbeRequest request)
    {
        var result = WobbeCalculator.Calculate(request.Pcs, request.RelativeDensity);
        return new CalculationResponse
        {
            Result = Math.Round(result, 6),
            Unit = "Same as PCS input (MJ/m³ or kJ/m³)",
            Classification = null,
            Notes = WobbeCalculator.Interpret(result),
            InputEcho = request
        };
    }

    public CalculationResponse CalculateReynolds(ReynoldsRequest request)
    {
        var result = ReynoldsCalculator.Calculate(
            request.Rho, request.Velocity, request.Diameter, request.Mu);
        return new CalculationResponse
        {
            Result = Math.Round(result, 4),
            Unit = "-",
            Classification = ReynoldsCalculator.Classify(result),
            Notes = ReynoldsCalculator.Interpret(result),
            InputEcho = request
        };
    }

    public CalculationResponse CalculateRayleigh(RayleighRequest request)
    {
        var result = RayleighCalculator.Calculate(
            request.G, request.Beta, request.DeltaT, request.L, request.Nu, request.Alpha);
        return new CalculationResponse
        {
            Result = Math.Round(result, 4),
            Unit = "-",
            Classification = RayleighCalculator.Classify(result),
            Notes = RayleighCalculator.Interpret(result),
            InputEcho = request
        };
    }

    public CalculationResponse CalculateCarnot(CarnotRequest request)
    {
        var efficiency = CarnotCalculator.Calculate(request.Th, request.Tc);
        var percentage = CarnotCalculator.ToPercentage(efficiency);
        return new CalculationResponse
        {
            Result = Math.Round(efficiency, 6),
            Unit = $"fraction (= {Math.Round(percentage, 2)}%)",
            Classification = null,
            Notes = CarnotCalculator.Interpret(efficiency),
            InputEcho = request
        };
    }
}
