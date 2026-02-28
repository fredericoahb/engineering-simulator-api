namespace EngineeringSimulator.Application.DTOs;

/// <summary>
/// Standard response envelope for all engineering calculations.
/// </summary>
public sealed class CalculationResponse
{
    public double Result { get; init; }
    public string Unit { get; init; } = string.Empty;
    public string? Classification { get; init; }
    public string Notes { get; init; } = string.Empty;
    public object InputEcho { get; init; } = new();
}
