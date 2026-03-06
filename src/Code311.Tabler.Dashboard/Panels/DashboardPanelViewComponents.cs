using Code311.Tabler.Components.Common;
using Code311.Tabler.Core.Mapping;
using Code311.Tabler.Dashboard.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Code311.Tabler.Dashboard.Panels;

/// <summary>
/// Renders a semantic metric card panel.
/// </summary>
public sealed class Cd311MetricCardViewComponent(ITablerSemanticClassMapper mapper) : ViewComponent
{
    public IViewComponentResult Invoke(DashboardPanelModel panel)
    {
        var html = $"<article class=\"card border-{mapper.MapTone(panel.Tone)}\" data-panel=\"{panel.PanelKey}\"><header>{panel.Title}</header><div>{panel.BodyHtml}</div></article>";
        return new HtmlContentViewComponentResult(new HtmlString(html));
    }
}

/// <summary>
/// Renders an activity feed panel.
/// </summary>
public sealed class Cd311ActivityPanelViewComponent(ITablerSemanticClassMapper mapper) : ViewComponent
{
    public IViewComponentResult Invoke(string title, IReadOnlyCollection<DashboardActivityItem>? items)
    {
        var htmlItems = string.Join(string.Empty, (items ?? []).Select(i => $"<li class=\"text-{mapper.MapTone(i.Tone)}\">{i.Text} <small>{i.OccurredAt:O}</small></li>"));
        return new HtmlContentViewComponentResult(new HtmlString($"<section class=\"activity-panel\"><h3>{title}</h3><ul>{htmlItems}</ul></section>"));
    }
}

/// <summary>
/// Renders a quick actions panel.
/// </summary>
public sealed class Cd311QuickActionsPanelViewComponent(ITablerSemanticClassMapper mapper) : ViewComponent
{
    public IViewComponentResult Invoke(string title, IReadOnlyCollection<DashboardQuickAction>? actions)
    {
        var htmlItems = string.Join(string.Empty, (actions ?? []).Select(a => $"<button class=\"btn btn-{mapper.MapTone(a.Tone)}\">{a.Action.Text}</button>"));
        return new HtmlContentViewComponentResult(new HtmlString($"<section class=\"quick-actions-panel\"><h3>{title}</h3><div>{htmlItems}</div></section>"));
    }
}
