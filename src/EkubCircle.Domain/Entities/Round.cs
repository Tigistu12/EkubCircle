using EkubCircle.Domain.Enums;
namespace EkubCircle.Domain.Entities;

public class Round
{
    public int Id { get; set; }
    public int CircleId { get; set; }
    public int RoundNumber { get; set; }
    public RoundStatus Status { get; set; } = RoundStatus.Open;
    
    // Single designated winner/receiver for the round
    public int? ReceiverMemberId { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidOutAt { get; set; }

    // Navigation Properties
    public Circle? Circle { get; set; }
    public Member? ReceiverMember { get; set; }
    public ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();
}