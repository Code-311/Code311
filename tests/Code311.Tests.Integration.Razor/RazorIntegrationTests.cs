using Code311.Tabler.Core.Assets;
using Code311.Tabler.Core.Widgets;
using Code311.Tabler.Razor.Assets;
using Code311.Tabler.Razor.Feedback;
using Code311.Tabler.Razor.Filters;
using Code311.Tabler.Razor.Theming;
using Code311.Ui.Abstractions.Options;
using Code311.Ui.Core.Feedback;
using Code311.Ui.Core.Loading;
using Code311.Ui.Core.Theming;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Xunit;

namespace Code311.Tests.Integration.Razor;

/// <summary>
/// Validates Razor adapter request lifecycle integration behaviors.
/// </summary>
public sealed class RazorIntegrationTests
{
    [Fact]
    public async Task PageFeedbackFilter_ShouldCaptureModelErrors()
    {
        var channel = new InMemoryFeedbackChannel();
        var scoped = new Code311RequestFeedbackStore();
        var filter = new Code311PageFeedbackFilter(channel, scoped);

        var actionContext = new Microsoft.AspNetCore.Mvc.ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor(), new ModelStateDictionary());
        actionContext.ModelState.AddModelError("Email", "Email invalid");

        var pageContext = new PageContext(actionContext);
        var executing = new PageHandlerExecutingContext(pageContext, [], new HandlerMethodDescriptor(), new Dictionary<string, object?>(), new object());

        await filter.OnPageHandlerExecutionAsync(executing, () => Task.FromResult(new PageHandlerExecutedContext(pageContext, [], new HandlerMethodDescriptor(), new object())));

        Assert.Single(scoped.GetAll());
        Assert.Single(channel.Drain());
    }

    [Fact]
    public async Task BusyTransitionPageFilter_ShouldToggleRequestScope()
    {
        var busy = new BusyStateCoordinator();
        var preloader = new PreloaderOrchestrator();
        var filter = new Code311BusyTransitionPageFilter(busy, preloader);

        var actionContext = new Microsoft.AspNetCore.Mvc.ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor(), new ModelStateDictionary());
        var pageContext = new PageContext(actionContext);
        var executing = new PageHandlerExecutingContext(pageContext, [], new HandlerMethodDescriptor(), new Dictionary<string, object?>(), new object());

        await filter.OnPageHandlerExecutionAsync(executing, () =>
        {
            Assert.True(busy.IsBusy("razor-request"));
            Assert.True(preloader.IsActive("razor-request"));
            return Task.FromResult(new PageHandlerExecutedContext(pageContext, [], new HandlerMethodDescriptor(), new object()));
        });

        Assert.False(busy.IsBusy("razor-request"));
        Assert.False(preloader.IsActive("razor-request"));
    }

    [Fact]
    public async Task ThemePageFilter_ShouldSetThemeContext()
    {
        var registry = new ThemeRegistry();
        registry.Register(new Code311.Ui.Abstractions.Theming.ThemeProfile { Name = "green" });
        var resolver = new DefaultThemeProfileResolver(registry, Options.Create(new UiFrameworkOptions()));
        var themeContext = new Code311ThemeRequestContext();
        var filter = new Code311ThemePageFilter(resolver, themeContext);

        var http = new DefaultHttpContext();
        http.Request.QueryString = new QueryString("?theme=green");
        var actionContext = new Microsoft.AspNetCore.Mvc.ActionContext(http, new RouteData(), new ActionDescriptor(), new ModelStateDictionary());
        var pageContext = new PageContext(actionContext);
        var executing = new PageHandlerExecutingContext(pageContext, [], new HandlerMethodDescriptor(), new Dictionary<string, object?>(), new object());

        await filter.OnPageHandlerExecutionAsync(executing, () => Task.FromResult(new PageHandlerExecutedContext(pageContext, [], new HandlerMethodDescriptor(), new object())));

        Assert.Equal("green", themeContext.Current?.Name);
    }

    [Fact]
    public void AssetRequestStore_ShouldAcceptWidgetAssetContributions()
    {
        var store = new Code311AssetRequestStore();
        var widget = new TestWidgetSlotParticipant();

        store.AddWidgetAssets(widget);

        var assets = store.GetAll();
        Assert.Collection(
            assets,
            asset => Assert.False(asset.IsScript),
            asset => Assert.True(asset.IsScript));
    }

    private sealed class TestWidgetSlotParticipant : ITablerWidgetSlotParticipant
    {
        public TablerWidgetSlotDefinition Slot { get; } = new("test", "panel-body", new TablerWidgetOptionsEnvelope(new Dictionary<string, object?>()));

        public IReadOnlyList<TablerAssetDescriptor> GetAssetContributions()
            =>
            [
                new("/scripts/test.js", TablerAssetType.Script, 20),
                new("/styles/test.css", TablerAssetType.Style, 10)
            ];

        public TablerWidgetInitializationRequest CreateInitialization(string elementId)
            => new(Slot.WidgetKey, elementId, "{}");
    }
}
