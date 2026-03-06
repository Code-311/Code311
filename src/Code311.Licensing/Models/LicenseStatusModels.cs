namespace Code311.Licensing.Models;

/// <summary>
/// Represents a machine-readable status item produced by licensing operations.
/// </summary>
public sealed record LicenseStatusItem(LicenseStatusLevel Level, string Code, string Message);

/// <summary>
/// Represents the result of license validation.
/// </summary>
public sealed record LicenseValidationResult(
    bool IsValid,
    LicenseStatusLevel OverallLevel,
    IReadOnlyList<LicenseStatusItem> Items,
    Code311License? License);

/// <summary>
/// Represents a bounded runtime feature check result.
/// </summary>
public sealed record LicenseFeatureCheckResult(
    bool IsAllowed,
    string Feature,
    LicenseStatusLevel Level,
    string Reason,
    Code311License? License);

/// <summary>
/// Represents reported runtime status suitable for host diagnostics surfaces.
/// </summary>
public sealed record LicenseRuntimeStatus(
    DateTimeOffset OccurredAtUtc,
    LicenseCheckStage Stage,
    LicenseStatusLevel Level,
    string Code,
    string Message);
