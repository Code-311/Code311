using Code311.Tabler.Components.Common;
using Code311.Tabler.Core.Mapping;
using Code311.Ui.Abstractions.Semantics;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Code311.Tabler.Components.Forms;

/// <summary>
/// Renders a semantic text input.
/// </summary>
/// <remarks>
/// This component accepts semantic parameters and maps rendering through Tabler mapping services.
/// </remarks>
[HtmlTargetElement("cd311-input")]
public sealed class Cd311InputTagHelper(ITablerSemanticClassMapper mapper) : TagHelper
{
    public string? Field { get; set; }
    public string? Label { get; set; }
    public UiState State { get; set; } = UiState.Default;
    public UiSize Size { get; set; } = UiSize.Medium;
    public UiAppearance Appearance { get; set; } = UiAppearance.Soft;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "input";
        output.TagMode = TagMode.SelfClosing;
        output.Attributes.SetAttribute("name", Field ?? string.Empty);
        output.Attributes.SetAttribute("aria-label", Label ?? Field ?? "input");
        output.AddClass($"form-control {mapper.SizeClass(Size)} state-{mapper.MapState(State)} app-{mapper.MapAppearance(Appearance)}");
    }
}

[HtmlTargetElement("cd311-textarea")]
public sealed class Cd311TextAreaTagHelper(ITablerSemanticClassMapper mapper) : TagHelper
{
    public string? Field { get; set; }
    public int Rows { get; set; } = 3;
    public UiState State { get; set; } = UiState.Default;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "textarea";
        output.Attributes.SetAttribute("name", Field ?? string.Empty);
        output.Attributes.SetAttribute("rows", Math.Max(1, Rows));
        output.AddClass($"form-control state-{mapper.MapState(State)}");
    }
}

[HtmlTargetElement("cd311-select")]
public sealed class Cd311SelectTagHelper(ITablerSemanticClassMapper mapper) : TagHelper
{
    public string? Field { get; set; }
    public IReadOnlyCollection<Cd311OptionItem>? Options { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "select";
        output.Attributes.SetAttribute("name", Field ?? string.Empty);
        output.AddClass("form-select");

        foreach (var option in Options ?? [])
        {
            var selected = option.Selected ? " selected" : string.Empty;
            var disabled = option.Disabled ? " disabled" : string.Empty;
            output.Content.AppendHtml($"<option value=\"{option.Value}\"{selected}{disabled}>{option.Text}</option>");
        }
    }
}

[HtmlTargetElement("cd311-checkbox")]
public sealed class Cd311CheckboxTagHelper(ITablerSemanticClassMapper mapper) : TagHelper
{
    public string? Field { get; set; }
    public bool Checked { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "input";
        output.TagMode = TagMode.SelfClosing;
        output.Attributes.SetAttribute("type", "checkbox");
        output.Attributes.SetAttribute("name", Field ?? string.Empty);
        if (Checked)
        {
            output.Attributes.SetAttribute("checked", "checked");
        }

        output.AddClass($"form-check-input state-{mapper.MapState(Checked ? UiState.Active : UiState.Default)}");
    }
}

[HtmlTargetElement("cd311-radio-group")]
public sealed class Cd311RadioGroupTagHelper : TagHelper
{
    public string? Field { get; set; }
    public IReadOnlyCollection<Cd311OptionItem>? Options { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.AddClass("cd311-radio-group");
        foreach (var option in Options ?? [])
        {
            var selected = option.Selected ? " checked" : string.Empty;
            var disabled = option.Disabled ? " disabled" : string.Empty;
            output.Content.AppendHtml($"<label><input type=\"radio\" name=\"{Field}\" value=\"{option.Value}\"{selected}{disabled}/> {option.Text}</label>");
        }
    }
}

[HtmlTargetElement("cd311-switch")]
public sealed class Cd311SwitchTagHelper(ITablerSemanticClassMapper mapper) : TagHelper
{
    public string? Field { get; set; }
    public bool Enabled { get; set; }
    public UiTone Tone { get; set; } = UiTone.Accent;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "input";
        output.TagMode = TagMode.SelfClosing;
        output.Attributes.SetAttribute("type", "checkbox");
        output.Attributes.SetAttribute("name", Field ?? string.Empty);
        if (Enabled)
        {
            output.Attributes.SetAttribute("checked", "checked");
        }

        output.AddClass($"form-check-input switch tone-{mapper.MapTone(Tone)}");
    }
}

[HtmlTargetElement("cd311-input-group")]
public sealed class Cd311InputGroupTagHelper(ITablerSemanticClassMapper mapper) : TagHelper
{
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.AddClass($"input-group app-{mapper.MapAppearance(UiAppearance.Soft)}");
        if (!string.IsNullOrWhiteSpace(Prefix))
        {
            output.Content.AppendHtml($"<span class=\"input-group-text\">{Prefix}</span>");
        }

        output.Content.AppendHtml("<input class=\"form-control\" />");

        if (!string.IsNullOrWhiteSpace(Suffix))
        {
            output.Content.AppendHtml($"<span class=\"input-group-text\">{Suffix}</span>");
        }
    }
}
