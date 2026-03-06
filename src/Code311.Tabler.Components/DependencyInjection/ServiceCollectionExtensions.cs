using Microsoft.Extensions.DependencyInjection;

namespace Code311.Tabler.Components.DependencyInjection;

/// <summary>
/// Provides dependency-injection registration helpers for Code311 Tabler components.
/// </summary>
/// <remarks>
/// This registration layer wires the component package and relies on existing Ui.Core and Tabler.Core services.
/// </remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Code311 Tabler components package services.
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    /// <returns>The updated service collection.</returns>
    /// <remarks>
    /// Current component implementations are primarily TagHelpers and ViewComponents and do not require extra service registrations.
    /// </remarks>
    public static IServiceCollection AddCode311TablerComponents(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        return services;
    }
}
