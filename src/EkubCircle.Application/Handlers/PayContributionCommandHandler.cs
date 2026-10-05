using EkubCircle.Application.Exceptions;
using EkubCircle.Application.Interfaces;
using EkubCircle.Application.Commands;
using EkubCircle.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EkubCircle.Application.Handlers;

public class PayContributionCommandHandler : IRequestHandler<PayContributionCommand, PayContributionResponse>
{
    private readonly IApplicationDbContext _context;

    public PayContributionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PayContributionResponse> Handle(PayContributionCommand request, CancellationToken cancellationToken)
    {
        var round = await _context.Rounds
            .Include(r => r.Contributions)
            .FirstOrDefaultAsync(r => r.Id == request.RoundId, cancellationToken);

        if (round == null)
        {
            throw new NotFoundException($"Round with ID {request.RoundId} was not found.");
        }

        if (round.Status != RoundStatus.Open)
        {
            throw new InvalidOperationException("Contributions can only be made to an Open round.");
        }

        // Find the member record corresponding to the current requesting user in this circle
        var member = await _context.Members
            .FirstOrDefaultAsync(m => m.CircleId == round.CircleId && m.UserId == request.UserId, cancellationToken);

        if (member == null)
        {
            throw new InvalidOperationException("You are not a registered member of this Circle.");
        }

        // Retrieve member's contribution record for this round
        var contribution = round.Contributions
            .FirstOrDefault(c => c.MemberId == member.Id);

        if (contribution == null)
        {
            throw new NotFoundException("Contribution record not found for this member in the specified round.");
        }

        if (contribution.Status == ContributionStatus.Paid)
        {
            throw new InvalidOperationException("Contribution for this round has already been paid.");
        }

        // Process payment
        contribution.Status = ContributionStatus.Paid;
        contribution.PaidAt = DateTime.UtcNow;

        // Check if all members have completed payment for this round
        bool isRoundFullyPaid = round.Contributions.All(c => c.Status == ContributionStatus.Paid);

        await _context.SaveChangesAsync(cancellationToken);

        return new PayContributionResponse(
            contribution.Id,
            contribution.RoundId,
            contribution.MemberId,
            contribution.Amount,
            contribution.Status.ToString(),
            contribution.PaidAt.Value,
            isRoundFullyPaid
        );
    }
}