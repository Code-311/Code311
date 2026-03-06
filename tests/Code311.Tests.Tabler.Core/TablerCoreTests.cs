using Code311.Ui.Abstractions.Semantics;
using Code311.Ui.Abstractions.Theming;
using Code311.Tabler.Core.Assets;
using Code311.Tabler.Core.Mapping;
using Code311.Tabler.Core.Theming;
using Xunit;

namespace Code311.Tests.Tabler.Core;

/// <summary>
/// Validates Tabler core mapping and asset foundation behavior.
/// </summary>
/// <remarks>
/// Tests verify deterministic translation from semantic contracts to Tabler-specific values.
/// </remarks>
public sealed class TablerCoreTests
{
    /// <summary>
    /// Ensures tone mapping translates known semantic values.
    /// </summary>
    /// <remarks>
    /// Mapping outputs are internal details but must remain stable across releases.
    /// </remarks>
    [Fact]
    public void SemanticMapper_ShouldMapTone()
    {
        var mapper = new TablerSemanticClassMapper();

        Assert.Equal("success", mapper.MapTone(UiTone.Success));
        Assert.Equal("danger", mapper.MapTone(UiTone.Danger));
    }

    /// <summary>
    /// Ensures theme mapper builds expected layout classes.
    /// </summary>
    /// <remarks>
    /// Theme mapping should consume semantic profile data without leaking upstream concerns.
    /// </remarks>
    [Fact]
    public void ThemeMapper_ShouldBuildTablerThemeMap()
    {
        var mapper = new TablerThemeMapper(new TablerSemanticClassMapper());

        var map = mapper.Map(new ThemeProfile
        {
            Name = "default",
            Tone = UiTone.Accent,
            Density = UiDensity.Comfortable,
            SidebarMode = SidebarMode.Collapsed,
            NavbarStyle = NavbarStyle.Contrast,
            DarkMode = true
        });

        Assert.Equal("default", map.ThemeName);
        Assert.Contains("theme-dark", map.BodyClass);
        Assert.Contains("bg-primary", map.NavbarClass);
        Assert.Contains("sidebar-collapsed", map.SidebarClass);
    }

    /// <summary>
    /// Ensures asset manifest includes deterministic core entries.
    /// </summary>
    /// <remarks>
    /// Core assets must preserve deterministic ordering for predictable host integration.
    /// </remarks>
    [Fact]
    public void AssetManifestProvider_ShouldReturnCoreAssets()
    {
        ITablerAssetManifestProvider provider = new DefaultTablerAssetManifestProvider();

        var assets = provider.GetAssets();

        Assert.Equal(2, assets.Count);
        Assert.Equal(TablerAssetType.Style, assets[0].Type);
        Assert.Equal(TablerAssetType.Script, assets[1].Type);
        Assert.True(assets[0].Order < assets[1].Order);
    }
}
