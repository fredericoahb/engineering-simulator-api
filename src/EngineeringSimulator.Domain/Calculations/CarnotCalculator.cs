using EngineeringSimulator.Domain.Exceptions;

namespace EngineeringSimulator.Domain.Calculations;

/// <summary>
/// Calculates the Carnot cycle thermal efficiency — the theoretical maximum
/// efficiency for a heat engine operating between two thermal reservoirs.
/// η = 1 − (Tc / Th)
/// </summary>
public static class CarnotCalculator
{
    /// <summary>
    /// Computes the Carnot efficiency.
    /// </summary>
    /// <param name="th">Hot reservoir temperature (K). Must be > 0.</param>
    /// <param name="tc">Cold reservoir temperature (K). Must be > 0 and &lt; Th.</param>
    /// <returns>Efficiency as a fraction in [0, 1).</returns>
    public static double Calculate(double th, double tc)
    {
        if (th <= 0)
            throw new DomainValidationException(nameof(th),
                "Hot reservoir temperature (Th) must be greater than zero.");
        if (tc <= 0)
            throw new DomainValidationException(nameof(tc),
                "Cold reservoir temperature (Tc) must be greater than zero.");
        if (th <= tc)
            throw new DomainValidationException(nameof(th),
                "Hot reservoir temperature (Th) must be strictly greater than cold reservoir temperature (Tc).");

        return 1.0 - (tc / th);
    }

    /// <summary>
    /// Converts fractional efficiency to percentage.
    /// </summary>
    public static double ToPercentage(double efficiency) => efficiency * 100.0;

    /// <summary>
    /// Returns a brief physical interpretation.
    /// </summary>
    public static string Interpret(double efficiency)
    {
        return efficiency switch
        {
            < 0.20 => "Low theoretical efficiency — small temperature differential between reservoirs.",
            < 0.50 => "Moderate Carnot efficiency — typical of many industrial heat engine scenarios.",
            < 0.75 => "High theoretical efficiency — significant temperature gradient.",
            _ => "Very high Carnot efficiency — extreme temperature ratio between reservoirs."
        };
    }
}
