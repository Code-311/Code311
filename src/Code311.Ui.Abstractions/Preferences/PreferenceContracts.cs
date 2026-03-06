namespace Code311.Ui.Abstractions.Preferences;

/// <summary>
/// Persists and retrieves user UI preferences.
/// </summary>
/// <remarks>
/// Persistence implementations are expected in infrastructure packages and must support tenant-aware operations.
/// </remarks>
public interface IUserUiPreferenceStore
{
    /// <summary>
    /// Gets a user's preferences.
    /// </summary>
    /// <param name="tenantId">The tenant identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The user preference, or <see langword="null"/> when none exists.</returns>
    /// <remarks>
    /// Retrieval should be side-effect free and safe for repeated invocation.
    /// </remarks>
    Task<UserUiPreference?> GetAsync(string tenantId, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Upserts a user's preference record.
    /// </summary>
    /// <param name="preference">The preference model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing completion.</returns>
    /// <remarks>
    /// Implementations should treat <see cref="UserUiPreference.TenantId"/> and <see cref="UserUiPreference.UserId"/> as a composite key.
    /// </remarks>
    Task UpsertAsync(UserUiPreference preference, CancellationToken cancellationToken = default);
}
