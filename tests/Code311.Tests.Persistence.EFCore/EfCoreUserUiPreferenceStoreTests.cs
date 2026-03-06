using Code311.Persistence.EFCore.DependencyInjection;
using Code311.Ui.Abstractions.Preferences;
using Code311.Ui.Abstractions.Semantics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Code311.Tests.Persistence.EFCore;

public sealed class EfCoreUserUiPreferenceStoreTests
{
    [Fact]
    public async Task UpsertAndGet_ShouldRoundTripPreference()
    {
        await using var provider = BuildServiceProvider();
        using var scope = provider.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IUserUiPreferenceStore>();

        var preference = CreatePreference("tenant-a", "user-1", "dark");
        await store.UpsertAsync(preference);

        var loaded = await store.GetAsync("tenant-a", "user-1");

        Assert.NotNull(loaded);
        Assert.Equal("dark", loaded!.Theme);
        Assert.Equal(UiDensity.Compact, loaded.Density);
    }

    [Fact]
    public async Task Get_ShouldRespectTenantAndUserScope()
    {
        await using var provider = BuildServiceProvider();
        using var scope = provider.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IUserUiPreferenceStore>();

        await store.UpsertAsync(CreatePreference("tenant-a", "user-1", "dark"));
        await store.UpsertAsync(CreatePreference("tenant-b", "user-1", "light"));

        var tenantA = await store.GetAsync("tenant-a", "user-1");
        var tenantB = await store.GetAsync("tenant-b", "user-1");
        var missing = await store.GetAsync("tenant-c", "user-1");

        Assert.Equal("dark", tenantA!.Theme);
        Assert.Equal("light", tenantB!.Theme);
        Assert.Null(missing);
    }

    [Fact]
    public async Task Upsert_ShouldUpdateExistingRowForSameTenantAndUser()
    {
        await using var provider = BuildServiceProvider();
        using var scope = provider.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IUserUiPreferenceStore>();

        await store.UpsertAsync(CreatePreference("tenant-a", "user-1", "dark"));
        await Task.Delay(5);
        await store.UpsertAsync(CreatePreference("tenant-a", "user-1", "light") with { SidebarCollapsed = false });

        var loaded = await store.GetAsync("tenant-a", "user-1");

        Assert.NotNull(loaded);
        Assert.Equal("light", loaded!.Theme);
        Assert.False(loaded.SidebarCollapsed);
    }

    [Fact]
    public void AddCode311PersistenceEfCore_ShouldRegisterStore()
    {
        var services = new ServiceCollection();

        services.AddCode311PersistenceEfCore(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString("N")));

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        Assert.NotNull(scope.ServiceProvider.GetService<IUserUiPreferenceStore>());
    }

    private static ServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddCode311PersistenceEfCore(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString("N")));
        return services.BuildServiceProvider();
    }

    private static UserUiPreference CreatePreference(string tenantId, string userId, string theme)
        => new()
        {
            TenantId = tenantId,
            UserId = userId,
            Theme = theme,
            Density = UiDensity.Compact,
            SidebarCollapsed = true,
            DefaultPageSize = 50,
            Language = "en-US",
            TimeZone = "UTC"
        };
}
