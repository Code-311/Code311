using Code311.Ui.Abstractions.Semantics;

namespace Code311.Ui.Abstractions.Preferences;

/// <summary>
/// Represents a persisted user interface preference set.
/// </summary>
/// <remarks>
/// This model is tenant-aware and provider-neutral so persistence packages can implement storage without UI coupling.
/// </remarks>
public sealed record UserUiPreference
{
    /// <summary>
    /// Gets the tenant identifier for multi-tenant partitioning.
    /// </summary>
    /// <remarks>
    /// Implementations should treat this value as required for tenant-aware data segregation.
    /// </remarks>
    public required string TenantId { get; init; }

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    /// <remarks>
    /// The identifier format is application-defined and intentionally opaque to the framework.
    /// </remarks>
    public required string UserId { get; init; }

    /// <summary>
    /// Gets the selected theme profile name.
    /// </summary>
    /// <remarks>
    /// The value should match a registered <c>ThemeProfile</c> name when possible.
    /// </remarks>
    public required string Theme { get; init; }

    /// <summary>
    /// Gets the preferred density.
    /// </summary>
    /// <remarks>
    /// Density is used by layout and control rendering services.
    /// </remarks>
    public UiDensity Density { get; init; } = UiDensity.Comfortable;

    /// <summary>
    /// Gets a value indicating whether the sidebar is collapsed.
    /// </summary>
    /// <remarks>
    /// Sidebar rendering packages can use this value as initial navigation state.
    /// </remarks>
    public bool SidebarCollapsed { get; init; }

    /// <summary>
    /// Gets the default page size for list/data views.
    /// </summary>
    /// <remarks>
    /// Consumers can ignore this value for pages that require fixed server-side paging.
    /// </remarks>
    public int DefaultPageSize { get; init; } = 25;

    /// <summary>
    /// Gets the preferred language code.
    /// </summary>
    /// <remarks>
    /// The value should follow BCP-47 conventions where possible.
    /// </remarks>
    public string Language { get; init; } = "en";

    /// <summary>
    /// Gets the preferred time zone identifier.
    /// </summary>
    /// <remarks>
    /// The exact identifier system is left to host/application policy.
    /// </remarks>
    public string TimeZone { get; init; } = "UTC";

    /// <summary>
    /// Gets the UTC timestamp when the preference row was last updated.
    /// </summary>
    /// <remarks>
    /// Persistence implementations should set this value at write time.
    /// </remarks>
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}
