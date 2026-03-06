using Code311.Tabler.Components.Common;
using Code311.Tabler.Core.Mapping;
using Code311.Ui.Abstractions.Semantics;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Code311.Tabler.Components.Navigation;

public sealed class Cd311TopNavViewComponent(ITablerSemanticClassMapper mapper) : ViewComponent
{
    public IViewComponentResult Invoke(string? brand, IReadOnlyCollection<Cd311NavItem>? items, UiPinned pinned = UiPinned.None)
    {
        var links = string.Join(string.Empty, (items ?? []).Select(i => $"<a href=\"{i.Url}\" class=\"nav-link {(i.Active ? "active" : string.Empty)}\">{i.Text}</a>"));
        return new HtmlContentViewComponentResult(new HtmlString($"<nav class=\"navbar {mapper.MapPinned(pinned)}\"><span>{brand}</span>{links}</nav>"));
    }
}

public sealed class Cd311SidebarNavViewComponent(ITablerSemanticClassMapper mapper) : ViewComponent
{
    public IViewComponentResult Invoke(IReadOnlyCollection<Cd311NavItem>? sections, UiState state = UiState.Default)
    {
        var links = string.Join(string.Empty, (sections ?? []).Select(i => $"<li class=\"nav-item\"><a href=\"{i.Url}\">{i.Text}</a></li>"));
        return new HtmlContentViewComponentResult(new HtmlString($"<aside class=\"navbar-vertical state-{mapper.MapState(state)}\"><ul>{links}</ul></aside>"));
    }
}

[HtmlTargetElement("cd311-breadcrumb")]
public sealed class Cd311BreadcrumbTagHelper : TagHelper
{
    public IReadOnlyCollection<Cd311NavItem>? Items { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "nav";
        output.AddClass("breadcrumb");
        var html = string.Join(string.Empty, (Items ?? []).Select(i => $"<a class=\"breadcrumb-item\" href=\"{i.Url}\">{i.Text}</a>"));
        output.Content.SetHtmlContent(html);
    }
}

[HtmlTargetElement("cd311-pagination")]
public sealed class Cd311PaginationTagHelper : TagHelper
{
    public int Page { get; set; } = 1;
    public int Total { get; set; } = 1;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        output.AddClass("pagination");
        for (var i = 1; i <= Math.Max(1, Total); i++)
        {
            output.Content.AppendHtml($"<li class=\"page-item {(i == Page ? "active" : string.Empty)}\"><a class=\"page-link\">{i}</a></li>");
        }
    }
}

[HtmlTargetElement("cd311-tabs")]
public sealed class Cd311TabsTagHelper(ITablerSemanticClassMapper mapper) : TagHelper
{
    public IReadOnlyCollection<Cd311NavItem>? Items { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        output.AddClass($"nav nav-tabs {mapper.MapLayout(UiLayout.Inline)}");
        foreach (var item in Items ?? [])
        {
            output.Content.AppendHtml($"<li class=\"nav-item\"><a class=\"nav-link {(item.Active ? "active" : string.Empty)}\" href=\"{item.Url}\">{item.Text}</a></li>");
        }
    }
}

public sealed class Cd311DropdownMenuViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string title, IReadOnlyCollection<Cd311ActionItem>? items)
    {
        var htmlItems = string.Join(string.Empty, (items ?? []).Select(i => $"<li><button class=\"dropdown-item\">{i.Text}</button></li>"));
        var html = $"<div class=\"dropdown\"><button class=\"btn dropdown-toggle\">{title}</button><ul class=\"dropdown-menu\">{htmlItems}</ul></div>";
        return new HtmlContentViewComponentResult(new HtmlString(html));
    }
}
