using Code311.Tabler.Core.Assets;
using Code311.Tabler.Widgets.DataTables.Options;
using Code311.Tabler.Widgets.DataTables.Widgets;
using Xunit;

namespace Code311.Tests.Tabler.Widgets.DataTables;

public sealed class DataTableWidgetTests
{
    [Fact]
    public void Builder_ShouldCreateConfiguredOptions()
    {
        var options = new DataTableWidgetOptionsBuilder()
            .WithPageLength(50)
            .EnableSearch(false)
            .WithDefaultSort("createdAt", "desc")
            .Build();

        Assert.Equal(50, options.PageLength);
        Assert.False(options.SearchEnabled);
        Assert.Equal("createdAt", options.DefaultSortColumn);
        Assert.Equal("desc", options.DefaultSortDirection);
    }

    [Fact]
    public void Slot_ShouldExposeAssetsAndInitialization()
    {
        var slot = new DataTableWidgetSlot("datatable:incidents", "panel-body", new DataTableWidgetOptions());

        Assert.Contains(slot.GetAssetContributions(), x => x.Type == TablerAssetType.Style);
        Assert.Contains(slot.GetAssetContributions(), x => x.Type == TablerAssetType.Script);

        var init = slot.CreateInitialization("incidents-table");
        Assert.Equal("datatable:incidents", init.WidgetKey);
        Assert.Contains("pageLength", init.OptionsJson);
    }
}
