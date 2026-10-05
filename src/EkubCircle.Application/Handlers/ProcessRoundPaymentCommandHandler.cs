using EkubCircle.Application.Exceptions;
using EkubCircle.Application.Interfaces;
using EkubCircle.Application.Commands;
using EkubCircle.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EkubCircle.Application.Handlers;

public class ProcessRoundPayoutCommandHandler : IRequestHandler<ProcessRoundPayoutCommand, ProcessRoundPayoutResponse>
{
    private readonly IApplicationDbContext _context;

    public ProcessRoundPayoutCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProcessRoundPayoutResponse> Handle(ProcessRoundPayoutCommand request, CancellationToken cancellationToken)
    {
        // Fetch round with contributions, circle, receiver member, and all sibling rounds
        var round = await _context.Rounds
            .Include(r => r.Contributions)
            .Include(r => r.ReceiverMember)
            .Include(r => r.Circle)
                .ThenInclude(c => c!.Rounds)
            .FirstOrDefaultAsync(r => r.Id == request.RoundId, cancellationToken);

        if (round == null)
        {
            throw new NotFoundException($"Round with ID {request.RoundId} was not found.");
        }

        if (round.Circle == null)
        {
            throw new NotFoundException($"Circle associated with Round ID {request.RoundId} was not found.");
        }

        // Authorization Guard: Only the Circle Organizer can trigger payouts
        if (round.Circle.OrganizerUserId != request.RequestingUserId)
        {
            throw new UnauthorizedAccessException("Only the Circle Organizer can process payouts.");
        }

        if (round.Status == RoundStatus.PaidOut)
        {
            throw new InvalidOperationException("Payout for this round has already been processed.");
        }

        // Business Rule Guard: All contributions must be 'Paid' before distributing payout
        bool hasPendingContributions = round.Contributions.Any(c => c.Status != ContributionStatus.Paid);
        if (hasPendingContributions)
        {
            throw new InvalidOperationException("Cannot process payout until all members have paid their contributions for this round.");
        }

        if (round.ReceiverMember == null)
        {
            throw new InvalidOperationException("No designated receiver member found for this round.");
        }

        // 1. Process Payout for the Round
        round.Status = RoundStatus.PaidOut;
        round.PaidOutAt = DateTime.UtcNow;
        round.ReceiverMember.HasReceived = true;

        decimal totalPayoutAmount = round.Contributions.Sum(c => c.Amount);

        // 2. Check if all rounds in the circle are now paid out
        bool areAllRoundsPaidOut = round.Circle.Rounds.All(r => r.Id == round.Id || r.Status == RoundStatus.PaidOut);

        if (areAllRoundsPaidOut)
        {
            round.Circle.Status = CircleStatus.Completed;
            round.Circle.CompletedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new ProcessRoundPayoutResponse(
            round.Id,
            round.CircleId,
            round.ReceiverMember.Id,
            round.ReceiverMember.FullName,
            totalPayoutAmount,
            round.Status.ToString(),
            round.PaidOutAt.Value,
            areAllRoundsPaidOut,
            round.Circle.Status.ToString()
        );
    }
}