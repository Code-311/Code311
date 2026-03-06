using Code311.Tabler.Mvc.Feedback;
using Code311.Ui.Abstractions.Semantics;
using Code311.Ui.Core.Feedback;
using Microsoft.AspNetCore.Mvc;

namespace Code311.Tabler.Mvc.Helpers;

/// <summary>
/// Provides optional, lightweight base controller helpers for Code311-enabled MVC apps.
/// </summary>
/// <remarks>
/// Inheriting from this type is optional and does not affect integration behavior.
/// </remarks>
public abstract class Code311ControllerBase(IFeedbackChannel feedbackChannel, ICode311RequestFeedbackStore requestFeedbackStore) : Controller
{
    /// <summary>
    /// Publishes a semantic feedback message for the current request.
    /// </summary>
    /// <param name="tone">The semantic tone.</param>
    /// <param name="message">The message.</param>
    protected void PublishFeedback(UiTone tone, string message)
    {
        var model = new FeedbackMessage(tone, message, null, DateTimeOffset.UtcNow);
        feedbackChannel.Publish(model);
        requestFeedbackStore.Add(model);
    }
}
