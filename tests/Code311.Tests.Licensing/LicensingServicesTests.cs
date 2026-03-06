using Code311.Licensing.DependencyInjection;
using Code311.Licensing.Diagnostics;
using Code311.Licensing.Models;
using Code311.Licensing.Validation;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Code311.Tests.Licensing;

public sealed class LicensingServicesTests
{
    [Fact]
    public async Task Validator_ShouldReturnValid_ForActiveLicense()
    {
        var validator = new DefaultLicenseValidator();
        var license = BuildLicense(expiresUtc: DateTimeOffset.UtcNow.AddDays(30));

        var result = validator.Validate(license, DateTimeOffset.UtcNow, new LicensingOptions());

        Assert.True(result.IsValid);
        Assert.Equal(LicenseStatusLevel.Valid, result.OverallLevel);
    }

    [Fact]
    public void Validator_ShouldReturnInvalid_ForExpiredLicense()
    {
        var validator = new DefaultLicenseValidator();
        var license = BuildLicense(expiresUtc: DateTimeOffset.UtcNow.AddMinutes(-1));

        var result = validator.Validate(license, DateTimeOffset.UtcNow, new LicensingOptions());

        Assert.False(result.IsValid);
        Assert.Equal(LicenseStatusLevel.Error, result.OverallLevel);
    }

    [Fact]
    public async Task StartupValidator_ShouldThrow_WhenStartupRequiresValidLicense()
    {
        var services = new ServiceCollection()
            .AddCode311Licensing(options => options.RequireValidLicenseAtStartup = true)
            .AddCode311InMemoryLicenseSource(null)
            .BuildServiceProvider();

        var startup = services.GetRequiredService<IStartupLicenseValidator>();

        await Assert.ThrowsAsync<InvalidOperationException>(() => startup.ValidateAtStartupAsync());
    }

    [Fact]
    public async Task StartupValidator_ShouldReportStatus_WhenValidationRuns()
    {
        var services = new ServiceCollection()
            .AddCode311Licensing(options => options.RequireValidLicenseAtStartup = false)
            .AddCode311InMemoryLicenseSource(BuildLicense(expiresUtc: DateTimeOffset.UtcNow.AddDays(20)))
            .BuildServiceProvider();

        var startup = services.GetRequiredService<IStartupLicenseValidator>();
        var reporter = services.GetRequiredService<ILicensingStatusReporter>();

        var result = await startup.ValidateAtStartupAsync();

        Assert.True(result.IsValid);
        Assert.NotNull(reporter.Current);
        Assert.Equal(LicenseCheckStage.Startup, reporter.Current!.Stage);
    }

    [Fact]
    public async Task FeatureGate_ShouldDeny_WhenFeatureMissing()
    {
        var services = new ServiceCollection()
            .AddCode311Licensing(options => options.RequireValidLicenseAtStartup = false)
            .AddCode311InMemoryLicenseSource(BuildLicense(features: ["dashboard.basic"]))
            .BuildServiceProvider();

        var gate = services.GetRequiredService<ILicenseFeatureGate>();

        var result = await gate.CheckFeatureAsync("dashboard.advanced");

        Assert.False(result.IsAllowed);
        Assert.Equal(LicenseStatusLevel.Warning, result.Level);
    }

    [Fact]
    public async Task FeatureGate_ShouldAllow_WhenFeaturePresent()
    {
        var services = new ServiceCollection()
            .AddCode311Licensing(options => options.RequireValidLicenseAtStartup = false)
            .AddCode311InMemoryLicenseSource(BuildLicense(features: ["dashboard.advanced"]))
            .BuildServiceProvider();

        var gate = services.GetRequiredService<ILicenseFeatureGate>();

        var result = await gate.CheckFeatureAsync("dashboard.advanced");

        Assert.True(result.IsAllowed);
        Assert.Equal("dashboard.advanced", result.Feature);
    }

    [Fact]
    public void AddCode311Licensing_ShouldRegisterCoreServices()
    {
        var services = new ServiceCollection();
        services.AddCode311Licensing();

        using var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetService<IStartupLicenseValidator>());
        Assert.NotNull(provider.GetService<ILicenseFeatureGate>());
        Assert.NotNull(provider.GetService<ILicenseValidator>());
        Assert.NotNull(provider.GetService<ILicensingStatusReporter>());
    }

    [Fact]
    public void UiAndTablerPackages_ShouldNotReferenceCode311Licensing()
    {
        var forbiddenProjectFiles = new[]
        {
            "src/Code311.Ui.Abstractions/Code311.Ui.Abstractions.csproj",
            "src/Code311.Ui.Core/Code311.Ui.Core.csproj",
            "src/Code311.Tabler.Core/Code311.Tabler.Core.csproj",
            "src/Code311.Tabler.Components/Code311.Tabler.Components.csproj",
            "src/Code311.Tabler.Dashboard/Code311.Tabler.Dashboard.csproj",
            "src/Code311.Tabler.Widgets.DataTables/Code311.Tabler.Widgets.DataTables.csproj",
            "src/Code311.Tabler.Widgets.Calendar/Code311.Tabler.Widgets.Calendar.csproj",
            "src/Code311.Tabler.Widgets.Charts/Code311.Tabler.Widgets.Charts.csproj"
        };

        foreach (var relativePath in forbiddenProjectFiles)
        {
            var full = Path.Combine(AppContext.BaseDirectory, "../../../../", relativePath);
            var xml = File.ReadAllText(full);
            Assert.DoesNotContain("Code311.Licensing", xml, StringComparison.Ordinal);
        }
    }

    private static Code311License BuildLicense(DateTimeOffset? expiresUtc = null, DateTimeOffset? notBeforeUtc = null, IReadOnlySet<string>? features = null)
        => new()
        {
            LicenseId = "lic-001",
            CustomerName = "Code311 Test",
            Plan = "pro",
            NotBeforeUtc = notBeforeUtc,
            ExpiresUtc = expiresUtc ?? DateTimeOffset.UtcNow.AddDays(30),
            Features = features ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "dashboard.basic" }
        };
}
