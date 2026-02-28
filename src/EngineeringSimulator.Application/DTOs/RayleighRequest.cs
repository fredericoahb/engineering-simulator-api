namespace EngineeringSimulator.Application.DTOs;

/// <summary>
/// Request for Rayleigh Number calculation.
/// </summary>
public sealed class RayleighRequest
{
    /// <summary>Gravitational acceleration (m/s²).</summary>
    public double G { get; init; }

    /// <summary>Thermal expansion coefficient (1/K).</summary>
    public double Beta { get; init; }

    /// <summary>Temperature difference (K).</summary>
    public double DeltaT { get; init; }

    /// <summary>Characteristic length (m).</summary>
    public double L { get; init; }

    /// <summary>Kinematic viscosity (m²/s).</summary>
    public double Nu { get; init; }

    /// <summary>Thermal diffusivity (m²/s).</summary>
    public double Alpha { get; init; }
}
