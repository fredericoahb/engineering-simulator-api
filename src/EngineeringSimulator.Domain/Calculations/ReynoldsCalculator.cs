using EngineeringSimulator.Domain.Exceptions;

namespace EngineeringSimulator.Domain.Calculations;

/// <summary>
/// Calculates the Reynolds Number, a dimensionless quantity used to predict
/// flow patterns (laminar, transitional, or turbulent) in fluid mechanics.
/// Re = (ρ · V · D) / μ
/// </summary>
public static class ReynoldsCalculator
{
    public const double LaminarThreshold = 2300.0;
    public const double TurbulentThreshold = 4000.0;

    /// <summary>
    /// Computes the Reynolds Number.
    /// </summary>
    /// <param name="rho">Fluid density (kg/m³). Must be > 0.</param>
    /// <param name="velocity">Flow velocity (m/s). Must be > 0.</param>
    /// <param name="diameter">Characteristic length / diameter (m). Must be > 0.</param>
    /// <param name="mu">Dynamic viscosity (Pa·s). Must be > 0.</param>
    /// <returns>Dimensionless Reynolds Number.</returns>
    public static double Calculate(double rho, double velocity, double diameter, double mu)
    {
        if (rho <= 0)
            throw new DomainValidationException(nameof(rho), "Fluid density (rho) must be greater than zero.");
        if (velocity <= 0)
            throw new DomainValidationException(nameof(velocity), "Flow velocity must be greater than zero.");
        if (diameter <= 0)
            throw new DomainValidationException(nameof(diameter), "Characteristic diameter must be greater than zero.");
        if (mu <= 0)
            throw new DomainValidationException(nameof(mu), "Dynamic viscosity (mu) must be greater than zero.");

        return (rho * velocity * diameter) / mu;
    }

    /// <summary>
    /// Classifies the flow regime based on the Reynolds Number.
    /// </summary>
    public static string Classify(double re)
    {
        return re switch
        {
            < LaminarThreshold => "Laminar",
            < TurbulentThreshold => "Transition",
            _ => "Turbulent"
        };
    }

    /// <summary>
    /// Returns a brief physical interpretation.
    /// </summary>
    public static string Interpret(double re)
    {
        var classification = Classify(re);
        return classification switch
        {
            "Laminar" => "Laminar flow — viscous forces dominate, smooth streamlines.",
            "Transition" => "Transitional regime — flow may intermittently switch between laminar and turbulent.",
            "Turbulent" => "Turbulent flow — inertial forces dominate, chaotic eddies present.",
            _ => "Reynolds number computed."
        };
    }
}
