using System.Text.Json;
using Code311.Tabler.Core.Assets;

namespace Code311.Tabler.Core.Widgets;

public sealed record TablerWidgetOptionsEnvelope(IReadOnlyDictionary<string, object?> Values);

public sealed record TablerWidgetSlotDefinition(string WidgetKey, string SlotIntent, TablerWidgetOptionsEnvelope Options);

public sealed record TablerWidgetInitializationRequest(string WidgetKey, string ElementId, string OptionsJson);

public interface ITablerWidgetSlotParticipant
{
    TablerWidgetSlotDefinition Slot { get; }
    IReadOnlyList<TablerAssetDescriptor> GetAssetContributions();
    TablerWidgetInitializationRequest CreateInitialization(string elementId);
}

public static class TablerWidgetInitializationSerializer
{
    public static string Serialize(TablerWidgetOptionsEnvelope options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return JsonSerializer.Serialize(options.Values);
    }
}
