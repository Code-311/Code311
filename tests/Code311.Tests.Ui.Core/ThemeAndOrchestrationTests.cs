using Code311.Ui.Abstractions.Options;
using Code311.Ui.Abstractions.Semantics;
using Code311.Ui.Abstractions.Theming;
using Code311.Ui.Core.Feedback;
using Code311.Ui.Core.Loading;
using Code311.Ui.Core.Theming;
using Microsoft.Extensions.Options;
using Xunit;

namespace Code311.Tests.Ui.Core;

/// <summary>
/// Validates neutral orchestration behavior in <c>Code311.Ui.Core</c>.
/// </summary>
/// <remarks>
/// Tests ensure core behavior remains semantic and design-system-agnostic.
/// </remarks>
public sealed class ThemeAndOrchestrationTests
{
    /// <summary>
    /// Ensures resolver returns explicit theme when present.
    /// </summary>
    /// <remarks>
    /// Explicit registration should win over fallback behavior.
    /// </remarks>
    [Fact]
    public async Task ThemeResolver_ShouldReturnRegisteredThemeAsync()
    {
        var registry = new ThemeRegistry();
        registry.Register(new ThemeProfile { Name = "enterprise", Tone = UiTone.Accent });

        var resolver = new DefaultThemeProfileResolver(
            registry,
            Options.Create(new UiFrameworkOptions { DefaultDensity = UiDensity.Spacious }));

        var result = await resolver.ResolveAsync("enterprise");

        Assert.NotNull(result);
        Assert.Equal("enterprise", result!.Name);
        Assert.Equal(UiTone.Accent, result.Tone);
    }

    /// <summary>
    /// Ensures fallback theme uses framework options when explicit theme is missing.
    /// </summary>
    /// <remarks>
    /// Fallback behavior must remain deterministic.
    /// </remarks>
    [Fact]
    public async Task ThemeResolver_ShouldReturnFallbackThemeAsync()
    {
        var resolver = new DefaultThemeProfileResolver(
            new ThemeRegistry(),
            Options.Create(new UiFrameworkOptions { DefaultDensity = UiDensity.Compact }));

        var result = await resolver.ResolveAsync("unknown");

        Assert.NotNull(result);
        Assert.Equal("default", result!.Name);
        Assert.Equal(UiDensity.Compact, result.Density);
    }

    /// <summary>
    /// Ensures feedback channel drains messages in FIFO order.
    /// </summary>
    /// <remarks>
    /// Order preservation is important for predictable UX sequencing.
    /// </remarks>
    [Fact]
    public void FeedbackChannel_ShouldDrainInOrder()
    {
        var channel = new InMemoryFeedbackChannel();
        channel.Publish(new FeedbackMessage(UiTone.Info, "first", null, DateTimeOffset.UtcNow));
        channel.Publish(new FeedbackMessage(UiTone.Warning, "second", null, DateTimeOffset.UtcNow));

        var drained = channel.Drain();

        Assert.Collection(
            drained,
            x => Assert.Equal("first", x.Message),
            x => Assert.Equal("second", x.Message));
        Assert.Empty(channel.Drain());
    }

    /// <summary>
    /// Ensures busy scope increments and decrements as expected.
    /// </summary>
    /// <remarks>
    /// Scope disposal should clear busy state for the tracked key.
    /// </remarks>
    [Fact]
    public void BusyStateCoordinator_ShouldTrackScopes()
    {
        var coordinator = new BusyStateCoordinator();

        using (coordinator.EnterScope("save"))
        {
            Assert.True(coordinator.IsBusy("save"));
        }

        Assert.False(coordinator.IsBusy("save"));
    }

    /// <summary>
    /// Ensures preloader activation and completion update state.
    /// </summary>
    /// <remarks>
    /// Start/complete orchestration must be idempotent and predictable.
    /// </remarks>
    [Fact]
    public void PreloaderOrchestrator_ShouldTrackActivation()
    {
        var orchestrator = new PreloaderOrchestrator();

        orchestrator.Start("route-load");
        Assert.True(orchestrator.IsActive("route-load"));

        orchestrator.Complete("route-load");
        Assert.False(orchestrator.IsActive("route-load"));
    }
}
