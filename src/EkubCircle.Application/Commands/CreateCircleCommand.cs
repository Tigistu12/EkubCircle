using MediatR;

namespace EkubCircle.Application.Commands;

public record CreateCircleCommand(
    string Name,
    decimal ContributionAmount,
    string MeetingLabel,
    string OrganizerUserId
) : IRequest<CreateCircleResponse>;

public record CreateCircleResponse(
    int Id,
    string Name,
    decimal ContributionAmount,
    string MeetingLabel,
    string OrganizerUserId,
    string Status,
    DateTime CreatedAt
);