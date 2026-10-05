using EkubCircle.Application.DTOs.Circle;
using EkubCircle.Application.DTOs.Member;
using EkubCircle.Application.Interfaces;
using EkubCircle.Domain.Entities;
using EkubCircle.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EkubCircle.Application.Services;

public class CircleService : ICircleService
{
    private readonly IEkubDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public CircleService(
        IEkubDbContext db,
        UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<object> CreateCircleAsync(
        CreateCircleDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Circle name is required.");

        if (request.ContributionAmount <= 0)
            throw new ArgumentException(
                "Contribution amount must be greater than zero.");

        if (string.IsNullOrWhiteSpace(request.MeetingLabel))
            throw new ArgumentException(
                "Meeting label is required.");

        var organizer = await _userManager.FindByIdAsync(
            request.OrganizerId);

        if (organizer == null)
            throw new KeyNotFoundException(
                "Organizer not found.");

        var circle = new Circle
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            ContributionAmount = request.ContributionAmount,
            MeetingLabel = request.MeetingLabel.Trim(),
            OrganizerId = request.OrganizerId,
            MemberCount = 1,
            CurrentRoundNumber = 1,
            CurrentReceiverPosition = 1,
            Status = CircleStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        var member = new CircleMember
        {
            Id = Guid.NewGuid(),
            CircleId = circle.Id,
            UserId = request.OrganizerId,
            Position = 1,
            HasReceived = false
        };

        circle.Members.Add(member);

        _db.Circles.Add(circle);
        _db.CircleMembers.Add(member);

        await _db.SaveChangesAsync();

        return new
        {
            circle.Id,
            circle.Name,
            circle.ContributionAmount,
            circle.MeetingLabel,
            circle.MemberCount,
            circle.Status,
            circle.CurrentRoundNumber
        };
    }

    public async Task<object> GetCircleAsync(Guid circleId)
    {
        var circle = await _db.Circles
            .AsNoTracking()
            .Include(x => x.Members)
            .FirstOrDefaultAsync(x => x.Id == circleId);

        if (circle == null)
            throw new KeyNotFoundException(
                "Circle not found.");

        return new
        {
            circle.Id,
            circle.Name,
            circle.ContributionAmount,
            circle.MeetingLabel,
            circle.MemberCount,
            circle.Status,
            circle.CurrentRoundNumber,
            circle.CurrentReceiverPosition,
            Members = circle.Members
                .OrderBy(x => x.Position)
                .Select(x => new
                {
                    x.UserId,
                    x.Position,
                    x.HasReceived
                })
                .ToList()
        };
    }

    public async Task AddMemberAsync(
        Guid circleId,
        AddMemberDto request,
        string organizerId)
    {
        var circle = await _db.Circles
            .Include(x => x.Members)
            .FirstOrDefaultAsync(x => x.Id == circleId);

        if (circle == null)
            throw new KeyNotFoundException(
                "Circle not found.");

        if (circle.OrganizerId != organizerId)
            throw new UnauthorizedAccessException(
                "Only the organizer can add members.");

        if (circle.Status == CircleStatus.Completed)
            throw new InvalidOperationException(
                "Completed circles cannot accept members.");

        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.TemporaryPassword))
        {
            throw new ArgumentException(
                "Name, email and temporary password are required.");
        }

        var existing = await _userManager.FindByEmailAsync(
            request.Email.Trim());

        if (existing != null)
            throw new InvalidOperationException(
                "A user with this email already exists.");

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.Email.Trim().ToLowerInvariant(),
            Email = request.Email.Trim().ToLowerInvariant(),
            FullName = request.Name.Trim(),
            MustChangePassword = true
        };

        var result = await _userManager.CreateAsync(
            user,
            request.TemporaryPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                "; ",
                result.Errors.Select(x => x.Description));

            throw new InvalidOperationException(errors);
        }

        await _userManager.AddToRoleAsync(user, "Member");

        var position = circle.Members.Count + 1;

        var member = new CircleMember
        {
            Id = Guid.NewGuid(),
            CircleId = circle.Id,
            UserId = user.Id,
            Position = position,
            HasReceived = false
        };

        _db.CircleMembers.Add(member);

        circle.MemberCount = position;

        await _db.SaveChangesAsync();
    }

    public async Task<List<object>> GetMembersAsync(
        Guid circleId)
    {
        var members = await _db.CircleMembers
            .AsNoTracking()
            .Where(x => x.CircleId == circleId)
            .Include(x => x.User)
            .OrderBy(x => x.Position)
            .ToListAsync();

        return members.Select(x => (object)new
        {
            x.UserId,
            Name = x.User?.FullName ?? "",
            Email = x.User?.Email ?? "",
            x.Position,
            x.HasReceived
        }).ToList();
    }
}