using Code311.Persistence.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Code311.Persistence.EFCore.Extensions;

/// <summary>
/// Provides provider-agnostic model configuration helpers for Code311 preference entities.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies Code311 UI preference persistence mappings to a model builder.
    /// </summary>
    public static ModelBuilder ApplyCode311PreferenceStorage(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserUiPreferenceEntityConfiguration());
        return modelBuilder;
    }
}
