using EkubCircle.Domain.Enums;
namespace EkubCircle.Domain.Entities;

public class Circle
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required decimal ContributionAmount { get; set; }
    public required string MeetingLabel { get; set; } // e.g. "Weekly", "Monthly"
    public required string OrganizerUserId { get; set; }
    public CircleStatus Status { get; set; } = CircleStatus.Forming;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Navigation Properties
    public ICollection<Member> Members { get; set; } = new List<Member>();
    public ICollection<Round> Rounds { get; set; } = new List<Round>();
}