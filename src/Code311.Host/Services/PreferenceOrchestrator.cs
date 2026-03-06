using Code311.Ui.Abstractions.Preferences;
using Code311.Ui.Abstractions.Semantics;

namespace Code311.Host.Services;

public sealed record PreferenceInputModel(string Theme, UiDensity Density, int DefaultPageSize, bool SidebarCollapsed);

public interface IPreferenceOrchestrator
{
    Task<UserUiPreference> GetCurrentAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(PreferenceInputModel input, CancellationToken cancellationToken = default);
}

public sealed class PreferenceOrchestrator(IUserUiPreferenceStore store, IDemoUserContext userContext) : IPreferenceOrchestrator
{
    public async Task<UserUiPreference> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        var existing = await store.GetAsync(userContext.TenantId, userContext.UserId, cancellationToken);
        return existing ?? new UserUiPreference
        {
            TenantId = userContext.TenantId,
            UserId = userContext.UserId,
            Theme = "default",
            Density = UiDensity.Comfortable,
            DefaultPageSize = 25,
            SidebarCollapsed = false,
            Language = "en-US",
            TimeZone = "UTC"
        };
    }

    public async Task SaveAsync(PreferenceInputModel input, CancellationToken cancellationToken = default)
    {
        var model = new UserUiPreference
        {
            TenantId = userContext.TenantId,
            UserId = userContext.UserId,
            Theme = input.Theme,
            Density = input.Density,
            DefaultPageSize = input.DefaultPageSize,
            SidebarCollapsed = input.SidebarCollapsed,
            Language = "en-US",
            TimeZone = "UTC"
        };

        await store.UpsertAsync(model, cancellationToken);
    }
}
