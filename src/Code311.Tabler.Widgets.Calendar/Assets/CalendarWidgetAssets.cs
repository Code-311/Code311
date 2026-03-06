using Code311.Tabler.Core.Assets;

namespace Code311.Tabler.Widgets.Calendar.Assets;

internal static class CalendarWidgetAssets
{
    public static readonly IReadOnlyList<TablerAssetDescriptor> Assets =
    [
        new("_content/Code311.Tabler.Widgets.Calendar/css/calendar.tabler.min.css", TablerAssetType.Style, 120),
        new("_content/Code311.Tabler.Widgets.Calendar/js/calendar.tabler.min.js", TablerAssetType.Script, 130)
    ];
}

internal sealed class CalendarAssetManifestProvider : ITablerAssetManifestProvider
{
    public IReadOnlyList<TablerAssetDescriptor> GetAssets() => CalendarWidgetAssets.Assets;
}
