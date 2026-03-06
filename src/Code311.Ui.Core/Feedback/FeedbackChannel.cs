using Code311.Ui.Abstractions.Semantics;
using System.Collections.Concurrent;

namespace Code311.Ui.Core.Feedback;

/// <summary>
/// Represents a semantic feedback message produced by neutral orchestration services.
/// </summary>
/// <param name="Tone">The semantic tone.</param>
/// <param name="Message">The message text.</param>
/// <param name="Code">An optional code for diagnostics and localization.</param>
/// <param name="CreatedAt">The UTC creation timestamp.</param>
/// <remarks>
/// Feedback messages intentionally avoid design-system rendering details.
/// </remarks>
public sealed record FeedbackMessage(UiTone Tone, string Message, string? Code, DateTimeOffset CreatedAt);

/// <summary>
/// Defines a feedback pipeline channel for transient messages.
/// </summary>
/// <remarks>
/// The channel is suitable for in-process orchestration and can be adapted to host-specific transport models.
/// </remarks>
public interface IFeedbackChannel
{
    /// <summary>
    /// Publishes a feedback message.
    /// </summary>
    /// <param name="message">The message to publish.</param>
    /// <remarks>
    /// Implementations should preserve publish order.
    /// </remarks>
    void Publish(FeedbackMessage message);

    /// <summary>
    /// Drains all currently queued messages.
    /// </summary>
    /// <returns>The drained messages in publish order.</returns>
    /// <remarks>
    /// Draining clears the channel queue for the current in-memory instance.
    /// </remarks>
    IReadOnlyList<FeedbackMessage> Drain();
}

/// <summary>
/// Default in-memory feedback channel implementation.
/// </summary>
/// <remarks>
/// This channel is thread-safe and deterministic for typical request-scope and app-scope usage.
/// </remarks>
public sealed class InMemoryFeedbackChannel : IFeedbackChannel
{
    private readonly ConcurrentQueue<FeedbackMessage> _queue = new();

    /// <inheritdoc />
    public void Publish(FeedbackMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);
        _queue.Enqueue(message);
    }

    /// <inheritdoc />
    public IReadOnlyList<FeedbackMessage> Drain()
    {
        var drained = new List<FeedbackMessage>();
        while (_queue.TryDequeue(out var message))
        {
            drained.Add(message);
        }

        return drained;
    }
}
