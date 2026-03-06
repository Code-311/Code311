namespace Code311.Host.Services;

/// <summary>
/// Represents host-local startup failure due to licensing validation.
/// </summary>
public sealed class HostStartupLicenseException(string message, Exception innerException) : Exception(message, innerException);
