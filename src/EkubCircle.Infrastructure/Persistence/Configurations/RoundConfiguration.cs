using EkubCircle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EkubCircle.Infrastructure.Persistence.Configurations;

public class RoundConfiguration : IEntityTypeConfiguration<Round>
{
    public void Configure(EntityTypeBuilder<Round> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.RoundNumber)
            .IsRequired();

        builder.Property(r => r.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(r => r.ReceiverMember)
            .WithMany()
            .HasForeignKey(r => r.ReceiverMemberId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(r => r.Contributions)
            .WithOne(c => c.Round)
            .HasForeignKey(c => c.RoundId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}