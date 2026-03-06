using Code311.Tabler.Components.DependencyInjection;
using Code311.Tabler.Core.DependencyInjection;
using Code311.Tabler.Mvc.Assets;
using Code311.Tabler.Mvc.Feedback;
using Code311.Tabler.Mvc.Filters;
using Code311.Tabler.Mvc.Theming;
using Code311.Ui.Core.DependencyInjection;
using Code311.Ui.Core.Feedback;
using Code311.Ui.Core.Loading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Code311.Tabler.Mvc.DependencyInjection;

/// <summary>
/// Provides MVC adapter registration helpers for Code311 Tabler integration.
/// </summary>
/// <remarks>
/// The adapter is intentionally thin and wires existing core services into MVC request lifecycle hooks.
/// </remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Code311 Tabler MVC integration services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    /// <remarks>
    /// This method wires Ui.Core, Tabler.Core, Tabler.Components and MVC lifecycle filters.
    /// </remarks>
    public static IServiceCollection AddCode311TablerMvc(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddCode311UiCore();
        services.AddCode311TablerCore();
        services.AddCode311TablerComponents();

        // Override default singleton orchestration with request-scoped instances for deterministic host behavior.
        services.AddScoped<IFeedbackChannel, InMemoryFeedbackChannel>();
        services.AddScoped<IBusyStateCoordinator, BusyStateCoordinator>();
        services.AddScoped<IPreloaderOrchestrator, PreloaderOrchestrator>();

        services.TryAddScoped<ICode311RequestFeedbackStore, Code311RequestFeedbackStore>();
        services.TryAddScoped<ICode311AssetRequestStore, Code311AssetRequestStore>();
        services.TryAddScoped<ICode311ThemeRequestContext, Code311ThemeRequestContext>();

        services.TryAddScoped<Code311FeedbackActionFilter>();
        services.TryAddScoped<Code311BusyTransitionFilter>();
        services.TryAddScoped<Code311ThemeContextFilter>();

        services.AddHttpContextAccessor();
        services.AddTransient<IConfigureOptions<MvcOptions>, ConfigureCode311MvcOptions>();

        return services;
    }
}

internal sealed class ConfigureCode311MvcOptions(
    IServiceProvider serviceProvider) : IConfigureOptions<MvcOptions>
{
    public void Configure(MvcOptions options)
    {
        options.Filters.AddService<Code311FeedbackActionFilter>();
        options.Filters.AddService<Code311BusyTransitionFilter>();
        options.Filters.AddService<Code311ThemeContextFilter>();
    }
}
