using Code311.Ui.Core.Feedback;

namespace Code311.Tabler.Razor.Feedback;

/// <summary>
/// Stores request-scoped feedback messages for Razor Pages.
/// </summary>
public interface ICode311RequestFeedbackStore
{
    void Add(FeedbackMessage message);
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
