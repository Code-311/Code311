using Code311.Tabler.Core.Assets;

namespace Code311.Tabler.Widgets.Charts.Assets;

internal static class ChartWidgetAssets
{
    public static readonly IReadOnlyList<TablerAssetDescriptor> Assets =
    [
        new("_content/Code311.Tabler.Widgets.Charts/css/charts.tabler.min.css", TablerAssetType.Style, 140),
        new("_content/Code311.Tabler.Widgets.Charts/js/charts.tabler.min.js", TablerAssetType.Script, 150)
    ];
}

internal sealed class ChartAssetManifestProvider : ITablerAssetManifestProvider
{
    public IReadOnlyList<TablerAssetDescriptor> GetAssets() => ChartWidgetAssets.Assets;
}
