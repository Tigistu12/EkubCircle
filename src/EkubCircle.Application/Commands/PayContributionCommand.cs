using MediatR;

namespace EkubCircle.Application.Commands;

public record PayContributionCommand(
    int RoundId,
    string UserId
) : IRequest<PayContributionResponse>;

public record PayContributionResponse(
    int ContributionId,
    int RoundId,
    int MemberId,
    decimal Amount,
    string Status,
    DateTime PaidAt,
    bool IsRoundFullyPaid
);