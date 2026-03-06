using Code311.Tabler.Components.Common;
using Code311.Ui.Abstractions.Semantics;

namespace Code311.Tabler.Dashboard.Models;

/// <summary>
/// Represents a dashboard page model.
/// </summary>
/// <remarks>
/// Dashboard pages are composition surfaces that organize semantic zones and panels.
/// </remarks>
public sealed record DashboardPageModel(string Title, IReadOnlyCollection<DashboardZoneModel> Zones);

/// <summary>
/// Represents a dashboard zone/region.
/// </summary>
/// <param name="Key">The zone key.</param>
/// <param name="Title">The zone title.</param>
/// <param name="Layout">The semantic layout for the zone.</param>
/// <param name="Panels">Panels in this zone.</param>
/// <remarks>
/// Zones are intentionally generic so future personalization can reorder or hide zones.
/// </remarks>
public sealed record DashboardZoneModel(string Key, string Title, UiLayout Layout, IReadOnlyCollection<DashboardPanelModel> Panels);

/// <summary>
/// Represents a dashboard panel composition model.
/// </summary>
/// <param name="PanelKey">The panel key.</param>
/// <param name="Title">The panel title.</param>
/// <param name="BodyHtml">Panel body HTML.</param>
/// <param name="Tone">Panel semantic tone.</param>
/// <remarks>
/// Panels are composition-level units and do not own base component semantics.
/// </remarks>
public sealed record DashboardPanelModel(string PanelKey, string Title, string BodyHtml, UiTone Tone = UiTone.Neutral);

/// <summary>
/// Represents a KPI/stat summary item.
/// </summary>
/// <param name="Label">The KPI label.</param>
/// <param name="Value">The KPI value.</param>
/// <param name="Tone">The semantic tone.</param>
/// <param name="Trend">Optional trend label.</param>
public sealed record DashboardKpiItem(string Label, string Value, UiTone Tone = UiTone.Info, string? Trend = null);

/// <summary>
/// Represents an activity feed item.
/// </summary>
/// <param name="Text">The activity text.</param>
/// <param name="OccurredAt">The occurrence timestamp.</param>
/// <param name="Tone">The semantic tone.</param>
public sealed record DashboardActivityItem(string Text, DateTimeOffset OccurredAt, UiTone Tone = UiTone.Neutral);

/// <summary>
/// Represents quick action entry metadata.
/// </summary>
/// <param name="Action">The action descriptor.</param>
/// <param name="Tone">The semantic tone.</param>
public sealed record DashboardQuickAction(Cd311ActionItem Action, UiTone Tone = UiTone.Accent);
