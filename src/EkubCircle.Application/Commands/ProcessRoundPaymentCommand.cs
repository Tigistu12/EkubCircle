using MediatR;

namespace EkubCircle.Application.Commands;

public record ProcessRoundPayoutCommand(
    int RoundId,
    string RequestingUserId
) : IRequest<ProcessRoundPayoutResponse>;

public record ProcessRoundPayoutResponse(
    int RoundId,
    int CircleId,
    int ReceiverMemberId,
    string ReceiverFullName,
    decimal TotalPayoutAmount,
    string RoundStatus,
    DateTime PaidOutAt,
    bool IsCircleCompleted,
    string CircleStatus
);