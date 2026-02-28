using EngineeringSimulator.Application.DTOs;
using EngineeringSimulator.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EngineeringSimulator.API.Controllers;

[ApiController]
[Route("api/v1/wobbe")]
[Tags("Wobbe Index")]
public class WobbeController : ControllerBase
{
    private readonly EngineeringCalculationService _service;
    private readonly IValidator<WobbeRequest> _validator;

    public WobbeController(EngineeringCalculationService service, IValidator<WobbeRequest> validator)
    {
        _service = service;
        _validator = validator;
    }

    /// <summary>
    /// Calculates the Wobbe Index: W = PCS / sqrt(relative_density).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CalculationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Middleware.ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult Calculate([FromBody] WobbeRequest request)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid)
            return BadRequest(ToErrorResponse(validation));

        var result = _service.CalculateWobbe(request);
        return Ok(result);
    }

    private static Middleware.ApiErrorResponse ToErrorResponse(FluentValidation.Results.ValidationResult result) =>
        new()
        {
            StatusCode = 400,
            Message = "One or more validation errors occurred.",
            Errors = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
        };
}
