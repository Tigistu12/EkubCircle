using MediatR;

namespace EkubCircle.Application.Commands;

public record StartCircleCommand(
    int CircleId,
    string RequestingUserId
) : IRequest<StartCircleResponse>;

public record MemberPayoutOrderDto(
    int MemberId,
    string FullName,
    int PayoutOrder
);

public record StartCircleResponse(
    int CircleId,
    string Status,
    DateTime StartedAt,
    int TotalRounds,
    List<MemberPayoutOrderDto> MemberPayoutOrders
);