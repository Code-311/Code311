using Code311.Tabler.Components.Common;
using Code311.Tabler.Core.Mapping;
using Code311.Ui.Abstractions.Semantics;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace Code311.Tabler.Components.Forms;

/// <summary>
/// Renders a semantic form field group.
/// </summary>
/// <remarks>
/// Field grouping is expressed with semantic layout and appearance parameters.
/// </remarks>
public sealed class Cd311FieldGroupViewComponent(ITablerSemanticClassMapper mapper) : ViewComponent
{
    public IViewComponentResult Invoke(string? legend, UiLayout layout = UiLayout.Stack, UiDensity density = UiDensity.Comfortable)
    {
        var html = $"<fieldset class=\"cd311-field-group {mapper.MapLayout(layout)} {mapper.MapDensity(density)}\"><legend>{legend}</legend></fieldset>";
        return new HtmlContentViewComponentResult(new HtmlString(html));
    }
}

/// <summary>
/// Renders a semantic form actions bar.
/// </summary>
/// <remarks>
/// The actions bar provides consistent action placement semantics for create/edit workflows.
/// </remarks>
public sealed class Cd311FormActionsBarViewComponent(ITablerSemanticClassMapper mapper) : ViewComponent
{
    public IViewComponentResult Invoke(IReadOnlyCollection<Cd311ActionItem>? actions, UiPinned pinned = UiPinned.None)
    {
        var items = string.Join(string.Empty, (actions ?? []).Select(a => $"<button class=\"btn {(a.Primary ? "btn-primary" : "btn-secondary")}\">{a.Text}</button>"));
        var html = $"<div class=\"cd311-form-actions {mapper.MapPinned(pinned)}\">{items}</div>";
        return new HtmlContentViewComponentResult(new HtmlString(html));
    }
}
