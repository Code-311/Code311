using Code311.Tabler.Dashboard.Models;

namespace Code311.Tabler.Dashboard.Composition;

/// <summary>
/// Provides dashboard page composition helpers.
/// </summary>
/// <remarks>
/// The composer transforms zone and panel models into composition-ready HTML fragments.
/// </remarks>
public interface IDashboardPageComposer
{
    /// <summary>
    /// Composes a dashboard page model into HTML.
    /// </summary>
    /// <param name="model">The page model.</param>
    /// <returns>A composed HTML payload.</returns>
    string Compose(DashboardPageModel model);
}

/// <summary>
/// Provides optional dashboard personalization hooks.
/// </summary>
/// <remarks>
/// Hooks remain abstraction-only and avoid persistence implementation details.
/// </remarks>
public interface IDashboardPersonalizationHook
{
    /// <summary>
    /// Applies personalization to a page model.
    /// </summary>
    /// <param name="model">The model to personalize.</param>
    /// <returns>The updated model.</returns>
    DashboardPageModel Apply(DashboardPageModel model);
}

internal sealed class DefaultDashboardPageComposer : IDashboardPageComposer
{
    public string Compose(DashboardPageModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        var zones = string.Join(string.Empty, model.Zones.Select(z =>
            $"<section data-zone=\"{z.Key}\"><h2>{z.Title}</h2>{string.Join(string.Empty, z.Panels.Select(p => p.BodyHtml))}</section>"));

        return $"<div class=\"cd311-dashboard-page\"><header><h1>{model.Title}</h1></header>{zones}</div>";
    }
}
