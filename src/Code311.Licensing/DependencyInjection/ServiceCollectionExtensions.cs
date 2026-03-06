using Code311.Licensing.Diagnostics;
using Code311.Licensing.Models;
using Code311.Licensing.Sources;
using Code311.Licensing.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Code311.Licensing.DependencyInjection;

/// <summary>
/// Provides registration helpers for Code311 licensing services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Code311 licensing services and an environment-variable-based source.
    /// </summary>
    public static IServiceCollection AddCode311Licensing(
        this IServiceCollection services,
        Action<LicensingOptions>? configure = null,
        string environmentVariableName = "CODE311_LICENSE_JSON")
    {
        ArgumentNullException.ThrowIfNull(services);

        var options = new LicensingOptions();
        configure?.Invoke(options);

        services.TryAddSingleton(options);
        services.TryAddSingleton<ILicensingStatusReporter, InMemoryLicensingStatusReporter>();
        services.TryAddSingleton<ILicenseValidator, DefaultLicenseValidator>();
        services.TryAddSingleton<ILicenseSource>(_ => new EnvironmentVariableLicenseSource(environmentVariableName));
        services.TryAddSingleton<IStartupLicenseValidator, StartupLicenseValidator>();
        services.TryAddSingleton<ILicenseFeatureGate, LicenseFeatureGate>();

        return services;
    }

    /// <summary>
    /// Replaces the registered source with an in-memory payload.
    /// </summary>
    public static IServiceCollection AddCode311InMemoryLicenseSource(this IServiceCollection services, Code311License? license)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.Replace(ServiceDescriptor.Singleton<ILicenseSource>(_ => new InMemoryLicenseSource(license)));
        return services;
    }
}
