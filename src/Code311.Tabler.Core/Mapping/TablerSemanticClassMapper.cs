using Code311.Ui.Abstractions.Internal.Contracts;
using Code311.Ui.Abstractions.Semantics;

namespace Code311.Tabler.Core.Mapping;

/// <summary>
/// Exposes semantic-to-Tabler class mapping operations.
/// </summary>
/// <remarks>
/// This mapper centralizes Tabler-specific class translations so upstream packages stay semantic and neutral.
/// </remarks>
public interface ITablerSemanticClassMapper
{
    /// <summary>
    /// Maps semantic tone to Tabler class suffix.
    /// </summary>
    /// <param name="tone">The semantic tone.</param>
    /// <returns>The mapped Tabler class token.</returns>
    /// <remarks>
    /// Returned values are implementation details for Tabler packages.
    /// </remarks>
    string MapTone(UiTone tone);

    /// <summary>
    /// Maps semantic appearance to Tabler class token.
    /// </summary>
    /// <param name="appearance">The semantic appearance.</param>
    /// <returns>The mapped class token.</returns>
    /// <remarks>
    /// The mapping avoids exposing Tabler tokens in consumer APIs.
    /// </remarks>
    string MapAppearance(UiAppearance appearance);

    /// <summary>
    /// Maps semantic density to Tabler spacing class token.
    /// </summary>
    /// <param name="density">The semantic density.</param>
    /// <returns>The mapped class token.</returns>
    /// <remarks>
    /// Density mappings are used by layout and form groups.
    /// </remarks>
    string MapDensity(UiDensity density);

    /// <summary>
    /// Maps semantic size to Tabler size class token.
    /// </summary>
    /// <param name="size">The semantic size.</param>
    /// <returns>The mapped class token.</returns>
    /// <remarks>
    /// Size mappings can be reused by controls and media.
    /// </remarks>
    string MapSize(UiSize size);

    /// <summary>
    /// Maps semantic state to Tabler class token.
    /// </summary>
    /// <param name="state">The semantic state.</param>
    /// <returns>The mapped class token.</returns>
    /// <remarks>
    /// State mappings support active, disabled, read-only, and busy scenarios.
    /// </remarks>
    string MapState(UiState state);

    /// <summary>
    /// Maps semantic placement to Tabler class token.
    /// </summary>
    /// <param name="placement">The semantic placement.</param>
    /// <returns>The mapped class token.</returns>
    /// <remarks>
    /// Placement tokens are used by popovers, tooltips, and toast positions.
    /// </remarks>
    string MapPlacement(UiPlacement placement);

    /// <summary>
    /// Maps semantic layout to Tabler class token.
    /// </summary>
    /// <param name="layout">The semantic layout intent.</param>
    /// <returns>The mapped class token.</returns>
    /// <remarks>
    /// Layout mappings are intentionally coarse and can be refined in later phases.
    /// </remarks>
    string MapLayout(UiLayout layout);

    /// <summary>
    /// Maps semantic pinning to Tabler class token.
    /// </summary>
    /// <param name="pinned">The semantic pinning intent.</param>
    /// <returns>The mapped class token.</returns>
    /// <remarks>
    /// Pinning mappings are consumed by header/sidebar shell components.
    /// </remarks>
    string MapPinned(UiPinned pinned);
}

/// <summary>
/// Default semantic-to-Tabler mapper implementation.
/// </summary>
/// <remarks>
/// This type implements all approved Tabler mapper abstraction contracts.
/// </remarks>
public sealed class TablerSemanticClassMapper :
    ITablerSemanticClassMapper,
    ITablerFormClassMapper,
    ITablerNavigationClassMapper,
    ITablerLayoutClassMapper,
    ITablerFeedbackClassMapper,
    ITablerDataClassMapper,
    ITablerMediaClassMapper
{
    /// <inheritdoc />
    public string MapTone(UiTone tone) => tone switch
    {
        UiTone.Neutral => "secondary",
        UiTone.Accent => "primary",
        UiTone.Success => "success",
        UiTone.Warning => "warning",
        UiTone.Danger => "danger",
        UiTone.Info => "info",
        _ => "secondary"
    };

    /// <inheritdoc />
    public string MapAppearance(UiAppearance appearance) => appearance switch
    {
        UiAppearance.Solid => "",
        UiAppearance.Soft => "-lt",
        UiAppearance.Outline => "-outline",
        UiAppearance.Ghost => "-ghost",
        UiAppearance.Link => "-link",
        _ => string.Empty
    };

    /// <inheritdoc />
    public string MapDensity(UiDensity density) => density switch
    {
        UiDensity.Compact => "density-compact",
        UiDensity.Comfortable => "density-comfortable",
        UiDensity.Spacious => "density-spacious",
        _ => "density-comfortable"
    };

    /// <inheritdoc />
    public string MapSize(UiSize size) => size switch
    {
        UiSize.Small => "sm",
        UiSize.Medium => "md",
        UiSize.Large => "lg",
        _ => "md"
    };

    /// <inheritdoc />
    public string MapState(UiState state) => state switch
    {
        UiState.Default => "",
        UiState.Active => "active",
        UiState.Disabled => "disabled",
        UiState.ReadOnly => "readonly",
        UiState.Busy => "busy",
        UiState.Hidden => "d-none",
        _ => string.Empty
    };

    /// <inheritdoc />
    public string MapPlacement(UiPlacement placement) => placement switch
    {
        UiPlacement.Top => "top",
        UiPlacement.TopStart => "top-start",
        UiPlacement.TopEnd => "top-end",
        UiPlacement.Right => "right",
        UiPlacement.Bottom => "bottom",
        UiPlacement.BottomStart => "bottom-start",
        UiPlacement.BottomEnd => "bottom-end",
        UiPlacement.Left => "left",
        UiPlacement.Center => "center",
        _ => "top"
    };

    /// <inheritdoc />
    public string MapLayout(UiLayout layout) => layout switch
    {
        UiLayout.Inline => "d-inline-flex",
        UiLayout.Stack => "d-flex flex-column",
        UiLayout.Grid => "row",
        UiLayout.Split => "d-flex justify-content-between",
        UiLayout.Fill => "w-100",
        _ => "d-flex"
    };

    /// <inheritdoc />
    public string MapPinned(UiPinned pinned) => pinned switch
    {
        UiPinned.None => string.Empty,
        UiPinned.Start => "sticky-top",
        UiPinned.End => "sticky-bottom",
        UiPinned.Both => "position-sticky",
        _ => string.Empty
    };
}
