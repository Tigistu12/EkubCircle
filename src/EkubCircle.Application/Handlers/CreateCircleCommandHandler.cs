using EkubCircle.Application.Interfaces;
using EkubCircle.Domain.Entities;
using EkubCircle.Domain.Enums;
using MediatR;
using EkubCircle.Application.Commands;

namespace EkubCircle.Application.Handlers;

public class CreateCircleCommandHandler : IRequestHandler<CreateCircleCommand, CreateCircleResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateCircleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateCircleResponse> Handle(CreateCircleCommand request, CancellationToken cancellationToken)
    {
     
    
        var circle = new Circle
        {
            Name = request.Name,
            ContributionAmount = request.ContributionAmount,
            MeetingLabel = request.MeetingLabel,
            OrganizerUserId = request.OrganizerUserId,
            Status = CircleStatus.Forming,
            CreatedAt = DateTime.UtcNow
        };

        _context.Circles.Add(circle);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateCircleResponse(
            circle.Id,
            circle.Name,
            circle.ContributionAmount,
            circle.MeetingLabel,
            circle.OrganizerUserId,
            circle.Status.ToString(),
            circle.CreatedAt
        );
    }
}