namespace EkubCircle.Domain.Entities;

public class CircleMember
{
    public Guid Id { get; set; }

    public Guid CircleId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public int OrderNumber { get; set; }

    public bool HasReceived { get; set; }

    public Circle Circle { get; set; } = null!;

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public ICollection<Round> ReceivingRounds { get; set; } = new List<Round>();
}