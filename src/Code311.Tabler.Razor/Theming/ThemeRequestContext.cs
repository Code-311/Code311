using Code311.Ui.Abstractions.Theming;

namespace Code311.Tabler.Razor.Theming;

/// <summary>
/// Provides access to request-scoped theme context for Razor Pages.
/// </summary>
public interface ICode311ThemeRequestContext
{
    ThemeProfile? Current { get; set; }
}

internal sealed class Code311ThemeRequestContext : ICode311ThemeRequestContext
{
    public ThemeProfile? Current { get; set; }
}
