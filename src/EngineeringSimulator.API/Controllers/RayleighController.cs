using EngineeringSimulator.Application.DTOs;
using EngineeringSimulator.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EngineeringSimulator.API.Controllers;

[ApiController]
[Route("api/v1/rayleigh")]
[Tags("Rayleigh Number")]
public class RayleighController : ControllerBase
{
    private readonly EngineeringCalculationService _service;
    private readonly IValidator<RayleighRequest> _validator;

    public RayleighController(EngineeringCalculationService service, IValidator<RayleighRequest> validator)
    {
        _service = service;
        _validator = validator;
    }

    /// <summary>
    /// Calculates the Rayleigh Number: Ra = (g·β·ΔT·L³) / (ν·α).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CalculationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Middleware.ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult Calculate([FromBody] RayleighRequest request)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid)
            return BadRequest(new Middleware.ApiErrorResponse
            {
                StatusCode = 400,
                Message = "One or more validation errors occurred.",
                Errors = validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            });

        var result = _service.CalculateRayleigh(request);
        return Ok(result);
    }
}
