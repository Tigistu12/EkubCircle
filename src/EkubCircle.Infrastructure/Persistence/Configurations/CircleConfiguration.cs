using EkubCircle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EkubCircle.Infrastructure.Persistence.Configurations;

public class CircleConfiguration : IEntityTypeConfiguration<Circle>
{
    public void Configure(EntityTypeBuilder<Circle> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.ContributionAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.MeetingLabel)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.OrganizerUserId)
            .IsRequired();

        builder.Property(c => c.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasMany(c => c.Members)
            .WithOne(m => m.Circle)
            .HasForeignKey(m => m.CircleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Rounds)
            .WithOne(r => r.Circle)
            .HasForeignKey(r => r.CircleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}