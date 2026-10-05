using EkubCircle.Domain.Entities;
using EkubCircle.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EkubCircle.Infrastructure.Persistence;

public class EkubDbContext : IdentityDbContext<ApplicationUser>
{
    public EkubDbContext(DbContextOptions<EkubDbContext> options)
        : base(options)
    {
    }

    public DbSet<Circle> Circles => Set<Circle>();

    public DbSet<CircleMember> CircleMembers => Set<CircleMember>();

    public DbSet<Round> Rounds => Set<Round>();

    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(EkubDbContext).Assembly);
    }
}