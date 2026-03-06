using System.IO;
using Code311.Tabler.Components.Common;
using Code311.Tabler.Components.Data;
using Code311.Tabler.Components.Feedback;
using Code311.Tabler.Components.Forms;
using Code311.Tabler.Components.Layout;
using Code311.Tabler.Components.Media;
using Code311.Tabler.Components.Navigation;
using Code311.Tabler.Core.Mapping;
using Code311.Ui.Abstractions.Semantics;
using Code311.Ui.Core.Feedback;
using Code311.Ui.Core.Loading;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace Code311.Tests.Tabler.Components;

/// <summary>
/// Validates representative TagHelper and component behavior for the core component subset.
/// </summary>
/// <remarks>
/// These tests intentionally focus on semantic-to-render mapping assertions rather than exhaustive HTML snapshots.
/// </remarks>
public sealed class ComponentRenderingTests
{
    [Fact]
    public void Cd311Input_ShouldRenderSemanticClasses()
    {
        var helper = new Cd311InputTagHelper(new TablerSemanticClassMapper())
        {
            Field = "Email",
            Label = "Email",
            State = UiState.Active,
            Size = UiSize.Large,
            Appearance = UiAppearance.Outline
        };

        var output = CreateOutput();
        helper.Process(CreateContext(), output);

        var cls = output.Attributes["class"].Value?.ToString() ?? string.Empty;
        Assert.Contains("form-control", cls);
        Assert.Contains("state-active", cls);
    }

    [Fact]
    public void Cd311Tabs_ShouldRenderItems()
    {
        var helper = new Cd311TabsTagHelper(new TablerSemanticClassMapper())
        {
            Items =
            [
                new Cd311NavItem("Overview", "/", true),
                new Cd311NavItem("Settings", "/settings")
            ]
        };

        var output = CreateOutput();
        helper.Process(CreateContext(), output);

        var content = output.Content.GetContent();
        Assert.Contains("Overview", content);
        Assert.Contains("Settings", content);
    }

    [Fact]
    public void Cd311Card_ShouldSetToneClass()
    {
        var helper = new Cd311CardTagHelper(new TablerSemanticClassMapper()) { Tone = UiTone.Warning, Title = "Card" };
        var output = CreateOutput();

        helper.Process(CreateContext(), output);

        Assert.Contains("border-warning", output.Attributes["class"].Value?.ToString());
    }

    [Fact]
    public void Cd311Progress_ShouldClampAndRenderBar()
    {
        var helper = new Cd311ProgressTagHelper(new TablerSemanticClassMapper()) { Value = 150, Tone = UiTone.Success };
        var output = CreateOutput();

        helper.Process(CreateContext(), output);

        var content = output.Content.GetContent();
        Assert.Contains("width:100%", content);
        Assert.Contains("bg-success", content);
    }

    [Fact]
    public void Cd311Badge_ShouldRenderTone()
    {
        var helper = new Cd311BadgeTagHelper(new TablerSemanticClassMapper()) { Tone = UiTone.Danger, Text = "Blocked" };
        var output = CreateOutput();

        helper.Process(CreateContext(), output);

        Assert.Contains("bg-danger", output.Attributes["class"].Value?.ToString());
        Assert.Equal("Blocked", output.Content.GetContent());
    }

    [Fact]
    public void Cd311Avatar_ShouldFallbackToInitials()
    {
        var helper = new Cd311AvatarTagHelper(new TablerSemanticClassMapper()) { Name = "Code Three" };
        var output = CreateOutput();

        helper.Process(CreateContext(), output);

        Assert.Contains("avatar", output.Attributes["class"].Value?.ToString());
        Assert.Equal("CO", output.Content.GetContent());
    }

    [Fact]
    public void BusyAndAlertComponents_ShouldConsumeUiCoreServices()
    {
        var busy = new BusyStateCoordinator();
        using var _ = busy.EnterScope("submit");

        var overlay = new Cd311BusyOverlayTagHelper(busy) { Scope = "submit" };
        var overlayOutput = CreateOutput();
        overlay.Process(CreateContext(), overlayOutput);

        var feedback = new InMemoryFeedbackChannel();
        feedback.Publish(new FeedbackMessage(UiTone.Info, "Saved", null, DateTimeOffset.UtcNow));
        var host = new Cd311PageAlertHostViewComponent(feedback);

        Assert.Contains("active", overlayOutput.Attributes["class"].Value?.ToString());
        var result = Assert.IsType<Microsoft.AspNetCore.Mvc.ViewComponents.HtmlContentViewComponentResult>(host.Invoke());
        using var writer = new StringWriter();
        result.HtmlContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
        Assert.Contains("Saved", writer.ToString());
    }

    private static TagHelperContext CreateContext() =>
        new(new TagHelperAttributeList(), new Dictionary<object, object?>(), Guid.NewGuid().ToString("N"));

    private static TagHelperOutput CreateOutput() =>
        new("div", new TagHelperAttributeList(), static (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
}
