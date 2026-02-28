namespace EngineeringSimulator.Application.DTOs;

/// <summary>
/// Request for Wobbe Index calculation.
/// </summary>
public sealed class WobbeRequest
{
    /// <summary>Higher Heating Value (MJ/m³ or kJ/m³).</summary>
    public double Pcs { get; init; }

    /// <summary>Relative density of the gas (dimensionless, gas/air).</summary>
    public double RelativeDensity { get; init; }
}
