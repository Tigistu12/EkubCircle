using EkubCircle.Domain.Enums;
namespace EkubCircle.Domain.Entities;

public class Contribution
{
    public int Id { get; set; }
    public int RoundId { get; set; }
    public int MemberId { get; set; }
    public decimal Amount { get; set; }
    public ContributionStatus Status { get; set; } = ContributionStatus.Pending;
    public DateTime? PaidAt { get; set; }

    // Navigation Properties
    public Round? Round { get; set; }
    public Member? Member { get; set; }
}