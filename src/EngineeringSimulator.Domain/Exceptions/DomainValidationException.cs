namespace EngineeringSimulator.Domain.Exceptions;

/// <summary>
/// Thrown when a domain-level engineering calculation receives invalid parameters.
/// </summary>
public class DomainValidationException : Exception
{
    public string ParameterName { get; }

    public DomainValidationException(string parameterName, string message)
        : base(message)
    {
        ParameterName = parameterName;
    }
}
