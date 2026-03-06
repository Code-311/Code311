using Code311.Tabler.Core.Assets;
using Code311.Tabler.Widgets.Charts.Options;
using Code311.Tabler.Widgets.Charts.Widgets;
using Xunit;

namespace Code311.Tests.Tabler.Widgets.Charts;

public sealed class ChartWidgetTests
{
    [Fact]
    public void Builder_ShouldCreateConfiguredOptions()
    {
        var options = new ChartWidgetOptionsBuilder()
            .WithType("bar")
            .ShowLegend(false)
            .UseResponsiveLayout(true)
            .Build();

        Assert.Equal("bar", options.ChartType);
        Assert.False(options.LegendVisible);
        Assert.True(options.Responsive);
    }

    [Fact]
    public void Slot_ShouldExposeAssetsAndInitialization()
    {
        var slot = new ChartWidgetSlot("chart:kpi", "panel-body", new ChartWidgetOptions());

        Assert.Contains(slot.GetAssetContributions(), x => x.Type == TablerAssetType.Style);
        Assert.Contains(slot.GetAssetContributions(), x => x.Type == TablerAssetType.Script);

        var init = slot.CreateInitialization("kpi-chart");
        Assert.Equal("chart:kpi", init.WidgetKey);
        Assert.Contains("chartType", init.OptionsJson);
    }
}
