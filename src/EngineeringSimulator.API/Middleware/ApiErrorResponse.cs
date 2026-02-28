namespace EngineeringSimulator.API.Middleware;

/// <summary>
/// Standard error response returned by the API on validation or internal errors.
/// </summary>
public sealed class ApiErrorResponse
{
    public int StatusCode { get; init; }
    public string Message { get; init; } = string.Empty;
    public IDictionary<string, string[]>? Errors { get; init; }
}
