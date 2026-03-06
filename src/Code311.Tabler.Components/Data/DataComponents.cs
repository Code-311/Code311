using Code311.Tabler.Components.Common;
using Code311.Tabler.Core.Mapping;
using Code311.Ui.Abstractions.Semantics;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Code311.Tabler.Components.Data;

[HtmlTargetElement("cd311-table")]
public sealed class Cd311TableTagHelper : TagHelper
{
    public IReadOnlyCollection<string>? Headers { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "table";
        output.Attributes.SetAttribute("class", "table");
        var headerHtml = string.Join(string.Empty, (Headers ?? []).Select(h => $"<th>{h}</th>"));
        output.PreContent.SetHtmlContent($"<thead><tr>{headerHtml}</tr></thead><tbody></tbody>");
    }
}

public sealed class Cd311FilterToolbarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IReadOnlyCollection<Cd311OptionItem>? filters)
    {
        var html = string.Join(string.Empty, (filters ?? []).Select(f => $"<button class=\"btn btn-outline-secondary\">{f.Text}</button>"));
        return new HtmlContentViewComponentResult(new HtmlString($"<div class=\"cd311-filter-toolbar\">{html}</div>"));
    }
}

[HtmlTargetElement("cd311-badge")]
public sealed class Cd311BadgeTagHelper(ITablerSemanticClassMapper mapper) : TagHelper
{
    public UiTone Tone { get; set; } = UiTone.Neutral;
    public string Text { get; set; } = string.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        output.Attributes.SetAttribute("class", $"badge bg-{mapper.MapTone(Tone)}");
        output.Content.SetContent(Text);
    }
}

public sealed class Cd311StatKpiViewComponent(ITablerSemanticClassMapper mapper) : ViewComponent
{
    public IViewComponentResult Invoke(string label, string value, UiTone tone = UiTone.Info)
        => new HtmlContentViewComponentResult(new HtmlString($"<div class=\"kpi text-{mapper.MapTone(tone)}\"><div>{label}</div><strong>{value}</strong></div>"));
}

[HtmlTargetElement("cd311-list-group")]
public sealed class Cd311ListGroupTagHelper : TagHelper
{
    public IReadOnlyCollection<string>? Items { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        output.Attributes.SetAttribute("class", "list-group");
        foreach (var item in Items ?? [])
        {
            output.Content.AppendHtml($"<li class=\"list-group-item\">{item}</li>");
        }
    }
}

[HtmlTargetElement("cd311-key-value-list")]
public sealed class Cd311KeyValueListTagHelper : TagHelper
{
    public IReadOnlyCollection<Cd311KeyValueItem>? Items { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "dl";
        output.Attributes.SetAttribute("class", "row");
        foreach (var kv in Items ?? [])
        {
            output.Content.AppendHtml($"<dt class=\"col-sm-4\">{kv.Key}</dt><dd class=\"col-sm-8\">{kv.Value}</dd>");
        }
    }
}

[HtmlTargetElement("cd311-empty-state")]
public sealed class Cd311EmptyStateTagHelper : TagHelper
{
    public string Title { get; set; } = "No data";
    public string? Description { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "empty");
        output.Content.SetHtmlContent($"<p class=\"empty-title\">{Title}</p><p class=\"empty-subtitle\">{Description}</p>");
    }
}
