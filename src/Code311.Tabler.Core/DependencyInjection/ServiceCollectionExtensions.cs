using Code311.Ui.Abstractions.Internal.Contracts;
using Code311.Tabler.Core.Assets;
using Code311.Tabler.Core.Mapping;
using Code311.Tabler.Core.Theming;
using Microsoft.Extensions.DependencyInjection;

namespace Code311.Tabler.Core.DependencyInjection;

/// <summary>
/// Provides dependency-injection registrations for Tabler core mapping and assets.
/// </summary>
/// <remarks>
/// This extension method registers only Tabler-core foundations and no component/UI runtime implementation.
/// </remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Tabler core services.
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    /// <returns>The updated service collection.</returns>
    /// <remarks>
    /// The semantic mapper is registered as all approved Tabler mapper abstraction contracts.
    /// </remarks>
    public static IServiceCollection AddCode311TablerCore(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<TablerSemanticClassMapper>();
        services.AddSingleton<ITablerFormClassMapper>(sp => sp.GetRequiredService<TablerSemanticClassMapper>());
        services.AddSingleton<ITablerNavigationClassMapper>(sp => sp.GetRequiredService<TablerSemanticClassMapper>());
        services.AddSingleton<ITablerLayoutClassMapper>(sp => sp.GetRequiredService<TablerSemanticClassMapper>());
        services.AddSingleton<ITablerFeedbackClassMapper>(sp => sp.GetRequiredService<TablerSemanticClassMapper>());
        services.AddSingleton<ITablerDataClassMapper>(sp => sp.GetRequiredService<TablerSemanticClassMapper>());
        services.AddSingleton<ITablerMediaClassMapper>(sp => sp.GetRequiredService<TablerSemanticClassMapper>());
        services.AddSingleton<ITablerSemanticClassMapper>(sp => sp.GetRequiredService<TablerSemanticClassMapper>());

        services.AddSingleton<ITablerAssetManifestProvider, DefaultTablerAssetManifestProvider>();
        services.AddSingleton<ITablerThemeMapper, TablerThemeMapper>();

        return services;
    }
}
