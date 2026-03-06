namespace Code311.Ui.Abstractions.Semantics;

/// <summary>
/// Represents semantic visual tone independent of any CSS framework.
/// </summary>
/// <remarks>
/// Tone values communicate intent (success, warning, etc.) and are mapped internally by design-system packages.
/// </remarks>
public enum UiTone
{
    Neutral,
    Accent,
    Success,
    Warning,
    Danger,
    Info
}

/// <summary>
/// Represents semantic component appearance variants.
/// </summary>
/// <remarks>
/// Appearance values are stable public API semantics and must never expose design-system tokens.
/// </remarks>
public enum UiAppearance
{
    Solid,
    Soft,
    Outline,
    Ghost,
    Link
}

/// <summary>
/// Represents semantic density options for layout and controls.
/// </summary>
/// <remarks>
/// Density drives spacing and compactness behavior in a design-system-neutral manner.
/// </remarks>
public enum UiDensity
{
    Compact,
    Comfortable,
    Spacious
}

/// <summary>
/// Represents semantic component sizing options.
/// </summary>
/// <remarks>
/// Size values should be interpreted by design-system adapters using local mapping policies.
/// </remarks>
public enum UiSize
{
    Small,
    Medium,
    Large
}

/// <summary>
/// Represents semantic UI state used by controls and patterns.
/// </summary>
/// <remarks>
/// State values are intentionally generic so multiple components can share the same contract.
/// </remarks>
public enum UiState
{
    Default,
    Active,
    Disabled,
    ReadOnly,
    Busy,
    Hidden
}

/// <summary>
/// Represents semantic placement intent for overlays and anchored elements.
/// </summary>
/// <remarks>
/// Placement abstractions allow adapters to map into framework-specific placement primitives.
/// </remarks>
public enum UiPlacement
{
    Top,
    TopStart,
    TopEnd,
    Right,
    Bottom,
    BottomStart,
    BottomEnd,
    Left,
    Center
}

/// <summary>
/// Represents semantic layout intent for containers and grouping primitives.
/// </summary>
/// <remarks>
/// Layout semantics intentionally avoid concrete CSS display or grid syntax.
/// </remarks>
public enum UiLayout
{
    Inline,
    Stack,
    Grid,
    Split,
    Fill
}

/// <summary>
/// Represents semantic pinning behavior.
/// </summary>
/// <remarks>
/// Pinning indicates sticky/fixed intent without exposing implementation details.
/// </remarks>
public enum UiPinned
{
    None,
    Start,
    End,
    Both
}
