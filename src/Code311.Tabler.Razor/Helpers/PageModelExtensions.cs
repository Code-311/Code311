using Code311.Tabler.Razor.Assets;
using Code311.Tabler.Razor.Feedback;
using Code311.Tabler.Razor.Theming;
using Code311.Ui.Abstractions.Semantics;
using Code311.Ui.Core.Feedback;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace Code311.Tabler.Razor.Helpers;

/// <summary>
/// Provides lightweight Razor Pages helper extensions for Code311 integration.
/// </summary>
public static class PageModelExtensions
{
    /// <summary>
    /// Publishes semantic feedback for the current request.
    /// </summary>
    public static void PublishCode311Feedback(this PageModel page, UiTone tone, string message)
    {
        var services = page.HttpContext.RequestServices;
        var channel = services.GetRequiredService<IFeedbackChannel>();
        var scoped = services.GetRequiredService<ICode311RequestFeedbackStore>();
        var model = new FeedbackMessage(tone, message, null, DateTimeOffset.UtcNow);
        channel.Publish(model);
        scoped.Add(model);
    }

    /// <summary>
    /// Adds a script asset to request scope.
    /// </summary>
    public static void AddCode311Script(this PageModel page, string path)
        => page.HttpContext.RequestServices.GetRequiredService<ICode311AssetRequestStore>().AddScript(path);

    /// <summary>
    /// Adds a style asset to request scope.
    /// </summary>
    public static void AddCode311Style(this PageModel page, string path)
        => page.HttpContext.RequestServices.GetRequiredService<ICode311AssetRequestStore>().AddStyle(path);

    /// <summary>
    /// Gets the active request theme name.
    /// </summary>
    public static string GetCode311ThemeName(this PageModel page)
        => page.HttpContext.RequestServices.GetRequiredService<ICode311ThemeRequestContext>().Current?.Name ?? "default";
}
