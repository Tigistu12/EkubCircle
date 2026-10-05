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
            .HasMaxLength(200);

        builder.Property(c => c.Contribution)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.MeetingLabel)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Status)
            .IsRequired();

        builder.Property(c => c.OrganizerId)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.HasIndex(c => c.OrganizerId);

        builder.HasMany(c => c.Members)
            .WithOne(m => m.Circle)
            .HasForeignKey(m => m.CircleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Rounds)
            .WithOne(r => r.Circle)
            .HasForeignKey(r => r.CircleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}