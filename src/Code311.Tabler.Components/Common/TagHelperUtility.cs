using Code311.Tabler.Core.Mapping;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Code311.Tabler.Components.Common;

internal static class TagHelperUtility
{
    public static TagHelperContext CreateContext() =>
        new(
            new TagHelperAttributeList(),
            new Dictionary<object, object?>(),
            Guid.NewGuid().ToString("N"));

    public static TagHelperOutput CreateOutput(string tagName = "div") =>
        new(
            tagName,
            new TagHelperAttributeList(),
            static (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

    public static void AddClass(this TagHelperOutput output, string? @class)
    {
        if (string.IsNullOrWhiteSpace(@class))
        {
            return;
        }

        if (output.Attributes.TryGetAttribute("class", out var existing) && existing.Value is string existingValue)
        {
            output.Attributes.SetAttribute("class", $"{existingValue} {@class}".Trim());
            return;
        }

        output.Attributes.SetAttribute("class", @class);
    }

    public static string SizeClass(this ITablerSemanticClassMapper mapper, Code311.Ui.Abstractions.Semantics.UiSize size)
        => $"size-{mapper.MapSize(size)}";
}
