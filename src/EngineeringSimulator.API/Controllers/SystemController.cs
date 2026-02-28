using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace EngineeringSimulator.API.Controllers;

[ApiController]
[Tags("System")]
public class SystemController : ControllerBase
{
    /// <summary>
    /// Health check endpoint.
    /// </summary>
    [HttpGet("health")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult Health()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Returns API build version and runtime info.
    /// </summary>
    [HttpGet("api/v1/info")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult Info()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version?.ToString() ?? "1.0.0";

        return Ok(new
        {
            application = "Engineering Simulator API",
            version,
            runtime = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
            timestamp = DateTime.UtcNow
        });
    }
}
