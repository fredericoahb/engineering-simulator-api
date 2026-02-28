using EngineeringSimulator.Domain.Exceptions;

namespace EngineeringSimulator.Domain.Calculations;

/// <summary>
/// Calculates the Rayleigh Number, a dimensionless number associated with
/// buoyancy-driven (natural) convection.
/// Ra = (g · β · ΔT · L³) / (ν · α)
/// </summary>
public static class RayleighCalculator
{
    public const double ConvectionOnsetThreshold = 1708.0;
    public const double TurbulentConvectionThreshold = 1e9;

    /// <summary>
    /// Computes the Rayleigh Number.
    /// </summary>
    /// <param name="g">Gravitational acceleration (m/s²). Must be > 0.</param>
    /// <param name="beta">Thermal expansion coefficient (1/K). Must be > 0.</param>
    /// <param name="deltaT">Temperature difference (K). Must be > 0.</param>
    /// <param name="characteristicLength">Characteristic length (m). Must be > 0.</param>
    /// <param name="nu">Kinematic viscosity (m²/s). Must be > 0.</param>
    /// <param name="alpha">Thermal diffusivity (m²/s). Must be > 0.</param>
    /// <returns>Dimensionless Rayleigh Number.</returns>
    public static double Calculate(double g, double beta, double deltaT,
        double characteristicLength, double nu, double alpha)
    {
        if (g <= 0)
            throw new DomainValidationException(nameof(g), "Gravitational acceleration must be greater than zero.");
        if (beta <= 0)
            throw new DomainValidationException(nameof(beta), "Thermal expansion coefficient must be greater than zero.");
        if (deltaT <= 0)
            throw new DomainValidationException(nameof(deltaT), "Temperature difference must be greater than zero.");
        if (characteristicLength <= 0)
            throw new DomainValidationException(nameof(characteristicLength), "Characteristic length must be greater than zero.");
        if (nu <= 0)
            throw new DomainValidationException(nameof(nu), "Kinematic viscosity must be greater than zero.");
        if (alpha <= 0)
            throw new DomainValidationException(nameof(alpha), "Thermal diffusivity must be greater than zero.");

        double l3 = Math.Pow(characteristicLength, 3);
        return (g * beta * deltaT * l3) / (nu * alpha);
    }

    /// <summary>
    /// Classifies the convection regime.
    /// </summary>
    public static string Classify(double ra)
    {
        return ra switch
        {
            < ConvectionOnsetThreshold => "No convection (conduction dominant)",
            < TurbulentConvectionThreshold => "Laminar natural convection",
            _ => "Turbulent natural convection"
        };
    }

    /// <summary>
    /// Returns a brief physical interpretation.
    /// </summary>
    public static string Interpret(double ra)
    {
        var classification = Classify(ra);
        return classification switch
        {
            "No convection (conduction dominant)" =>
                "Ra below critical threshold (~1708) — heat transfer is primarily by conduction.",
            "Laminar natural convection" =>
                "Buoyancy-driven laminar convection — orderly convective cells form.",
            "Turbulent natural convection" =>
                "Ra exceeds ~10^9 — turbulent natural convection with enhanced heat transfer.",
            _ => "Rayleigh number computed."
        };
    }
}
