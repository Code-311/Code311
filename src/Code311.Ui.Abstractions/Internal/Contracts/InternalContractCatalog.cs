namespace Code311.Ui.Abstractions.Internal.Contracts;

/// <summary>
/// Maps form semantics to Tabler render classes.
/// </summary>
/// <remarks>
/// Tabler mappers are intentionally kept in abstractions so <c>Code311.Tabler.Core</c> can implement
/// mappings while public API semantics remain design-system-neutral.
/// </remarks>
public interface ITablerFormClassMapper;

/// <summary>
/// Maps navigation semantics to Tabler render classes.
/// </summary>
/// <remarks>
/// Mapping contracts isolate Tabler specifics from consumer-facing component APIs.
/// </remarks>
public interface ITablerNavigationClassMapper;

/// <summary>
/// Maps layout semantics to Tabler render classes.
/// </summary>
/// <remarks>
/// Layout mapping contracts are consumed by Tabler implementation packages only.
/// </remarks>
public interface ITablerLayoutClassMapper;

/// <summary>
/// Maps feedback semantics to Tabler render classes.
/// </summary>
/// <remarks>
/// Feedback mappings provide deterministic intent-to-style translation.
/// </remarks>
public interface ITablerFeedbackClassMapper;

/// <summary>
/// Maps data semantics to Tabler render classes.
/// </summary>
/// <remarks>
/// Data mappings support semantic rendering for tables, badges, and KPI visuals.
/// </remarks>
public interface ITablerDataClassMapper;

/// <summary>
/// Maps media semantics to Tabler render classes.
/// </summary>
/// <remarks>
/// Media mappings apply to semantic avatar, image, banner, and file-preview components.
/// </remarks>
public interface ITablerMediaClassMapper;
