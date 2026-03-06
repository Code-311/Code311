namespace Code311.Licensing.Models;

/// <summary>
/// Represents a parsed Code311 license payload.
/// </summary>
public sealed record Code311License
{
    public required string LicenseId { get; init; }
    public required string CustomerName { get; init; }
    public string Plan { get; init; } = "standard";
    public DateTimeOffset? NotBeforeUtc { get; init; }
    public DateTimeOffset? ExpiresUtc { get; init; }
    public IReadOnlySet<string> Features { get; init; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
}

/// <summary>
/// Configuration options for licensing behavior.
/// </summary>
public sealed class LicensingOptions
{
    /// <summary>
    /// Indicates whether startup validation is mandatory.
    /// </summary>
    public bool RequireValidLicenseAtStartup { get; set; } = true;

    /// <summary>
    /// Number of days before expiry where validation emits warning status.
    /// </summary>
    public int ExpiryWarningWindowDays { get; set; } = 14;
}

/// <summary>
/// Indicates licensing status severity at runtime.
/// </summary>
public enum LicenseStatusLevel
{
    Valid,
    Warning,
    Error
}

/// <summary>
/// Describes lifecycle stage where a license status was observed.
/// </summary>
public enum LicenseCheckStage
{
    Startup,
    RuntimeFeature
}
