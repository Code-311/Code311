using Code311.Host.Models;
using Code311.Host.Services;
using Code311.Ui.Abstractions.Semantics;
using Microsoft.AspNetCore.Mvc;

namespace Code311.Host.Controllers;

public sealed class PreferencesController(IPreferenceOrchestrator orchestrator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var current = await orchestrator.GetCurrentAsync(cancellationToken);
        return View(new PreferencesPageViewModel(current, string.Empty));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(string theme, UiDensity density, int defaultPageSize, bool sidebarCollapsed, CancellationToken cancellationToken)
    {
        await orchestrator.SaveAsync(new PreferenceInputModel(theme, density, defaultPageSize, sidebarCollapsed), cancellationToken);
        var current = await orchestrator.GetCurrentAsync(cancellationToken);
        return View("Index", new PreferencesPageViewModel(current, "Preferences saved for demo-tenant/demo-user."));
    }
}
