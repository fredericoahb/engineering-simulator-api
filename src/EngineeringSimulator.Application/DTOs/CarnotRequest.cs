namespace EngineeringSimulator.Application.DTOs;

/// <summary>
/// Request for Carnot cycle efficiency calculation.
/// </summary>
public sealed class CarnotRequest
{
    /// <summary>Hot reservoir temperature (K).</summary>
    public double Th { get; init; }

    /// <summary>Cold reservoir temperature (K).</summary>
    public double Tc { get; init; }
}
