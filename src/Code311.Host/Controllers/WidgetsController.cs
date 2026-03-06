using Code311.Host.Models;
using Code311.Tabler.Mvc.Assets;
using Code311.Tabler.Widgets.Calendar.Options;
using Code311.Tabler.Widgets.Calendar.Widgets;
using Code311.Tabler.Widgets.Charts.Options;
using Code311.Tabler.Widgets.Charts.Widgets;
using Code311.Tabler.Widgets.DataTables.Options;
using Code311.Tabler.Widgets.DataTables.Widgets;
using Microsoft.AspNetCore.Mvc;

namespace Code311.Host.Controllers;

public sealed class WidgetsController(ICode311AssetRequestStore assets) : Controller
{
    public IActionResult DataTables()
    {
        var widget = new DataTableWidgetSlot(
            "widget.datatables.incidents",
            "panel-body",
            new DataTableWidgetOptionsBuilder().WithPageLength(25).EnableSearch(true).Build());

        assets.AddWidgetAssets(widget);
        var init = widget.CreateInitialization("datatable-demo");
        return View(new WidgetDemoViewModel("DataTables", init.ElementId, init.OptionsJson));
    }

    public IActionResult Calendar()
    {
        var widget = new CalendarWidgetSlot(
            "widget.calendar.operations",
            "panel-body",
            new CalendarWidgetOptionsBuilder().WithInitialView("dayGridMonth").ShowWeekends(true).Build());

        assets.AddWidgetAssets(widget);
        var init = widget.CreateInitialization("calendar-demo");
        return View("WidgetPage", new WidgetDemoViewModel("Calendar", init.ElementId, init.OptionsJson));
    }

    public IActionResult Charts()
    {
        var widget = new ChartWidgetSlot(
            "widget.charts.kpi",
            "panel-body",
            new ChartWidgetOptionsBuilder().WithType("line").ShowLegend(true).Build());

        assets.AddWidgetAssets(widget);
        var init = widget.CreateInitialization("charts-demo");
        return View("WidgetPage", new WidgetDemoViewModel("Charts", init.ElementId, init.OptionsJson));
    }
}
