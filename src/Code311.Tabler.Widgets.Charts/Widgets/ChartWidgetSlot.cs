using Code311.Tabler.Core.Widgets;
using Code311.Tabler.Widgets.Charts.Assets;
using Code311.Tabler.Widgets.Charts.Options;

namespace Code311.Tabler.Widgets.Charts.Widgets;

public sealed class ChartWidgetSlot(string widgetKey, string slotIntent, ChartWidgetOptions options) : ITablerWidgetSlotParticipant
{
    public TablerWidgetSlotDefinition Slot { get; } = new(
        widgetKey,
        slotIntent,
        new TablerWidgetOptionsEnvelope(new Dictionary<string, object?>
        {
            ["chartType"] = options.ChartType,
            ["legendVisible"] = options.LegendVisible,
            ["responsive"] = options.Responsive
        }));

    public IReadOnlyList<Code311.Tabler.Core.Assets.TablerAssetDescriptor> GetAssetContributions() => ChartWidgetAssets.Assets;

    public TablerWidgetInitializationRequest CreateInitialization(string elementId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(elementId);
        return new TablerWidgetInitializationRequest(Slot.WidgetKey, elementId, TablerWidgetInitializationSerializer.Serialize(Slot.Options));
    }
}
