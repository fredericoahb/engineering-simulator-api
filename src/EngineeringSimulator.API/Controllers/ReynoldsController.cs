using EngineeringSimulator.Application.DTOs;
using EngineeringSimulator.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EngineeringSimulator.API.Controllers;

[ApiController]
[Route("api/v1/reynolds")]
[Tags("Reynolds Number")]
public class ReynoldsController : ControllerBase
{
    private readonly EngineeringCalculationService _service;
    private readonly IValidator<ReynoldsRequest> _validator;

    public ReynoldsController(EngineeringCalculationService service, IValidator<ReynoldsRequest> validator)
    {
        _service = service;
        _validator = validator;
    }

    /// <summary>
    /// Calculates the Reynolds Number: Re = (ρ·V·D) / μ.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CalculationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Middleware.ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult Calculate([FromBody] ReynoldsRequest request)
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

        var result = _service.CalculateReynolds(request);
        return Ok(result);
    }
}
