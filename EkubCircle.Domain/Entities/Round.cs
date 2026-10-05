using EkubCircle.Domain.Enums;

namespace EkubCircle.Domain.Entities;

public class Round
{
    public Guid Id { get; set; }

    public Guid CircleId { get; set; }

    public int RoundNumber { get; set; }

    public Guid ReceiverMemberId { get; set; }

    public RoundStatus Status { get; set; } = RoundStatus.Open;

    public decimal? PaidOutAmount { get; set; }

    public DateTime? PaidOutAt { get; set; }

    public Circle Circle { get; set; } = null!;

    public CircleMember ReceiverMember { get; set; } = null!;

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}