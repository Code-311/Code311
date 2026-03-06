using Code311.Tabler.Core.Mapping;
using Code311.Ui.Abstractions.Semantics;
using Code311.Ui.Core.Feedback;
using Code311.Ui.Core.Loading;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Code311.Tabler.Components.Feedback;

[HtmlTargetElement("cd311-alert")]
public sealed class Cd311AlertTagHelper(ITablerSemanticClassMapper mapper) : TagHelper
{
    public UiTone Tone { get; set; } = UiTone.Info;
    public string? Message { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", $"alert alert-{mapper.MapTone(Tone)}");
        output.Content.SetContent(Message ?? string.Empty);
    }
}

public sealed class Cd311ToastHostViewComponent(IFeedbackChannel feedbackChannel, ITablerSemanticClassMapper mapper) : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var messages = feedbackChannel.Drain();
        var html = string.Join(string.Empty, messages.Select(m => $"<div class=\"toast text-bg-{mapper.MapTone(m.Tone)}\">{m.Message}</div>"));
        return new HtmlContentViewComponentResult(new HtmlString($"<div class=\"toast-container\">{html}</div>"));
    }
}

public sealed class Cd311ModalViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string id, string title, string? body = null)
        => new HtmlContentViewComponentResult(new HtmlString($"<div id=\"{id}\" class=\"modal\"><div class=\"modal-header\">{title}</div><div class=\"modal-body\">{body}</div></div>"));
}

public sealed class Cd311ConfirmDialogViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string id, string question)
        => new HtmlContentViewComponentResult(new HtmlString($"<div id=\"{id}\" class=\"modal\"><div class=\"modal-body\">{question}</div><button class=\"btn btn-primary\">Confirm</button></div>"));
}

public sealed class Cd311PageAlertHostViewComponent(IFeedbackChannel feedbackChannel) : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var html = string.Join(string.Empty, feedbackChannel.Drain().Select(m => $"<div class=\"page-alert\">{m.Message}</div>"));
        return new HtmlContentViewComponentResult(new HtmlString($"<div class=\"cd311-page-alerts\">{html}</div>"));
    }
}

[HtmlTargetElement("cd311-busy-overlay")]
public sealed class Cd311BusyOverlayTagHelper(IBusyStateCoordinator coordinator) : TagHelper
{
    public string Scope { get; set; } = "default";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", $"busy-overlay {(coordinator.IsBusy(Scope) ? "active" : string.Empty)}");
    }
}

[HtmlTargetElement("cd311-busy-button")]
public sealed class Cd311BusyButtonTagHelper(IBusyStateCoordinator coordinator) : TagHelper
{
    public string Scope { get; set; } = "default";
    public string Label { get; set; } = "Submit";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "button";
        output.Attributes.SetAttribute("type", "button");
        output.Attributes.SetAttribute("class", "btn btn-primary");
        output.Content.SetContent(coordinator.IsBusy(Scope) ? "Working..." : Label);
    }
}

public sealed class Cd311PagePreloaderViewComponent(IPreloaderOrchestrator preloader) : ViewComponent
{
    public IViewComponentResult Invoke(string key = "default")
        => new HtmlContentViewComponentResult(new HtmlString($"<div class=\"page-preloader {(preloader.IsActive(key) ? "active" : string.Empty)}\"></div>"));
}

[HtmlTargetElement("cd311-progress")]
public sealed class Cd311ProgressTagHelper(ITablerSemanticClassMapper mapper) : TagHelper
{
    public int Value { get; set; }
    public UiTone Tone { get; set; } = UiTone.Info;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var clamped = Math.Clamp(Value, 0, 100);
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "progress");
        output.Content.SetHtmlContent($"<div class=\"progress-bar bg-{mapper.MapTone(Tone)}\" style=\"width:{clamped}%\"></div>");
    }
}
