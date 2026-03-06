using Code311.Tabler.Core.Mapping;
using Code311.Tabler.Dashboard.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace Code311.Tabler.Dashboard.Kpi;

/// <summary>
/// Renders KPI summary surfaces for dashboard pages.
/// </summary>
public sealed class Cd311KpiSummaryViewComponent(ITablerSemanticClassMapper mapper) : ViewComponent
{
    public IViewComponentResult Invoke(string title, IReadOnlyCollection<DashboardKpiItem>? items)
    {
        var htmlItems = string.Join(string.Empty, (items ?? []).Select(i =>
            $"<div class=\"kpi-item text-{mapper.MapTone(i.Tone)}\"><div>{i.Label}</div><strong>{i.Value}</strong><small>{i.Trend}</small></div>"));

        return new HtmlContentViewComponentResult(new HtmlString($"<section class=\"kpi-summary\"><h3>{title}</h3><div class=\"kpi-grid\">{htmlItems}</div></section>"));
    }
}
