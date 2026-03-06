using Code311.Host.Models;
using Code311.Licensing.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Code311.Host.Controllers;

public sealed class DashboardController(ILicenseFeatureGate featureGate) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var check = await featureGate.CheckFeatureAsync("dashboard.advanced", cancellationToken);
        var model = new DashboardDemoViewModel(check.IsAllowed, check.Reason);
        return View(model);
    }
}
