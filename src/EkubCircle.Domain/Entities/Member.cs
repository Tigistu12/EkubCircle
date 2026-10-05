namespace EkubCircle.Domain.Entities;

public class Member
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public required string FullName { get; set; }
    public int CircleId { get; set; }
    public int PayoutOrder { get; set; } // Set when Circle starts
    public bool HasReceived { get; set; } = false; // Flag to track if pot taken
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Circle? Circle { get; set; }
    public ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();
}