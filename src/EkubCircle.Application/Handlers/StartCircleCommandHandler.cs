using EkubCircle.Application.Commands;
using EkubCircle.Application.Interfaces;
using EkubCircle.Domain.Entities;
using EkubCircle.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EkubCircle.Application.Handlers;

public class StartCircleCommandHandler : IRequestHandler<StartCircleCommand, StartCircleResponse>
{
    private readonly IApplicationDbContext _context;

    public StartCircleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StartCircleResponse> Handle(StartCircleCommand request, CancellationToken cancellationToken)
    {
        var circle = await _context.Circles
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id == request.CircleId, cancellationToken);

        if (circle == null)
        {
            throw new KeyNotFoundException($"Circle with ID {request.CircleId} was not found.");
        }

        // Only the Organizer can start the circle
        if (circle.OrganizerUserId != request.RequestingUserId)
        {
            throw new UnauthorizedAccessException("Only the Circle Organizer can start the circle.");
        }

        if (circle.Status != CircleStatus.Forming)
        {
            throw new InvalidOperationException("Circle can only be started when it is in 'Forming' status.");
        }

        if (circle.Members.Count < 2)
        {
            throw new InvalidOperationException("At least 2 members are required to start a Circle.");
        }

        // 1. Shuffle members randomly to assign fair payout order
        var shuffledMembers = circle.Members.OrderBy(_ => Guid.NewGuid()).ToList();
        for (int i = 0; i < shuffledMembers.Count; i++)
        {
            shuffledMembers[i].PayoutOrder = i + 1;
        }

        // 2. Update Circle state
        circle.Status = CircleStatus.Active;
        circle.StartedAt = DateTime.UtcNow;

        // 3. Generate Rounds and expected Contributions
        int totalRounds = shuffledMembers.Count;
        for (int roundNum = 1; roundNum <= totalRounds; roundNum++)
        {
            var designatedReceiver = shuffledMembers.First(m => m.PayoutOrder == roundNum);

            var round = new Round
            {
                CircleId = circle.Id,
                RoundNumber = roundNum,
                Status = RoundStatus.Open,
                ReceiverMemberId = designatedReceiver.Id,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var member in shuffledMembers)
            {
                round.Contributions.Add(new Contribution
                {
                    MemberId = member.Id,
                    Amount = circle.ContributionAmount,
                    Status = ContributionStatus.Pending
                });
            }

            _context.Rounds.Add(round);
        }

        await _context.SaveChangesAsync(cancellationToken);

        var payoutOrders = shuffledMembers
            .OrderBy(m => m.PayoutOrder)
            .Select(m => new MemberPayoutOrderDto(m.Id, m.FullName, m.PayoutOrder))
            .ToList();

        return new StartCircleResponse(
            circle.Id,
            circle.Status.ToString(),
            circle.StartedAt.Value,
            totalRounds,
            payoutOrders
        );
    }
}