using Code311.Host.Models;
using Code311.Licensing.Diagnostics;
using Code311.Licensing.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Code311.Host.Pages.Diagnostics;

public sealed class LicensingModel(ILicensingStatusReporter reporter, ILicenseFeatureGate featureGate) : PageModel
{
    public LicensingDiagnosticsViewModel Diagnostics { get; private set; } = new(null, []);
    public Code311.Licensing.Models.LicenseFeatureCheckResult FeatureCheck { get; private set; } =
        new(false, "dashboard.advanced", Code311.Licensing.Models.LicenseStatusLevel.Error, "Not evaluated", null);

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        FeatureCheck = await featureGate.CheckFeatureAsync("dashboard.advanced", cancellationToken);
        Diagnostics = new LicensingDiagnosticsViewModel(reporter.Current, reporter.GetHistory());
    }
}
