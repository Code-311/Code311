using Xunit;
using Code311.Ui.Abstractions.Preferences;
using Code311.Ui.Abstractions.Semantics;
using Code311.Ui.Abstractions.Theming;

namespace Code311.Tests.Ui.Abstractions;

/// <summary>
/// Tests abstraction model defaults and initialization behavior.
/// </summary>
/// <remarks>
/// These tests validate contract-level expectations without depending on implementation packages.
/// </remarks>
public sealed class ThemeAndPreferenceTests
{
    /// <summary>
    /// Verifies theme profile required and default properties.
    /// </summary>
    /// <remarks>
    /// Defaults are part of the contract baseline and help produce predictable behavior.
    /// </remarks>
    [Fact]
    public void ThemeProfile_ShouldExposeExpectedDefaults()
    {
        var profile = new ThemeProfile { Name = "default" };

        Assert.Equal("default", profile.Name);
        Assert.Equal(UiTone.Neutral, profile.Tone);
        Assert.Equal(UiDensity.Comfortable, profile.Density);
        Assert.Equal(SidebarMode.Expanded, profile.SidebarMode);
        Assert.Equal(NavbarStyle.Default, profile.NavbarStyle);
        Assert.False(profile.DarkMode);
    }

    /// <summary>
    /// Verifies user preference required and default properties.
    /// </summary>
    /// <remarks>
    /// This ensures persistence consumers can rely on stable baseline values.
    /// </remarks>
    [Fact]
    public void UserUiPreference_ShouldExposeExpectedDefaults()
    {
        var preference = new UserUiPreference
        {
            TenantId = "tenant-a",
            UserId = "user-1",
            Theme = "default"
        };

        Assert.Equal("tenant-a", preference.TenantId);
        Assert.Equal("user-1", preference.UserId);
        Assert.Equal("default", preference.Theme);
        Assert.Equal(UiDensity.Comfortable, preference.Density);
        Assert.Equal(25, preference.DefaultPageSize);
        Assert.Equal("en", preference.Language);
        Assert.Equal("UTC", preference.TimeZone);
    }
}
