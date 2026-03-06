using Code311.Persistence.EFCore.Stores;
using Code311.Ui.Abstractions.Preferences;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Code311.Persistence.EFCore.DependencyInjection;

/// <summary>
/// Provides provider-agnostic service registration for Code311 EF Core persistence.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the default Code311 preference DbContext and EF-based preference store.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureDbContext">Provider-specific DbContext options configuration.</param>
    public static IServiceCollection AddCode311PersistenceEfCore(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> configureDbContext)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureDbContext);

        services.AddDbContext<Code311PreferenceDbContext>(configureDbContext);
        services.TryAddScoped<IUserUiPreferenceStore, EfCoreUserUiPreferenceStore>();

        return services;
    }
}
