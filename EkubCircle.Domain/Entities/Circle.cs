using EkubCircle.Domain.Enums;

namespace EkubCircle.Domain.Entities;

public class Circle
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Contribution { get; set; }

    public string MeetingLabel { get; set; } = string.Empty;

    public CircleStatus Status { get; set; } = CircleStatus.Forming;

    public string OrganizerId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public ICollection<CircleMember> Members { get; set; } = new List<CircleMember>();

    public ICollection<Round> Rounds { get; set; } = new List<Round>();
}