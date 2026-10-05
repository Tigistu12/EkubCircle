using System.Security.Claims;
using EkubCircle.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EkubCircle.Api.Controllers;

[ApiController]
[Route("api/v1/rounds")]
[Authorize]
public class RoundsController : ControllerBase
{
    private readonly ISender _sender;

    public RoundsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("{roundId:int}/contributions/pay")]
    [ProducesResponseType(typeof(PayContributionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PayContribution(int roundId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User identification token is invalid or missing.");

        var command = new PayContributionCommand(roundId, userId);
        var response = await _sender.Send(command, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Processes payout for a round to its designated receiver and completes the Circle if it's the final round.
    /// </summary>
    [HttpPost("{roundId:int}/payout")]
    [ProducesResponseType(typeof(ProcessRoundPayoutResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ProcessPayout(int roundId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User identification token is invalid or missing.");

        var command = new ProcessRoundPayoutCommand(roundId, userId);
        var response = await _sender.Send(command, cancellationToken);

        return Ok(response);
    }
}