using System.Security.Claims;
using EkubCircle.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EkubCircle.Api.Controllers;

public record CreateCircleRequest(
    string Name,
    decimal ContributionAmount,
    string MeetingLabel
);

[ApiController]
[Route("api/v1/circles")]
[AllowAnonymous]
public class CirclesController : ControllerBase
{
    private readonly ISender _sender;

    public CirclesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateCircleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateCircle([FromBody] CreateCircleRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) 
                     ?? User.FindFirstValue("sub") 
                     ?? throw new UnauthorizedAccessException("User ID claim not found.");

        var command = new CreateCircleCommand(
            request.Name,
            request.ContributionAmount,
            request.MeetingLabel,
            userId
        );

        var response = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetCircleById), new { id = response.Id }, response);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetCircleById(int id)
    {
        return Ok();
    }

    [HttpPost("{circleId:int}/start")]
    [ProducesResponseType(typeof(StartCircleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StartCircle(int circleId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User identification token is invalid or missing.");

        var command = new StartCircleCommand(circleId, userId);
        var response = await _sender.Send(command, cancellationToken);

        return Ok(response);
    }
    
}
