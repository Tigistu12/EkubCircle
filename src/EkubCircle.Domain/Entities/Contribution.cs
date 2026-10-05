using EkubCircle.Domain.Enums;

namespace EkubCircle.Domain.Entities;

public class Contribution
{
    public int Id { get; set; }
    public required int RoundId { get; set; }
    public required int MemberId { get; set; }
    public required decimal Amount { get; set; }
    public ContributionStatus Status { get; set; } = ContributionStatus.Pending;
    public DateTime? PaidAt { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation Properties
    public Round? Round { get; set; }
    public Member? Member { get; set; }
}