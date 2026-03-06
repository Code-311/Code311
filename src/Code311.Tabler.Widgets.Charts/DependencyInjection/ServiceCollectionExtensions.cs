using Code311.Tabler.Core.Assets;
using Code311.Tabler.Widgets.Charts.Assets;
using Microsoft.Extensions.DependencyInjection;

namespace Code311.Tabler.Widgets.Charts.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCode311TablerChartWidgets(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddSingleton<ITablerAssetManifestProvider, ChartAssetManifestProvider>();
        return services;
    }
}
