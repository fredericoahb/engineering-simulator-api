using EngineeringSimulator.Domain.Exceptions;

namespace EngineeringSimulator.Domain.Calculations;

/// <summary>
/// Calculates the Wobbe Index, a key indicator of fuel gas interchangeability.
/// W = PCS / sqrt(d)
/// where PCS = Higher Heating Value and d = relative density (gas/air).
/// </summary>
public static class WobbeCalculator
{
    /// <summary>
    /// Computes the Wobbe Index.
    /// </summary>
    /// <param name="pcs">Higher Heating Value (MJ/m³ or kJ/m³). Must be > 0.</param>
    /// <param name="relativeDensity">Relative density of the gas (dimensionless). Must be > 0.</param>
    /// <returns>Wobbe Index in the same unit as PCS.</returns>
    public static double Calculate(double pcs, double relativeDensity)
    {
        if (pcs <= 0)
            throw new DomainValidationException(nameof(pcs),
                "PCS (Higher Heating Value) must be greater than zero.");

        if (relativeDensity <= 0)
            throw new DomainValidationException(nameof(relativeDensity),
                "Relative density must be greater than zero.");

        return pcs / Math.Sqrt(relativeDensity);
    }

    /// <summary>
    /// Returns a brief interpretive note for a given Wobbe Index value (MJ/m³ context).
    /// </summary>
    public static string Interpret(double wobbeIndex)
    {
        return wobbeIndex switch
        {
            >= 48.0 and <= 53.0 => "Within typical natural gas interchangeability range (Group H).",
            >= 22.0 and < 48.0 => "Lower Wobbe range — possibly manufactured or mixed gas (Group L).",
            > 53.0 => "High Wobbe Index — rich gas, verify burner compatibility.",
            _ => "Wobbe Index computed. Verify against applicable gas group standards."
        };
    }
}
