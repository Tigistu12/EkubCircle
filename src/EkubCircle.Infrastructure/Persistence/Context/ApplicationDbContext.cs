using EkubCircle.Application.Interfaces;
using EkubCircle.Domain.Entities;
using EkubCircle.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EkubCircle.Infrastructure.Persistence.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Circle> Circles => Set<Circle>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Round> Rounds => Set<Round>();
    public DbSet<Contribution> Contributions => Set<Contribution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Ensures Identity entities are mapped properly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}