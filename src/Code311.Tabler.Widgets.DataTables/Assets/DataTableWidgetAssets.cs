using Code311.Tabler.Core.Assets;

namespace Code311.Tabler.Widgets.DataTables.Assets;

internal static class DataTableWidgetAssets
{
    public static readonly IReadOnlyList<TablerAssetDescriptor> Assets =
    [
        new("_content/Code311.Tabler.Widgets.DataTables/css/datatables.tabler.min.css", TablerAssetType.Style, 100),
        new("_content/Code311.Tabler.Widgets.DataTables/js/datatables.tabler.min.js", TablerAssetType.Script, 110)
    ];
}

internal sealed class DataTableAssetManifestProvider : ITablerAssetManifestProvider
{
    public IReadOnlyList<TablerAssetDescriptor> GetAssets() => DataTableWidgetAssets.Assets;
}
