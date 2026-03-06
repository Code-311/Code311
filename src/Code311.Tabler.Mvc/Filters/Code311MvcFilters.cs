using Code311.Tabler.Mvc.Feedback;
using Code311.Tabler.Mvc.Theming;
using Code311.Ui.Abstractions.Semantics;
using Code311.Ui.Abstractions.Theming;
using Code311.Ui.Core.Feedback;
using Code311.Ui.Core.Loading;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Code311.Tabler.Mvc.Filters;

/// <summary>
/// Captures MVC model-state errors into Code311 feedback channels.
/// </summary>
/// <remarks>
/// The filter keeps request-scoped feedback deterministic by writing to both scoped store and feedback channel.
/// </remarks>
public sealed class Code311FeedbackActionFilter(
    IFeedbackChannel feedbackChannel,
    ICode311RequestFeedbackStore requestFeedbackStore) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
        {
            return;
        }

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

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}

/// <summary>
/// Coordinates request-scope busy/preloader transitions for MVC requests.
/// </summary>
/// <remarks>
/// The filter ensures request-level start/complete orchestration for loader-related services.
/// </remarks>
public sealed class Code311BusyTransitionFilter(
    IBusyStateCoordinator busyStateCoordinator,
    IPreloaderOrchestrator preloaderOrchestrator) : IActionFilter
{
    private const string RequestScope = "mvc-request";
    private IDisposable? _busyScope;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _busyScope = busyStateCoordinator.EnterScope(RequestScope);
        preloaderOrchestrator.Start(RequestScope);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _busyScope?.Dispose();
        preloaderOrchestrator.Complete(RequestScope);
    }
}

/// <summary>
/// Resolves and attaches a semantic theme profile to the current request.
/// </summary>
/// <remarks>
/// Theme resolution uses <see cref="IThemeProfileResolver"/> and stores the result in request-scoped theme context.
/// </remarks>
public sealed class Code311ThemeContextFilter(
    IThemeProfileResolver themeProfileResolver,
    ICode311ThemeRequestContext themeRequestContext) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var requestedTheme = context.HttpContext.Request.Query["theme"].FirstOrDefault() ?? "default";
        themeRequestContext.Current = await themeProfileResolver.ResolveAsync(requestedTheme, context.HttpContext.RequestAborted)
            .ConfigureAwait(false);

        await next().ConfigureAwait(false);
    }
}
