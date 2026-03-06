namespace Code311.Tabler.Components.Common;

/// <summary>
/// Represents a semantic text/value option item.
/// </summary>
/// <remarks>
/// This item is used by form and navigation components without exposing framework-specific option models.
/// </remarks>
public sealed record Cd311OptionItem(string Value, string Text, bool Selected = false, bool Disabled = false);

/// <summary>
/// Represents a semantic navigation item.
/// </summary>
/// <remarks>
/// Navigation components consume this model to produce menu and tab structures.
/// </remarks>
public sealed record Cd311NavItem(string Text, string? Url = null, bool Active = false, bool Disabled = false);

/// <summary>
/// Represents a semantic action item.
/// </summary>
/// <remarks>
/// Action items provide consistent semantics across action bars, dropdowns, and modal actions.
/// </remarks>
public sealed record Cd311ActionItem(string Text, string? Command = null, bool Primary = false, bool Disabled = false);

/// <summary>
/// Represents a key/value semantic pair.
/// </summary>
/// <remarks>
/// Data components use this model for metadata and summary displays.
/// </remarks>
public sealed record Cd311KeyValueItem(string Key, string Value);
