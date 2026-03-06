using Code311.Ui.Abstractions.Options;
using Code311.Ui.Abstractions.Theming;
using Code311.Ui.Core.Feedback;
using Code311.Ui.Core.Loading;
using Code311.Ui.Core.Theming;
using Microsoft.Extensions.DependencyInjection;

namespace Code311.Ui.Core.DependencyInjection;

/// <summary>
/// Provides dependency-injection registrations for framework-neutral Code311 core services.
/// </summary>
/// <remarks>
/// Registrations in this extension method avoid design-system concerns and only wire neutral orchestration services.
/// </remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Code311 neutral core services.
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    /// <param name="configureOptions">An optional options configuration callback.</param>
    /// <returns>The updated service collection.</returns>
    /// <remarks>
    /// This method is intentionally minimal and can be extended in later phases without changing abstraction contracts.
    /// </remarks>
    public static IServiceCollection AddCode311UiCore(
        this IServiceCollection services,
        Action<UiFrameworkOptions>? configureOptions = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (configureOptions is not null)
        {
            services.Configure(configureOptions);
        }

        services.AddOptions<UiFrameworkOptions>();
        services.AddSingleton<IThemeRegistry, ThemeRegistry>();
        services.AddSingleton<IThemeProfileResolver, DefaultThemeProfileResolver>();
        services.AddSingleton<IFeedbackChannel, InMemoryFeedbackChannel>();
        services.AddSingleton<IBusyStateCoordinator, BusyStateCoordinator>();
        services.AddSingleton<IPreloaderOrchestrator, PreloaderOrchestrator>();

        return services;
    }
}
