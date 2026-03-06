using System.IO;
using Code311.Tabler.Core.Mapping;
using Code311.Tabler.Dashboard.Composition;
using Code311.Tabler.Dashboard.Kpi;
using Code311.Tabler.Dashboard.Layout;
using Code311.Tabler.Dashboard.Models;
using Code311.Tabler.Dashboard.Panels;
using Code311.Ui.Abstractions.Semantics;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Xunit;

namespace Code311.Tests.Tabler.Dashboard;

/// <summary>
/// Validates dashboard composition and panel rendering surfaces.
/// </summary>
public sealed class DashboardCompositionTests
{
    [Fact]
    public void DashboardComposer_ShouldComposeZonesAndPanels()
    {
        IDashboardPageComposer composer = new DefaultDashboardPageComposer();
        var page = new DashboardPageModel(
            "Operations",
            [
                new DashboardZoneModel(
                    "top",
                    "Top Zone",
                    UiLayout.Grid,
                    [new DashboardPanelModel("p1", "Revenue", "<div>$100</div>", UiTone.Success)])
            ]);

        var html = composer.Compose(page);

        Assert.Contains("Operations", html);
        Assert.Contains("Top Zone", html);
        Assert.Contains("$100", html);
    }

    [Fact]
    public void DashboardShell_ShouldRenderShellClasses()
    {
        var component = new Cd311DashboardShellViewComponent(new TablerSemanticClassMapper());
        var result = Assert.IsType<HtmlContentViewComponentResult>(component.Invoke(new DashboardPageModel("Sales", []), UiLayout.Grid));

        using var writer = new StringWriter();
        result.HtmlContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
        var html = writer.ToString();

        Assert.Contains("cd311-dashboard-shell", html);
        Assert.Contains("Sales", html);
    }

    [Fact]
    public void KpiSummary_ShouldRenderToneMappedItems()
    {
        var component = new Cd311KpiSummaryViewComponent(new TablerSemanticClassMapper());
        var result = Assert.IsType<HtmlContentViewComponentResult>(component.Invoke("KPIs", [new DashboardKpiItem("Users", "100", UiTone.Info, "+5%") ]));

        using var writer = new StringWriter();
        result.HtmlContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
        var html = writer.ToString();

        Assert.Contains("text-info", html);
        Assert.Contains("Users", html);
    }

    [Fact]
    public void QuickActionsPanel_ShouldRenderButtons()
    {
        var component = new Cd311QuickActionsPanelViewComponent(new TablerSemanticClassMapper());
        var result = Assert.IsType<HtmlContentViewComponentResult>(component.Invoke("Actions", [new DashboardQuickAction(new("Create"), UiTone.Accent)]));

        using var writer = new StringWriter();
        result.HtmlContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
        var html = writer.ToString();

        Assert.Contains("btn-primary", html);
        Assert.Contains("Create", html);
    }

    [Fact]
    public void ActivityPanel_ShouldRenderFeedItems()
    {
        var component = new Cd311ActivityPanelViewComponent(new TablerSemanticClassMapper());
        var result = Assert.IsType<HtmlContentViewComponentResult>(component.Invoke("Activity", [new DashboardActivityItem("Imported orders", DateTimeOffset.UtcNow, UiTone.Warning)]));

        using var writer = new StringWriter();
        result.HtmlContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
        var html = writer.ToString();

        Assert.Contains("Imported orders", html);
        Assert.Contains("text-warning", html);
    }
}
