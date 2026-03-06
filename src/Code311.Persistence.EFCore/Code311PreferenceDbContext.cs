using Code311.Persistence.EFCore.Entities;
using Code311.Persistence.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Code311.Persistence.EFCore;

/// <summary>
/// Default EF Core DbContext for Code311 persistence features.
/// </summary>
public sealed class Code311PreferenceDbContext(DbContextOptions<Code311PreferenceDbContext> options) : DbContext(options)
{
    public DbSet<UserUiPreferenceEntity> UserUiPreferences => Set<UserUiPreferenceEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyCode311PreferenceStorage();
        base.OnModelCreating(modelBuilder);
    }
}
