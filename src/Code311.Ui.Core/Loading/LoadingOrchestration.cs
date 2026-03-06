using System.Collections.Concurrent;

namespace Code311.Ui.Core.Loading;

/// <summary>
/// Coordinates semantic busy-state scopes.
/// </summary>
/// <remarks>
/// Busy coordination is intentionally rendering-agnostic and can be consumed by any host adapter.
/// </remarks>
public interface IBusyStateCoordinator
{
    /// <summary>
    /// Enters a busy scope.
    /// </summary>
    /// <param name="scope">The logical scope key.</param>
    /// <returns>An <see cref="IDisposable"/> that exits the scope when disposed.</returns>
    /// <remarks>
    /// Nested scopes increment counters and require matching disposal to clear busy state.
    /// </remarks>
    IDisposable EnterScope(string scope);

    /// <summary>
    /// Gets a value indicating whether the specified scope is currently busy.
    /// </summary>
    /// <param name="scope">The logical scope key.</param>
    /// <returns><see langword="true"/> when busy; otherwise <see langword="false"/>.</returns>
    /// <remarks>
    /// Scope checks are safe for concurrent access.
    /// </remarks>
    bool IsBusy(string scope);
}

/// <summary>
/// Coordinates semantic preloader state.
/// </summary>
/// <remarks>
/// Preloader orchestration allows hosts to trigger and clear loader states without UI-framework coupling.
/// </remarks>
public interface IPreloaderOrchestrator
{
    /// <summary>
    /// Marks a preloader key as active.
    /// </summary>
    /// <param name="key">The preloader key.</param>
    /// <remarks>
    /// Repeated calls are idempotent for the same key.
    /// </remarks>
    void Start(string key = "default");

    /// <summary>
    /// Marks a preloader key as inactive.
    /// </summary>
    /// <param name="key">The preloader key.</param>
    /// <remarks>
    /// Completing a missing key is safe and has no effect.
    /// </remarks>
    void Complete(string key = "default");

    /// <summary>
    /// Gets a value indicating whether a key is currently active.
    /// </summary>
    /// <param name="key">The preloader key.</param>
    /// <returns><see langword="true"/> when active; otherwise <see langword="false"/>.</returns>
    /// <remarks>
    /// Key checks are case-insensitive.
    /// </remarks>
    bool IsActive(string key = "default");
}

/// <summary>
/// Default in-memory busy-state coordinator.
/// </summary>
/// <remarks>
/// Counters are tracked per scope key and support nested scope usage.
/// </remarks>
public sealed class BusyStateCoordinator : IBusyStateCoordinator
{
    private readonly ConcurrentDictionary<string, int> _counters = new(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc />
    public IDisposable EnterScope(string scope)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(scope);
        _counters.AddOrUpdate(scope, 1, static (_, current) => current + 1);
        return new Scope(this, scope);
    }

    /// <inheritdoc />
    public bool IsBusy(string scope)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(scope);
        return _counters.TryGetValue(scope, out var value) && value > 0;
    }

    private void ExitScope(string scope)
    {
        _counters.AddOrUpdate(scope, 0, static (_, current) => Math.Max(0, current - 1));

        if (_counters.TryGetValue(scope, out var value) && value == 0)
        {
            _counters.TryRemove(scope, out _);
        }
    }

    private sealed class Scope : IDisposable
    {
        private readonly BusyStateCoordinator _owner;
        private readonly string _scope;
        private bool _disposed;

        public Scope(BusyStateCoordinator owner, string scope)
        {
            _owner = owner;
            _scope = scope;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _owner.ExitScope(_scope);
            _disposed = true;
        }
    }
}

/// <summary>
/// Default in-memory preloader orchestrator.
/// </summary>
/// <remarks>
/// Active keys are tracked as a set for predictable activation checks.
/// </remarks>
public sealed class PreloaderOrchestrator : IPreloaderOrchestrator
{
    private readonly ConcurrentDictionary<string, byte> _activeKeys = new(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc />
    public void Start(string key = "default")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        _activeKeys[key] = 0;
    }

    /// <inheritdoc />
    public void Complete(string key = "default")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        _activeKeys.TryRemove(key, out _);
    }

    /// <inheritdoc />
    public bool IsActive(string key = "default")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        return _activeKeys.ContainsKey(key);
    }
}
