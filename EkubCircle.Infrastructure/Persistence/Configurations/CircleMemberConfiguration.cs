using EkubCircle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EkubCircle.Infrastructure.Persistence.Configurations;

public class CircleMemberConfiguration : IEntityTypeConfiguration<CircleMember>
{
    public void Configure(EntityTypeBuilder<CircleMember> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.UserId)
            .IsRequired();

        builder.Property(m => m.OrderNumber)
            .IsRequired();

        builder.Property(m => m.HasReceived)
            .IsRequired();

        builder.HasIndex(m => m.UserId);

        builder.HasIndex(m => new
        {
            m.CircleId,
            m.UserId
        })
        .IsUnique();

        builder.HasIndex(m => new
        {
            m.CircleId,
            m.OrderNumber
        })
        .IsUnique();

        builder.HasMany(m => m.Payments)
            .WithOne(p => p.CircleMember)
            .HasForeignKey(p => p.CircleMemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.ReceivingRounds)
            .WithOne(r => r.ReceiverMember)
            .HasForeignKey(r => r.ReceiverMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}