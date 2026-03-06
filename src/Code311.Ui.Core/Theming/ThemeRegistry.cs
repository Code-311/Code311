using Code311.Ui.Abstractions.Options;
using Code311.Ui.Abstractions.Semantics;
using Code311.Ui.Abstractions.Theming;
using Microsoft.Extensions.Options;

namespace Code311.Ui.Core.Theming;

/// <summary>
/// Provides runtime registration and retrieval for semantic theme profiles.
/// </summary>
/// <remarks>
/// Theme registry operations are in-memory and deterministic, and can be composed with persistence in later phases.
/// </remarks>
public interface IThemeRegistry
{
    /// <summary>
    /// Registers or replaces a theme profile.
    /// </summary>
    /// <param name="profile">The theme profile.</param>
    /// <remarks>
    /// Profiles are keyed by name using case-insensitive lookup semantics.
    /// </remarks>
    void Register(ThemeProfile profile);

    /// <summary>
    /// Gets a theme profile by name.
    /// </summary>
    /// <param name="name">The profile name.</param>
    /// <returns>The profile when found; otherwise <see langword="null"/>.</returns>
    /// <remarks>
    /// Callers can use this method for fast non-async resolution where appropriate.
    /// </remarks>
    ThemeProfile? Get(string name);

    /// <summary>
    /// Returns all currently registered profiles.
    /// </summary>
    /// <returns>A read-only collection of profiles.</returns>
    /// <remarks>
    /// The returned collection is a snapshot and is safe for enumeration.
    /// </remarks>
    IReadOnlyCollection<ThemeProfile> GetAll();
}

/// <summary>
/// Default in-memory implementation of <see cref="IThemeRegistry"/>.
/// </summary>
/// <remarks>
/// This implementation avoids design-system details and only stores semantic profile values.
/// </remarks>
public sealed class ThemeRegistry : IThemeRegistry
{
    private readonly Dictionary<string, ThemeProfile> _profiles = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeRegistry"/> class.
    /// </summary>
    /// <remarks>
    /// The registry starts empty and can be seeded by startup modules.
    /// </remarks>
    public ThemeRegistry()
    {
    }

    /// <inheritdoc />
    public void Register(ThemeProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        _profiles[profile.Name] = profile;
    }

    /// <inheritdoc />
    public ThemeProfile? Get(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return _profiles.TryGetValue(name, out var profile) ? profile : null;
    }

    /// <inheritdoc />
    public IReadOnlyCollection<ThemeProfile> GetAll() => _profiles.Values.ToArray();
}

/// <summary>
/// Default theme resolver implementation based on registered themes and framework options.
/// </summary>
/// <remarks>
/// Resolver behavior is framework-neutral and does not expose any Tabler-specific semantics.
/// </remarks>
public sealed class DefaultThemeProfileResolver : IThemeProfileResolver
{
    private const string DefaultThemeName = "default";
    private readonly IThemeRegistry _themeRegistry;
    private readonly UiFrameworkOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultThemeProfileResolver"/> class.
    /// </summary>
    /// <param name="themeRegistry">The theme registry dependency.</param>
    /// <param name="options">The options accessor.</param>
    /// <remarks>
    /// The resolver uses options values to compose a deterministic fallback profile.
    /// </remarks>
    public DefaultThemeProfileResolver(IThemeRegistry themeRegistry, IOptions<UiFrameworkOptions> options)
    {
        _themeRegistry = themeRegistry ?? throw new ArgumentNullException(nameof(themeRegistry));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public Task<ThemeProfile?> ResolveAsync(string name, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        cancellationToken.ThrowIfCancellationRequested();

        var explicitTheme = _themeRegistry.Get(name);
        if (explicitTheme is not null)
        {
            return Task.FromResult<ThemeProfile?>(explicitTheme);
        }

        var fallbackTheme = _themeRegistry.Get(DefaultThemeName) ?? CreateFallback();
        return Task.FromResult<ThemeProfile?>(fallbackTheme);
    }

    private ThemeProfile CreateFallback() => new()
    {
        Name = DefaultThemeName,
        Density = _options.DefaultDensity,
        Tone = UiTone.Neutral,
        SidebarMode = SidebarMode.Expanded,
        NavbarStyle = NavbarStyle.Default,
        DarkMode = false
    };
}
