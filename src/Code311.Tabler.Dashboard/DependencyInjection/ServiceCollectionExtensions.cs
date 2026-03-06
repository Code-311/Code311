using Code311.Tabler.Components.DependencyInjection;
using Code311.Tabler.Core.DependencyInjection;
using Code311.Tabler.Dashboard.Composition;
using Code311.Ui.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Code311.Tabler.Dashboard.DependencyInjection;

/// <summary>
/// Provides dependency-injection registrations for Code311 Tabler dashboard composition services.
/// </summary>
/// <remarks>
/// Registration composes existing Ui/Core/Components packages without introducing widget or persistence dependencies.
/// </remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers dashboard composition services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddCode311TablerDashboard(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddCode311UiCore();
        services.AddCode311TablerCore();
        services.AddCode311TablerComponents();

        services.TryAddSingleton<IDashboardPageComposer, DefaultDashboardPageComposer>();

        return services;
    }
}
