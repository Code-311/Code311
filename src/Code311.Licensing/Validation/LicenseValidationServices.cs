using Code311.Licensing.Diagnostics;
using Code311.Licensing.Models;
using Code311.Licensing.Sources;

namespace Code311.Licensing.Validation;

/// <summary>
/// Validates license payload semantics.
/// </summary>
public interface ILicenseValidator
{
    LicenseValidationResult Validate(Code311License? license, DateTimeOffset nowUtc, LicensingOptions options);
}

/// <summary>
/// Provides explicit startup validation flow.
/// </summary>
public interface IStartupLicenseValidator
{
    Task<LicenseValidationResult> ValidateAtStartupAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides bounded runtime feature-level checks for integration points.
/// </summary>
public interface ILicenseFeatureGate
{
    Task<LicenseFeatureCheckResult> CheckFeatureAsync(string feature, CancellationToken cancellationToken = default);
}

public sealed class DefaultLicenseValidator : ILicenseValidator
{
    public LicenseValidationResult Validate(Code311License? license, DateTimeOffset nowUtc, LicensingOptions options)
    {
        var items = new List<LicenseStatusItem>();

        if (license is null)
        {
            items.Add(new LicenseStatusItem(LicenseStatusLevel.Error, "license.missing", "No license payload could be resolved."));
            return new LicenseValidationResult(false, LicenseStatusLevel.Error, items, null);
        }

        if (license.NotBeforeUtc.HasValue && nowUtc < license.NotBeforeUtc.Value)
        {
            items.Add(new LicenseStatusItem(LicenseStatusLevel.Error, "license.not_before", "License is not active yet."));
        }

        if (license.ExpiresUtc.HasValue)
        {
            if (nowUtc >= license.ExpiresUtc.Value)
            {
                items.Add(new LicenseStatusItem(LicenseStatusLevel.Error, "license.expired", "License has expired."));
            }
            else if (nowUtc >= license.ExpiresUtc.Value.AddDays(-Math.Abs(options.ExpiryWarningWindowDays)))
            {
                items.Add(new LicenseStatusItem(LicenseStatusLevel.Warning, "license.expiring_soon", "License is approaching expiry."));
            }
        }

        if (items.All(x => x.Level != LicenseStatusLevel.Error))
        {
            items.Add(new LicenseStatusItem(LicenseStatusLevel.Valid, "license.valid", "License is valid."));
        }

        var overall = items.Any(x => x.Level == LicenseStatusLevel.Error)
            ? LicenseStatusLevel.Error
            : items.Any(x => x.Level == LicenseStatusLevel.Warning)
                ? LicenseStatusLevel.Warning
                : LicenseStatusLevel.Valid;

        return new LicenseValidationResult(overall != LicenseStatusLevel.Error, overall, items, license);
    }
}

public sealed class StartupLicenseValidator(
    ILicenseSource source,
    ILicenseValidator validator,
    ILicensingStatusReporter reporter,
    LicensingOptions options) : IStartupLicenseValidator
{
    public async Task<LicenseValidationResult> ValidateAtStartupAsync(CancellationToken cancellationToken = default)
    {
        var license = await source.GetLicenseAsync(cancellationToken).ConfigureAwait(false);
        var result = validator.Validate(license, DateTimeOffset.UtcNow, options);

        var top = result.Items.FirstOrDefault() ?? new LicenseStatusItem(result.OverallLevel, "license.status", "License status generated.");
        reporter.Report(new LicenseRuntimeStatus(DateTimeOffset.UtcNow, LicenseCheckStage.Startup, result.OverallLevel, top.Code, top.Message));

        if (options.RequireValidLicenseAtStartup && !result.IsValid)
        {
            throw new InvalidOperationException("Code311 startup license validation failed.");
        }

        return result;
    }
}

public sealed class LicenseFeatureGate(
    ILicenseSource source,
    ILicenseValidator validator,
    ILicensingStatusReporter reporter,
    LicensingOptions options) : ILicenseFeatureGate
{
    public async Task<LicenseFeatureCheckResult> CheckFeatureAsync(string feature, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(feature);

        var license = await source.GetLicenseAsync(cancellationToken).ConfigureAwait(false);
        var validation = validator.Validate(license, DateTimeOffset.UtcNow, options);

        if (!validation.IsValid || validation.License is null)
        {
            var denied = new LicenseFeatureCheckResult(false, feature, LicenseStatusLevel.Error, "License invalid for feature check.", license);
            reporter.Report(new LicenseRuntimeStatus(DateTimeOffset.UtcNow, LicenseCheckStage.RuntimeFeature, denied.Level, "feature.denied.invalid_license", denied.Reason));
            return denied;
        }

        if (!validation.License.Features.Contains(feature))
        {
            var denied = new LicenseFeatureCheckResult(false, feature, LicenseStatusLevel.Warning, "Feature not covered by current license.", validation.License);
            reporter.Report(new LicenseRuntimeStatus(DateTimeOffset.UtcNow, LicenseCheckStage.RuntimeFeature, denied.Level, "feature.denied.not_licensed", denied.Reason));
            return denied;
        }

        var allowed = new LicenseFeatureCheckResult(true, feature, validation.OverallLevel, "Feature is licensed.", validation.License);
        reporter.Report(new LicenseRuntimeStatus(DateTimeOffset.UtcNow, LicenseCheckStage.RuntimeFeature, allowed.Level, "feature.allowed", allowed.Reason));
        return allowed;
    }
}
