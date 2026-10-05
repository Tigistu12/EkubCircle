using EkubCircle.Domain.Enums;

namespace EkubCircle.Domain.Entities;

public class Round
{
    public int Id { get; set; }
    public required int EkubId { get; set; }
    public required int RoundNumber { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public RoundStatus Status { get; set; } = RoundStatus.Pending;
    public int? WinnerMemberId { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation Properties
    public Ekub? Ekub { get; set; }
    public Member? WinnerMember { get; set; }
    public ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();
}