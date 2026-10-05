using EkubCircle.Domain.Enums;

namespace EkubCircle.Domain.Entities;

public class Ekub
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required decimal ContributionAmount { get; set; }
    public required int MaxMembers { get; set; }
    public EkubStatus Status { get; set; } = EkubStatus.Pending;
    public bool IsDeleted { get; set; }
    public DateTime? LastUpdated { get; set; }
    public uint Version { get; set; }

    // Navigation Properties
    public ICollection<Member> Members { get; set; } = new List<Member>();
    public ICollection<Round> Rounds { get; set; } = new List<Round>();
}