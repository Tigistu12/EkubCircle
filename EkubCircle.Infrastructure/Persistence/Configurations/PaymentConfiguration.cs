using EkubCircle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EkubCircle.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.PaidAt)
            .IsRequired();

        builder.HasIndex(p => new
        {
            p.RoundId,
            p.CircleMemberId
        })
        .IsUnique();

        builder.HasOne(p => p.Round)
            .WithMany(r => r.Payments)
            .HasForeignKey(p => p.RoundId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.CircleMember)
            .WithMany(m => m.Payments)
            .HasForeignKey(p => p.CircleMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}