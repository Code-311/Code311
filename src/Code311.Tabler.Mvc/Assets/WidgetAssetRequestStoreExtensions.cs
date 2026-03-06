using Code311.Tabler.Core.Assets;
using Code311.Tabler.Core.Widgets;

namespace Code311.Tabler.Mvc.Assets;

public static class WidgetAssetRequestStoreExtensions
{
    public static void AddWidgetAssets(this ICode311AssetRequestStore store, ITablerWidgetSlotParticipant widget)
    {
        ArgumentNullException.ThrowIfNull(store);
        ArgumentNullException.ThrowIfNull(widget);

        foreach (var asset in widget.GetAssetContributions().OrderBy(a => a.Order))
        {
            if (asset.Type == TablerAssetType.Script)
            {
                store.AddScript(asset.Path);
            }
            else
            {
                store.AddStyle(asset.Path);
            }
        }
    }
}
