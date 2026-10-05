using EkubCircle.Application.Commands;
using EkubCircle.Application.Interfaces;
using EkubCircle.Domain.Entities;
using EkubCircle.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EkubCircle.Application.Handlers;

public class AddMemberToCircleCommandHandler : IRequestHandler<AddMemberToCircleCommand, AddMemberToCircleResponse>
{
    private readonly IApplicationDbContext _context;

    public AddMemberToCircleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AddMemberToCircleResponse> Handle(AddMemberToCircleCommand request, CancellationToken cancellationToken)
    {
        var circle = await _context.Circles
            .FirstOrDefaultAsync(c => c.Id == request.CircleId, cancellationToken);

        if (circle == null)
        {
            throw new KeyNotFoundException($"Circle with ID {request.CircleId} was not found.");
        }

        if (circle.Status != CircleStatus.Forming)
        {
            throw new InvalidOperationException("Members can only be added when the Circle is in 'Forming' status.");
        }

        bool isAlreadyMember = await _context.Members
            .AnyAsync(m => m.CircleId == request.CircleId && m.UserId == request.UserId, cancellationToken);

        if (isAlreadyMember)
        {
            throw new InvalidOperationException("User is already a member of this Circle.");
        }

        var member = new Member
        {
            CircleId = circle.Id,
            UserId = request.UserId,
            FullName = request.FullName,
            JoinedAt = DateTime.UtcNow
        };

        _context.Members.Add(member);
        await _context.SaveChangesAsync(cancellationToken);

        return new AddMemberToCircleResponse(
            member.Id,
            member.CircleId,
            member.UserId,
            member.FullName,
            member.JoinedAt
        );
    }
}