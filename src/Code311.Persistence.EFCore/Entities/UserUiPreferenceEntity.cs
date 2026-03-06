using Code311.Ui.Abstractions.Semantics;

namespace Code311.Persistence.EFCore.Entities;

/// <summary>
/// EF Core persistence model for a tenant-scoped user UI preference record.
/// </summary>
public sealed class UserUiPreferenceEntity
{
    public required string TenantId { get; set; }
    public required string UserId { get; set; }
    public required string Theme { get; set; }
    public UiDensity Density { get; set; }
    public bool SidebarCollapsed { get; set; }
    public int DefaultPageSize { get; set; }
    public string Language { get; set; } = "en";
    public string TimeZone { get; set; } = "UTC";
    public DateTimeOffset UpdatedAt { get; set; }
}
