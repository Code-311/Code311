using Code311.Tabler.Core.Widgets;
using Code311.Tabler.Widgets.Calendar.Assets;
using Code311.Tabler.Widgets.Calendar.Options;

namespace Code311.Tabler.Widgets.Calendar.Widgets;

public sealed class CalendarWidgetSlot(string widgetKey, string slotIntent, CalendarWidgetOptions options) : ITablerWidgetSlotParticipant
{
    public TablerWidgetSlotDefinition Slot { get; } = new(
        widgetKey,
        slotIntent,
        new TablerWidgetOptionsEnvelope(new Dictionary<string, object?>
        {
            ["initialView"] = options.InitialView,
            ["weekendsVisible"] = options.WeekendsVisible,
            ["editable"] = options.Editable
        }));

    public IReadOnlyList<Code311.Tabler.Core.Assets.TablerAssetDescriptor> GetAssetContributions() => CalendarWidgetAssets.Assets;

    public TablerWidgetInitializationRequest CreateInitialization(string elementId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(elementId);
        return new TablerWidgetInitializationRequest(Slot.WidgetKey, elementId, TablerWidgetInitializationSerializer.Serialize(Slot.Options));
    }
}
