using Code311.Tabler.Mvc.Assets;
using Code311.Tabler.Mvc.Feedback;
using Code311.Tabler.Mvc.Theming;
using Code311.Ui.Abstractions.Semantics;
using Code311.Ui.Core.Feedback;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace Code311.Tabler.Mvc.Helpers;

/// <summary>
/// Provides lightweight MVC HTML helper extensions for Code311 host integration.
/// </summary>
/// <remarks>
/// Extensions are optional ergonomics helpers and do not alter framework-neutral core contracts.
/// </remarks>
public static class HtmlHelperExtensions
{
    /// <summary>
    /// Registers a script asset for the current request.
    /// </summary>
    /// <param name="html">The HTML helper.</param>
    /// <param name="path">The script path.</param>
    public static void AddCode311Script(this IHtmlHelper html, string path)
    {
        var store = html.ViewContext.HttpContext.RequestServices.GetRequiredService<ICode311AssetRequestStore>();
        store.AddScript(path);
    }

    /// <summary>
    /// Registers a style asset for the current request.
    /// </summary>
    /// <param name="html">The HTML helper.</param>
    /// <param name="path">The style path.</param>
    public static void AddCode311Style(this IHtmlHelper html, string path)
    {
        var store = html.ViewContext.HttpContext.RequestServices.GetRequiredService<ICode311AssetRequestStore>();
        store.AddStyle(path);
    }

    /// <summary>
    /// Publishes request-scoped feedback.
    /// </summary>
    /// <param name="html">The HTML helper.</param>
    /// <param name="tone">The semantic tone.</param>
    /// <param name="message">The message text.</param>
    public static void PublishCode311Feedback(this IHtmlHelper html, UiTone tone, string message)
    {
        var services = html.ViewContext.HttpContext.RequestServices;
        var feedback = services.GetRequiredService<IFeedbackChannel>();
        var scoped = services.GetRequiredService<ICode311RequestFeedbackStore>();
        var model = new FeedbackMessage(tone, message, null, DateTimeOffset.UtcNow);
        feedback.Publish(model);
        scoped.Add(model);
    }

    /// <summary>
    /// Gets the currently resolved theme name for the request.
    /// </summary>
    /// <param name="html">The HTML helper.</param>
    /// <returns>The theme name, or <c>default</c> when no request theme has been resolved.</returns>
    public static string GetCode311ThemeName(this IHtmlHelper html)
    {
        var context = html.ViewContext.HttpContext.RequestServices.GetRequiredService<ICode311ThemeRequestContext>();
        return context.Current?.Name ?? "default";
    }
}
