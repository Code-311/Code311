namespace Code311.Ui.Abstractions.Theming;

/// <summary>
/// Resolves active theme profiles.
/// </summary>
/// <remarks>
/// Theme resolution can consider tenant policy and user preference state.
/// </remarks>
public interface IThemeProfileResolver
{
    /// <summary>
    /// Resolves a theme profile by name.
    /// </summary>
    /// <param name="name">The theme name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The resolved theme profile, or <see langword="null"/> when not found.</returns>
    /// <remarks>
    /// Implementations should avoid throwing for unknown themes and return null instead.
    /// </remarks>
    Task<ThemeProfile?> ResolveAsync(string name, CancellationToken cancellationToken = default);
}
