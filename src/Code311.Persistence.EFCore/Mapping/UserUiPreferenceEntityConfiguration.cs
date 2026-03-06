using Code311.Persistence.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Code311.Persistence.EFCore.Mapping;

internal sealed class UserUiPreferenceEntityConfiguration : IEntityTypeConfiguration<UserUiPreferenceEntity>
{
    public void Configure(EntityTypeBuilder<UserUiPreferenceEntity> builder)
    {
        builder.ToTable("UserUiPreferences");

        builder.HasKey(x => new { x.TenantId, x.UserId });

        builder.Property(x => x.TenantId)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.Theme)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.Density)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.DefaultPageSize)
            .IsRequired();

        builder.Property(x => x.Language)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.TimeZone)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();
    }
}
