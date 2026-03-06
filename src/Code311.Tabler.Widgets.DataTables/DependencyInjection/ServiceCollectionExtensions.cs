using Code311.Tabler.Core.Assets;
using Code311.Tabler.Widgets.DataTables.Assets;
using Microsoft.Extensions.DependencyInjection;

namespace Code311.Tabler.Widgets.DataTables.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCode311TablerDataTablesWidgets(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddSingleton<ITablerAssetManifestProvider, DataTableAssetManifestProvider>();
        return services;
    }
}
