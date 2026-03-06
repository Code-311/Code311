using Code311.Tabler.Core.Mapping;
using Code311.Ui.Abstractions.Semantics;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Code311.Tabler.Components.Layout;

[HtmlTargetElement("cd311-container")]
public sealed class Cd311ContainerTagHelper(ITablerSemanticClassMapper mapper) : TagHelper
{
    public UiLayout Layout { get; set; } = UiLayout.Stack;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", $"container {mapper.MapLayout(Layout)}");
    }
}

[HtmlTargetElement("cd311-section")]
public sealed class Cd311SectionTagHelper : TagHelper
{
    public string? Title { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "section";
        if (!string.IsNullOrWhiteSpace(Title))
        {
            output.PreContent.SetHtmlContent($"<h2>{Title}</h2>");
        }
    }
}

public sealed class Cd311PageHeaderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string title, string? subtitle = null)
    {
        var html = $"<header class=\"page-header\"><h1>{title}</h1><p>{subtitle}</p></header>";
        return new HtmlContentViewComponentResult(new HtmlString(html));
    }
}

[HtmlTargetElement("cd311-card")]
public sealed class Cd311CardTagHelper(ITablerSemanticClassMapper mapper) : TagHelper
{
    public string? Title { get; set; }
    public UiTone Tone { get; set; } = UiTone.Neutral;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "article";
        output.Attributes.SetAttribute("class", $"card border-{mapper.MapTone(Tone)}");
        if (!string.IsNullOrWhiteSpace(Title))
        {
            output.PreContent.SetHtmlContent($"<div class=\"card-header\">{Title}</div>");
        }
    }
}

[HtmlTargetElement("cd311-grid")]
public sealed class Cd311GridTagHelper : TagHelper
{
    public int Columns { get; set; } = 2;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", $"row row-cols-{Math.Max(1, Columns)}");
    }
}

[HtmlTargetElement("cd311-stack")]
public sealed class Cd311StackTagHelper(ITablerSemanticClassMapper mapper) : TagHelper
{
    public UiDensity Density { get; set; } = UiDensity.Comfortable;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", $"d-flex flex-column {mapper.MapDensity(Density)}");
    }
}

public sealed class Cd311AccordionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IReadOnlyCollection<string>? items)
    {
        var html = string.Join(string.Empty, (items ?? []).Select((x, i) => $"<div class=\"accordion-item\"><h2>{x}</h2><div id=\"acc-{i}\"></div></div>"));
        return new HtmlContentViewComponentResult(new HtmlString($"<div class=\"accordion\">{html}</div>"));
    }
}
