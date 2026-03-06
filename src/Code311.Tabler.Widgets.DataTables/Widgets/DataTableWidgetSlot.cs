using Code311.Tabler.Core.Widgets;
using Code311.Tabler.Widgets.DataTables.Assets;
using Code311.Tabler.Widgets.DataTables.Options;

namespace Code311.Tabler.Widgets.DataTables.Widgets;

public sealed class DataTableWidgetSlot(string widgetKey, string slotIntent, DataTableWidgetOptions options) : ITablerWidgetSlotParticipant
{
    public TablerWidgetSlotDefinition Slot { get; } = new(
        widgetKey,
        slotIntent,
        new TablerWidgetOptionsEnvelope(new Dictionary<string, object?>
        {
            ["pageLength"] = options.PageLength,
            ["searchEnabled"] = options.SearchEnabled,
            ["orderingEnabled"] = options.OrderingEnabled,
            ["defaultSortColumn"] = options.DefaultSortColumn,
            ["defaultSortDirection"] = options.DefaultSortDirection
        }));

    public IReadOnlyList<Code311.Tabler.Core.Assets.TablerAssetDescriptor> GetAssetContributions() => DataTableWidgetAssets.Assets;

    public TablerWidgetInitializationRequest CreateInitialization(string elementId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(elementId);
        return new TablerWidgetInitializationRequest(Slot.WidgetKey, elementId, TablerWidgetInitializationSerializer.Serialize(Slot.Options));
    }
}
