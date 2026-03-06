using Code311.Tabler.Components.DependencyInjection;
using Code311.Tabler.Core.DependencyInjection;
using Code311.Tabler.Razor.Assets;
using Code311.Tabler.Razor.Feedback;
using Code311.Tabler.Razor.Filters;
using Code311.Tabler.Razor.Theming;
using Code311.Ui.Core.DependencyInjection;
using Code311.Ui.Core.Feedback;
using Code311.Ui.Core.Loading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Code311.Tabler.Razor.DependencyInjection;

/// <summary>
/// Provides Razor Pages adapter registration helpers for Code311 Tabler integration.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Code311 Tabler Razor integration services.
    /// </summary>
    public static IServiceCollection AddCode311TablerRazor(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddCode311UiCore();
        services.AddCode311TablerCore();
        services.AddCode311TablerComponents();

        services.AddScoped<IFeedbackChannel, InMemoryFeedbackChannel>();
        services.AddScoped<IBusyStateCoordinator, BusyStateCoordinator>();
        services.AddScoped<IPreloaderOrchestrator, PreloaderOrchestrator>();

        services.TryAddScoped<ICode311RequestFeedbackStore, Code311RequestFeedbackStore>();
        services.TryAddScoped<ICode311AssetRequestStore, Code311AssetRequestStore>();
        services.TryAddScoped<ICode311ThemeRequestContext, Code311ThemeRequestContext>();

        services.TryAddScoped<Code311PageFeedbackFilter>();
        services.TryAddScoped<Code311BusyTransitionPageFilter>();
        services.TryAddScoped<Code311ThemePageFilter>();

        services.AddHttpContextAccessor();
        services.AddTransient<IConfigureOptions<MvcOptions>, ConfigureCode311RazorOptions>();

        return services;
    }
}

internal sealed class ConfigureCode311RazorOptions : IConfigureOptions<MvcOptions>
{
    public void Configure(MvcOptions options)
    {
        options.Filters.AddService<Code311PageFeedbackFilter>();
        options.Filters.AddService<Code311BusyTransitionPageFilter>();
        options.Filters.AddService<Code311ThemePageFilter>();
    }
}
