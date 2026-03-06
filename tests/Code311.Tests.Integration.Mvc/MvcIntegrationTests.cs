using Code311.Tabler.Mvc.Assets;
using Code311.Tabler.Core.Assets;
using Code311.Tabler.Core.Widgets;
using Code311.Tabler.Mvc.Feedback;
using Code311.Tabler.Mvc.Filters;
using Code311.Tabler.Mvc.Theming;
using Code311.Ui.Abstractions.Options;
using Code311.Ui.Core.Feedback;
using Code311.Ui.Core.Loading;
using Code311.Ui.Core.Theming;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Xunit;

namespace Code311.Tests.Integration.Mvc;

/// <summary>
/// Validates MVC adapter request lifecycle integration behaviors.
/// </summary>
public sealed class MvcIntegrationTests
{
    [Fact]
    public void FeedbackActionFilter_ShouldPublishModelStateErrors()
    {
        var channel = new InMemoryFeedbackChannel();
        var scoped = new Code311RequestFeedbackStore();
        var filter = new Code311FeedbackActionFilter(channel, scoped);

        var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor(), new ModelStateDictionary());
        actionContext.ModelState.AddModelError("Name", "Name is required.");

        var ctx = new ActionExecutingContext(actionContext, [], new Dictionary<string, object?>(), new object());
        filter.OnActionExecuting(ctx);

        Assert.Single(scoped.GetAll());
        Assert.Single(channel.Drain());
    }

    [Fact]
    public void BusyTransitionFilter_ShouldToggleBusyAndPreloader()
    {
        var busy = new BusyStateCoordinator();
        var preloader = new PreloaderOrchestrator();
        var filter = new Code311BusyTransitionFilter(busy, preloader);

        var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor(), new ModelStateDictionary());
        var executing = new ActionExecutingContext(actionContext, [], new Dictionary<string, object?>(), new object());
        var executed = new ActionExecutedContext(actionContext, [], new object());

        filter.OnActionExecuting(executing);
        Assert.True(busy.IsBusy("mvc-request"));
        Assert.True(preloader.IsActive("mvc-request"));

        filter.OnActionExecuted(executed);
        Assert.False(busy.IsBusy("mvc-request"));
        Assert.False(preloader.IsActive("mvc-request"));
    }

    [Fact]
    public async Task ThemeFilter_ShouldSetRequestThemeContext()
    {
        var registry = new ThemeRegistry();
        registry.Register(new Code311.Ui.Abstractions.Theming.ThemeProfile { Name = "blue" });
        var resolver = new DefaultThemeProfileResolver(registry, Options.Create(new UiFrameworkOptions()));
        var themeContext = new Code311ThemeRequestContext();
        var filter = new Code311ThemeContextFilter(resolver, themeContext);

        var http = new DefaultHttpContext();
        http.Request.QueryString = new QueryString("?theme=blue");
        var actionContext = new ActionContext(http, new RouteData(), new ActionDescriptor(), new ModelStateDictionary());
        var executing = new ActionExecutingContext(actionContext, [], new Dictionary<string, object?>(), new object());

        await filter.OnActionExecutionAsync(executing, () => Task.FromResult(new ActionExecutedContext(actionContext, [], new object())));

        Assert.Equal("blue", themeContext.Current?.Name);
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
