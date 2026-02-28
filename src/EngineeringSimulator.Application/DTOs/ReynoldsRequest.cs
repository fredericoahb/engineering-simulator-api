namespace EngineeringSimulator.Application.DTOs;

/// <summary>
/// Request for Reynolds Number calculation.
/// </summary>
public sealed class ReynoldsRequest
{
    /// <summary>Fluid density (kg/m³).</summary>
    public double Rho { get; init; }

    /// <summary>Flow velocity (m/s).</summary>
    public double Velocity { get; init; }

    /// <summary>Characteristic diameter (m).</summary>
    public double Diameter { get; init; }

    /// <summary>Dynamic viscosity (Pa·s).</summary>
    public double Mu { get; init; }
}
