using Code311.Ui.Abstractions.Semantics;

namespace Code311.Ui.Abstractions.Theming;

/// <summary>
/// Represents a named and reusable semantic theme profile.
/// </summary>
/// <remarks>
/// Theme profiles are consumed by runtime services and can be persisted as user preferences.
/// </remarks>
public sealed record ThemeProfile
{
    /// <summary>
    /// Gets the unique theme profile name.
    /// </summary>
    /// <remarks>
    /// Names are expected to be stable identifiers for persistence and transport.
    /// </remarks>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the semantic primary tone for the profile.
    /// </summary>
    /// <remarks>
    /// The actual color mapping is design-system-specific and occurs outside abstractions.
    /// </remarks>
    public UiTone Tone { get; init; } = UiTone.Neutral;

    /// <summary>
    /// Gets the semantic density preset.
    /// </summary>
    /// <remarks>
    /// Density influences spacing rules in components and layout helpers.
    /// </remarks>
    public UiDensity Density { get; init; } = UiDensity.Comfortable;

    /// <summary>
    /// Gets the semantic sidebar mode.
    /// </summary>
    /// <remarks>
    /// Sidebars can be expanded/collapsed/overlay depending on adapter interpretation.
    /// </remarks>
    public SidebarMode SidebarMode { get; init; } = SidebarMode.Expanded;

    /// <summary>
    /// Gets the semantic navbar style.
    /// </summary>
    /// <remarks>
    /// Navbar style remains framework-neutral and should not reference concrete class names.
    /// </remarks>
    public NavbarStyle NavbarStyle { get; init; } = NavbarStyle.Default;

    /// <summary>
    /// Gets a value indicating whether dark mode is enabled.
    /// </summary>
    /// <remarks>
    /// Runtime adapters can honor this flag according to local rendering capabilities.
    /// </remarks>
    public bool DarkMode { get; init; }
}

/// <summary>
/// Represents semantic sidebar behavior options.
/// </summary>
/// <remarks>
/// The enum is used by profiles and user preferences for consistent persistence.
/// </remarks>
public enum SidebarMode
{
    Expanded,
    Collapsed,
    Overlay
}

/// <summary>
/// Represents semantic navbar styles.
/// </summary>
/// <remarks>
/// Values should map through design-system adapters and not be interpreted as CSS classes directly.
/// </remarks>
public enum NavbarStyle
{
    Default,
    Contrast,
    Minimal,
    Elevated
}
