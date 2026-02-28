using EngineeringSimulator.Application.DTOs;
using EngineeringSimulator.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EngineeringSimulator.API.Controllers;

[ApiController]
[Route("api/v1/carnot")]
[Tags("Carnot Efficiency")]
public class CarnotController : ControllerBase
{
    private readonly EngineeringCalculationService _service;
    private readonly IValidator<CarnotRequest> _validator;

    public CarnotController(EngineeringCalculationService service, IValidator<CarnotRequest> validator)
    {
        _service = service;
        _validator = validator;
    }

    /// <summary>
    /// Calculates the Carnot cycle efficiency: η = 1 − (Tc / Th).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CalculationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Middleware.ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult Calculate([FromBody] CarnotRequest request)
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

        var result = _service.CalculateCarnot(request);
        return Ok(result);
    }
}
