using Code311.Tabler.Core.Mapping;
using Code311.Ui.Abstractions.Semantics;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Code311.Tabler.Components.Media;

[HtmlTargetElement("cd311-avatar")]
public sealed class Cd311AvatarTagHelper(ITablerSemanticClassMapper mapper) : TagHelper
{
    public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public UiTone Tone { get; set; } = UiTone.Neutral;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        output.Attributes.SetAttribute("class", $"avatar bg-{mapper.MapTone(Tone)}");
        if (!string.IsNullOrWhiteSpace(ImageUrl))
        {
            output.Attributes.SetAttribute("style", $"background-image:url('{ImageUrl}')");
        }
        else
        {
            output.Content.SetContent(new string(Name.Where(char.IsLetter).Take(2).ToArray()).ToUpperInvariant());
        }
    }
}

[HtmlTargetElement("cd311-icon")]
public sealed class Cd311IconTagHelper : TagHelper
{
    public string Name { get; set; } = "circle";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "i";
        output.Attributes.SetAttribute("class", $"ti ti-{Name}");
    }
}

[HtmlTargetElement("cd311-image")]
public sealed class Cd311ImageTagHelper : TagHelper
{
    public string Src { get; set; } = string.Empty;
    public string Alt { get; set; } = string.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "img";
        output.TagMode = TagMode.SelfClosing;
        output.Attributes.SetAttribute("src", Src);
        output.Attributes.SetAttribute("alt", Alt);
        output.Attributes.SetAttribute("class", "img-fluid");
    }
}

public sealed class Cd311FilePreviewViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string fileName, string mediaType)
        => new HtmlContentViewComponentResult(new HtmlString($"<div class=\"file-preview\"><strong>{fileName}</strong><small>{mediaType}</small></div>"));
}

public sealed class Cd311ProfileBlockViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string title, string subtitle)
        => new HtmlContentViewComponentResult(new HtmlString($"<div class=\"profile-block\"><h4>{title}</h4><p>{subtitle}</p></div>"));
}
