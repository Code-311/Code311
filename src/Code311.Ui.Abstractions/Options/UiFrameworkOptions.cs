using Code311.Ui.Abstractions.Semantics;

namespace Code311.Ui.Abstractions.Options;

/// <summary>
/// Represents global framework behavior options.
/// </summary>
/// <remarks>
/// Hosts can configure these options to establish predictable defaults.
/// </remarks>
public sealed record UiFrameworkOptions
{
    /// <summary>
    /// Gets the default density.
    /// </summary>
    /// <remarks>
    /// Components should use this value when no explicit density is supplied.
    /// </remarks>
    public UiDensity DefaultDensity { get; init; } = UiDensity.Comfortable;

    /// <summary>
    /// Gets the default appearance.
    /// </summary>
    /// <remarks>
    /// Appearance defaults are semantic and design-system-neutral.
    /// </remarks>
    public UiAppearance DefaultAppearance { get; init; } = UiAppearance.Soft;

    /// <summary>
    /// Gets the default page size.
    /// </summary>
    /// <remarks>
    /// Data-oriented components can use this value when no explicit size is provided.
    /// </remarks>
    public int DefaultPageSize { get; init; } = 25;
}
