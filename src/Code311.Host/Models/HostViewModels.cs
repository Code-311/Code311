using Code311.Licensing.Models;
using Code311.Ui.Abstractions.Preferences;

namespace Code311.Host.Models;

public sealed record DashboardDemoViewModel(bool AdvancedEnabled, string FeatureReason);

public sealed record WidgetDemoViewModel(string WidgetName, string ElementId, string InitializationJson);

public sealed record LicensingDiagnosticsViewModel(LicenseRuntimeStatus? Current, IReadOnlyList<LicenseRuntimeStatus> History);

public sealed record PreferencesPageViewModel(UserUiPreference Current, string Message);
