using Code311.Licensing.Models;

namespace Code311.Licensing.Diagnostics;

/// <summary>
/// Reports and exposes licensing runtime status.
/// </summary>
public interface ILicensingStatusReporter
{
    void Report(LicenseRuntimeStatus status);
    LicenseRuntimeStatus? Current { get; }
    IReadOnlyList<LicenseRuntimeStatus> GetHistory();
}

public sealed class InMemoryLicensingStatusReporter : ILicensingStatusReporter
{
    private readonly List<LicenseRuntimeStatus> _history = [];

    public LicenseRuntimeStatus? Current { get; private set; }

    public void Report(LicenseRuntimeStatus status)
    {
        ArgumentNullException.ThrowIfNull(status);
        Current = status;
        _history.Add(status);
    }

    public IReadOnlyList<LicenseRuntimeStatus> GetHistory() => _history;
}
