using Code311.Ui.Core.Feedback;

namespace Code311.Tabler.Mvc.Feedback;

/// <summary>
/// Stores request-scoped feedback messages for MVC lifecycle handoff.
/// </summary>
/// <remarks>
/// This store complements <see cref="IFeedbackChannel"/> by preserving per-request determinism.
/// </remarks>
public interface ICode311RequestFeedbackStore
{
    /// <summary>
    /// Adds a feedback message to the request scope.
    /// </summary>
    /// <param name="message">The message.</param>
    void Add(FeedbackMessage message);

    /// <summary>
    /// Gets all request-scoped messages.
    /// </summary>
    /// <returns>The current request message list.</returns>
    IReadOnlyList<FeedbackMessage> GetAll();
}

internal sealed class Code311RequestFeedbackStore : ICode311RequestFeedbackStore
{
    private readonly List<FeedbackMessage> _messages = [];

    public void Add(FeedbackMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);
        _messages.Add(message);
    }

    public IReadOnlyList<FeedbackMessage> GetAll() => _messages;
}
