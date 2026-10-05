// Domain/Entities/Ekub.cs
using EkubCircle.Domain.Enums;

namespace EkubCircle.Domain.Entities;

public class Ekub
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required decimal ContributionAmount { get; set; }
    public required string Frequency { get; set; } // "Weekly" or "Monthly"
    public int MaxMembers { get; set; }
    public int OrganizerMemberId { get; set; }
    public EkubStatus Status { get; set; } = EkubStatus.Forming;
    public bool IsDeleted { get; set; }
    public DateTime? LastUpdated { get; set; }
    public uint Version { get; set; }

    public ICollection<Member> Members { get; set; } = new List<Member>();
    public ICollection<Round> Rounds { get; set; } = new List<Round>();
}