using Code311.Persistence.EFCore.Entities;
using Code311.Ui.Abstractions.Preferences;
using Microsoft.EntityFrameworkCore;

namespace Code311.Persistence.EFCore.Stores;

/// <summary>
/// Provider-agnostic EF Core implementation of <see cref="IUserUiPreferenceStore"/>.
/// </summary>
public sealed class EfCoreUserUiPreferenceStore(Code311PreferenceDbContext dbContext) : IUserUiPreferenceStore
{
    public async Task<UserUiPreference?> GetAsync(string tenantId, string userId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        var entity = await dbContext.UserUiPreferences
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.UserId == userId, cancellationToken)
            .ConfigureAwait(false);

        return entity is null ? null : ToModel(entity);
    }

    public async Task UpsertAsync(UserUiPreference preference, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(preference);
        ArgumentException.ThrowIfNullOrWhiteSpace(preference.TenantId);
        ArgumentException.ThrowIfNullOrWhiteSpace(preference.UserId);
        ArgumentException.ThrowIfNullOrWhiteSpace(preference.Theme);

        var existing = await dbContext.UserUiPreferences
            .SingleOrDefaultAsync(x => x.TenantId == preference.TenantId && x.UserId == preference.UserId, cancellationToken)
            .ConfigureAwait(false);

        var utcNow = DateTimeOffset.UtcNow;

        if (existing is null)
        {
            dbContext.UserUiPreferences.Add(ToEntity(preference, utcNow));
        }
        else
        {
            existing.Theme = preference.Theme;
            existing.Density = preference.Density;
            existing.SidebarCollapsed = preference.SidebarCollapsed;
            existing.DefaultPageSize = preference.DefaultPageSize;
            existing.Language = preference.Language;
            existing.TimeZone = preference.TimeZone;
            existing.UpdatedAt = utcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private static UserUiPreference ToModel(UserUiPreferenceEntity entity)
        => new()
        {
            TenantId = entity.TenantId,
            UserId = entity.UserId,
            Theme = entity.Theme,
            Density = entity.Density,
            SidebarCollapsed = entity.SidebarCollapsed,
            DefaultPageSize = entity.DefaultPageSize,
            Language = entity.Language,
            TimeZone = entity.TimeZone,
            UpdatedAt = entity.UpdatedAt
        };

    private static UserUiPreferenceEntity ToEntity(UserUiPreference model, DateTimeOffset utcNow)
        => new()
        {
            TenantId = model.TenantId,
            UserId = model.UserId,
            Theme = model.Theme,
            Density = model.Density,
            SidebarCollapsed = model.SidebarCollapsed,
            DefaultPageSize = model.DefaultPageSize,
            Language = model.Language,
            TimeZone = model.TimeZone,
            UpdatedAt = utcNow
        };
}
