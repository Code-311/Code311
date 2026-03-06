using Code311.Tabler.Core.Assets;
using Code311.Tabler.Widgets.Calendar.Options;
using Code311.Tabler.Widgets.Calendar.Widgets;
using Xunit;

namespace Code311.Tests.Tabler.Widgets.Calendar;

public sealed class CalendarWidgetTests
{
    [Fact]
    public void Builder_ShouldCreateConfiguredOptions()
    {
        var options = new CalendarWidgetOptionsBuilder()
            .WithInitialView("timeGridWeek")
            .ShowWeekends(false)
            .AllowEditing(true)
            .Build();

        Assert.Equal("timeGridWeek", options.InitialView);
        Assert.False(options.WeekendsVisible);
        Assert.True(options.Editable);
    }

    [Fact]
    public void Slot_ShouldExposeAssetsAndInitialization()
    {
        var slot = new CalendarWidgetSlot("calendar:operations", "panel-body", new CalendarWidgetOptions());

        Assert.Contains(slot.GetAssetContributions(), x => x.Type == TablerAssetType.Style);
        Assert.Contains(slot.GetAssetContributions(), x => x.Type == TablerAssetType.Script);

        var init = slot.CreateInitialization("operations-calendar");
        Assert.Equal("calendar:operations", init.WidgetKey);
        Assert.Contains("initialView", init.OptionsJson);
    }
}
