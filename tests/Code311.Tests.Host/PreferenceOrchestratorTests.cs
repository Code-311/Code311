using Code311.Host.Services;
using Code311.Ui.Abstractions.Preferences;
using Code311.Ui.Abstractions.Semantics;
using Xunit;

namespace Code311.Tests.Host;

public sealed class PreferenceOrchestratorTests
{
    [Fact]
    public async Task GetCurrentAsync_ShouldReturnDefaults_WhenStoreMissing()
    {
        var store = new InMemoryPreferenceStore();
        var orchestrator = new PreferenceOrchestrator(store, new DemoUserContext());

        var current = await orchestrator.GetCurrentAsync();

        Assert.Equal("demo-tenant", current.TenantId);
        Assert.Equal("demo-user", current.UserId);
        Assert.Equal("default", current.Theme);
    }

    [Fact]
    public async Task SaveAsync_ShouldPersistForDemoScope()
    {
        var store = new InMemoryPreferenceStore();
        var orchestrator = new PreferenceOrchestrator(store, new DemoUserContext());

        await orchestrator.SaveAsync(new PreferenceInputModel("blue", UiDensity.Compact, 40, true));
        var loaded = await orchestrator.GetCurrentAsync();

        Assert.Equal("blue", loaded.Theme);
        Assert.Equal(40, loaded.DefaultPageSize);
        Assert.True(loaded.SidebarCollapsed);
    }

    private sealed class InMemoryPreferenceStore : IUserUiPreferenceStore
    {
        private UserUiPreference? _current;

        public Task<UserUiPreference?> GetAsync(string tenantId, string userId, CancellationToken cancellationToken = default)
            => Task.FromResult(_current is not null && _current.TenantId == tenantId && _current.UserId == userId ? _current : null);

        public Task UpsertAsync(UserUiPreference preference, CancellationToken cancellationToken = default)
        {
            _current = preference with { UpdatedAt = DateTimeOffset.UtcNow };
            return Task.CompletedTask;
        }
    }
}
