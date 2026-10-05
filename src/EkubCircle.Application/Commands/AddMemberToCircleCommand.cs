using MediatR;

namespace EkubCircle.Application.Commands;

public record AddMemberToCircleCommand(
    int CircleId,
    string UserId,
    string FullName
) : IRequest<AddMemberToCircleResponse>;

public record AddMemberToCircleResponse(
    int MemberId,
    int CircleId,
    string UserId,
    string FullName,
    DateTime JoinedAt
);