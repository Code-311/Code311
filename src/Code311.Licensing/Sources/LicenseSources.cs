using System.Text.Json;
using Code311.Licensing.Models;

namespace Code311.Licensing.Sources;

/// <summary>
/// Resolves a license payload from a configured source.
/// </summary>
public interface ILicenseSource
{
    Task<Code311License?> GetLicenseAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// In-memory source suitable for tests and deterministic bootstrapping.
/// </summary>
public sealed class InMemoryLicenseSource(Code311License? license) : ILicenseSource
{
    public Task<Code311License?> GetLicenseAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(license);
}

/// <summary>
/// Reads a license payload from an environment variable containing JSON.
/// </summary>
public sealed class EnvironmentVariableLicenseSource(string variableName) : ILicenseSource
{
    public Task<Code311License?> GetLicenseAsync(CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(variableName);

        var payload = Environment.GetEnvironmentVariable(variableName);
        if (string.IsNullOrWhiteSpace(payload))
        {
            return Task.FromResult<Code311License?>(null);
        }

        var model = JsonSerializer.Deserialize<Code311License>(payload);
        return Task.FromResult(model);
    }
}
