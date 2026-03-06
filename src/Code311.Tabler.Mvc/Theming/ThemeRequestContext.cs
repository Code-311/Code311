using Code311.Ui.Abstractions.Theming;

namespace Code311.Tabler.Mvc.Theming;

/// <summary>
/// Provides access to the current request theme profile.
/// </summary>
/// <remarks>
/// Theme context is request-scoped and set by MVC filters.
/// </remarks>
public interface ICode311ThemeRequestContext
{
    /// <summary>
    /// Gets or sets the current request theme profile.
    /// </summary>
    ThemeProfile? Current { get; set; }
}

internal sealed class Code311ThemeRequestContext : ICode311ThemeRequestContext
{
    public ThemeProfile? Current { get; set; }
}
