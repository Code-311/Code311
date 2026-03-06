using Code311.Tabler.Razor.Feedback;
using Code311.Ui.Abstractions.Semantics;
using Code311.Ui.Core.Feedback;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Code311.Tabler.Razor.Helpers;

/// <summary>
/// Optional lightweight base type for Code311-enabled Razor Pages.
/// </summary>
public abstract class Code311PageModelBase(IFeedbackChannel feedbackChannel, ICode311RequestFeedbackStore requestFeedbackStore) : PageModel
{
    /// <summary>
    /// Publishes semantic feedback for current request.
    /// </summary>
    protected void PublishFeedback(UiTone tone, string message)
    {
        var model = new FeedbackMessage(tone, message, null, DateTimeOffset.UtcNow);
        feedbackChannel.Publish(model);
        requestFeedbackStore.Add(model);
    }
}
