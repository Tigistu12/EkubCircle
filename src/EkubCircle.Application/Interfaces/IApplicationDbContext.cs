using EkubCircle.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EkubCircle.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Circle> Circles { get; }
    DbSet<Member> Members { get; }
    DbSet<Round> Rounds { get; }
    DbSet<Contribution> Contributions { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}