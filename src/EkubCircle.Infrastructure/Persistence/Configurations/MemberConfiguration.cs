using EkubCircle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EkubCircle.Infrastructure.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.UserId)
            .IsRequired();

        builder.Property(m => m.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(m => m.PayoutOrder)
            .HasDefaultValue(0);

        builder.Property(m => m.HasReceived)
            .HasDefaultValue(false);

        // Unique constraint: Prevent duplicate membership for the same user in a circle
        builder.HasIndex(m => new { m.CircleId, m.UserId })
            .IsUnique();

        builder.HasMany(m => m.Contributions)
            .WithOne(c => c.Member)
            .HasForeignKey(c => c.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}