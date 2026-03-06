using Code311.Ui.Abstractions.Semantics;
using Code311.Ui.Abstractions.Theming;
using Code311.Tabler.Core.Mapping;

namespace Code311.Tabler.Core.Theming;

/// <summary>
/// Represents a resolved Tabler theme mapping result.
/// </summary>
/// <param name="ThemeName">The semantic theme profile name.</param>
/// <param name="BodyClass">The mapped body class string.</param>
/// <param name="NavbarClass">The mapped navbar class string.</param>
/// <param name="SidebarClass">The mapped sidebar class string.</param>
/// <remarks>
/// This model is used by adapters and components to compose deterministic root layout classes.
/// </remarks>
public sealed record TablerThemeMap(string ThemeName, string BodyClass, string NavbarClass, string SidebarClass);

/// <summary>
/// Maps semantic theme profiles to Tabler layout classes.
/// </summary>
/// <remarks>
/// Theme mapping centralizes Tabler-specific class generation and keeps upstream services semantic.
/// </remarks>
public interface ITablerThemeMapper
{
    /// <summary>
    /// Maps a semantic profile to a Tabler theme map.
    /// </summary>
    /// <param name="profile">The semantic theme profile.</param>
    /// <returns>The mapped Tabler theme data.</returns>
    /// <remarks>
    /// Implementations should be deterministic for predictable rendering and testing.
    /// </remarks>
    TablerThemeMap Map(ThemeProfile profile);
}

/// <summary>
/// Default semantic-to-Tabler theme mapper.
/// </summary>
/// <remarks>
/// The mapper composes layout and density classes using semantic profile inputs.
/// </remarks>
public sealed class TablerThemeMapper : ITablerThemeMapper
{
    private readonly ITablerSemanticClassMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="TablerThemeMapper"/> class.
    /// </summary>
    /// <param name="mapper">The semantic class mapper dependency.</param>
    /// <remarks>
    /// Mapper dependency is required to keep class token generation consistent.
    /// </remarks>
    public TablerThemeMapper(ITablerSemanticClassMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc />
    public TablerThemeMap Map(ThemeProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);

        var bodyClasses = new List<string>
        {
            _mapper.MapDensity(profile.Density)
        };

        if (profile.DarkMode)
        {
            bodyClasses.Add("theme-dark");
        }

        var navbarClasses = new List<string>
        {
            "navbar",
            $"navbar-{MapNavbar(profile.NavbarStyle)}",
            $"bg-{_mapper.MapTone(profile.Tone)}"
        };

        var sidebarClasses = new List<string>
        {
            "navbar-vertical",
            $"sidebar-{MapSidebar(profile.SidebarMode)}"
        };

        return new TablerThemeMap(
            profile.Name,
            string.Join(' ', bodyClasses.Where(static c => !string.IsNullOrWhiteSpace(c))),
            string.Join(' ', navbarClasses),
            string.Join(' ', sidebarClasses));
    }

    private static string MapSidebar(SidebarMode mode) => mode switch
    {
        SidebarMode.Expanded => "expanded",
        SidebarMode.Collapsed => "collapsed",
        SidebarMode.Overlay => "overlay",
        _ => "expanded"
    };

    private static string MapNavbar(NavbarStyle style) => style switch
    {
        NavbarStyle.Default => "default",
        NavbarStyle.Contrast => "dark",
        NavbarStyle.Minimal => "transparent",
        NavbarStyle.Elevated => "elevated",
        _ => "default"
    };
}
