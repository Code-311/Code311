using Code311.Tabler.Razor.Feedback;
using Code311.Tabler.Razor.Theming;
using Code311.Ui.Abstractions.Semantics;
using Code311.Ui.Abstractions.Theming;
using Code311.Ui.Core.Feedback;
using Code311.Ui.Core.Loading;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Code311.Tabler.Razor.Filters;

/// <summary>
/// Captures Razor Page model validation errors into Code311 feedback channels.
/// </summary>
public sealed class Code311PageFeedbackFilter(
    IFeedbackChannel feedbackChannel,
    ICode311RequestFeedbackStore requestFeedbackStore) : IAsyncPageFilter
{
    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) => Task.CompletedTask;

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            foreach (var kvp in context.ModelState)
            {
                foreach (var err in kvp.Value?.Errors ?? [])
                {
                    var message = new FeedbackMessage(UiTone.Danger, err.ErrorMessage, "MODEL_STATE", DateTimeOffset.UtcNow);
                    feedbackChannel.Publish(message);
                    requestFeedbackStore.Add(message);
                }
            }
        }

        await next().ConfigureAwait(false);
    }
}

/// <summary>
/// Coordinates request-scope busy/preloader transitions for Razor handlers.
/// </summary>
public sealed class Code311BusyTransitionPageFilter(
    IBusyStateCoordinator busyStateCoordinator,
    IPreloaderOrchestrator preloaderOrchestrator) : IAsyncPageFilter
{
    private const string RequestScope = "razor-request";

    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) => Task.CompletedTask;

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        using var scope = busyStateCoordinator.EnterScope(RequestScope);
        preloaderOrchestrator.Start(RequestScope);
        await next().ConfigureAwait(false);
        preloaderOrchestrator.Complete(RequestScope);
    }
}

/// <summary>
/// Resolves request theme for Razor Pages and stores it in request context.
/// </summary>
public sealed class Code311ThemePageFilter(
    IThemeProfileResolver themeProfileResolver,
    ICode311ThemeRequestContext themeRequestContext) : IAsyncPageFilter
{
    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) => Task.CompletedTask;

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        var requestedTheme = context.HttpContext.Request.Query["theme"].FirstOrDefault() ?? "default";
        themeRequestContext.Current = await themeProfileResolver.ResolveAsync(requestedTheme, context.HttpContext.RequestAborted)
            .ConfigureAwait(false);
        await next().ConfigureAwait(false);
    }
}
