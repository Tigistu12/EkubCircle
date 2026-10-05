namespace EkubCircle.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }

    public Guid RoundId { get; set; }

    public Guid CircleMemberId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaidAt { get; set; }

    public Round Round { get; set; } = null!;

    public CircleMember CircleMember { get; set; } = null!;
}