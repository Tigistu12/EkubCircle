using System.Security.Claims;
using EkubCircle.Application.DTOs.Circle;
using EkubCircle.Application.DTOs.Member;
using EkubCircle.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EkubCircle.Api.Controllers;

[ApiController]
[Route("api/circles")]
[Authorize]
public class CirclesController : ControllerBase
{
    private readonly ICircleService _circleService;

    public CirclesController(
        ICircleService circleService)
    {
        _circleService = circleService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCircleDto request)
    {
        try
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            request.OrganizerId = userId;

            var result =
                await _circleService.CreateCircleAsync(request);

            return Created("", result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }

    [HttpGet("{circleId:guid}")]
    public async Task<IActionResult> Get(
        Guid circleId)
    {
        try
        {
            var result =
                await _circleService.GetCircleAsync(circleId);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPost("{circleId:guid}/members")]
    [Authorize(Roles = "Organizer")]
    public async Task<IActionResult> AddMember(
        Guid circleId,
        AddMemberDto request)
    {
        try
        {
            var organizerId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(organizerId))
                return Unauthorized();

            await _circleService.AddMemberAsync(
                circleId,
                request,
                organizerId);

            return Ok(new
            {
                message = "Member created successfully."
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    [HttpGet("{circleId:guid}/members")]
    public async Task<IActionResult> Members(
        Guid circleId)
    {
        try
        {
            var result =
                await _circleService.GetMembersAsync(circleId);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }
}