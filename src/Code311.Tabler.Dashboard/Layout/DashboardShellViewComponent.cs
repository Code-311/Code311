using Code311.Tabler.Core.Mapping;
using Code311.Tabler.Dashboard.Models;
using Code311.Ui.Abstractions.Semantics;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Code311.Tabler.Dashboard.Layout;

/// <summary>
/// Renders the top-level dashboard shell.
/// </summary>
/// <remarks>
/// Shell rendering focuses on composition regions and reuses semantic mappings from Tabler.Core.
/// </remarks>
public sealed class Cd311DashboardShellViewComponent(ITablerSemanticClassMapper mapper) : ViewComponent
{
    /// <summary>
    /// Renders a dashboard shell from page model.
    /// </summary>
    /// <param name="page">The dashboard page model.</param>
    /// <param name="layout">The semantic shell layout.</param>
    public IViewComponentResult Invoke(DashboardPageModel page, UiLayout layout = UiLayout.Grid)
    {
        var zoneHtml = string.Join(string.Empty, page.Zones.Select(z => $"<div class=\"dashboard-zone\" data-zone=\"{z.Key}\"><h2>{z.Title}</h2></div>"));
        var html = $"<section class=\"cd311-dashboard-shell {mapper.MapLayout(layout)}\"><header><h1>{page.Title}</h1></header>{zoneHtml}</section>";
        return new HtmlContentViewComponentResult(new HtmlString(html));
    }
}

/// <summary>
/// Renders a dashboard zone layout container.
/// </summary>
public sealed class Cd311DashboardZoneViewComponent(ITablerSemanticClassMapper mapper) : ViewComponent
{
    public IViewComponentResult Invoke(DashboardZoneModel zone)
    {
        var html = $"<section class=\"dashboard-zone {mapper.MapLayout(zone.Layout)}\" data-zone=\"{zone.Key}\"><h2>{zone.Title}</h2></section>";
        return new HtmlContentViewComponentResult(new HtmlString(html));
    }
}
