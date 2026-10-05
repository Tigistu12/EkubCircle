// Domain/Entities/Member.cs
namespace EkubCircle.Domain.Entities;

public class Member
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public required int EkubId { get; set; }
    public required string FullName { get; set; }
    public int PayoutOrder { get; set; }
    public bool HasReceivedPot { get; set; }
    public bool IsDeleted { get; set; }

    public Ekub? Ekub { get; set; }
    public ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();
}