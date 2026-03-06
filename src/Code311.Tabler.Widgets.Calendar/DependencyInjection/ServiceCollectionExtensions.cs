using Code311.Tabler.Core.Assets;
using Code311.Tabler.Widgets.Calendar.Assets;
using Microsoft.Extensions.DependencyInjection;

namespace Code311.Tabler.Widgets.Calendar.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCode311TablerCalendarWidgets(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddSingleton<ITablerAssetManifestProvider, CalendarAssetManifestProvider>();
        return services;
    }
}
