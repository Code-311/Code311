namespace Code311.Ui.Abstractions.Widgets;

/// <summary>
/// Defines a widget metadata contract used by widget packages.
/// </summary>
/// <remarks>
/// Implementations describe widget identity, category, and option type information for discovery and rendering pipelines.
/// </remarks>
public interface IWidgetDefinition
{
    /// <summary>
    /// Gets the stable widget key.
    /// </summary>
    /// <remarks>
    /// Keys should be unique across all registered widgets in a host.
    /// </remarks>
    string Key { get; }

    /// <summary>
    /// Gets the display name.
    /// </summary>
    /// <remarks>
    /// Display names are intended for catalogs and dashboard editors.
    /// </remarks>
    string DisplayName { get; }

    /// <summary>
    /// Gets the widget category.
    /// </summary>
    /// <remarks>
    /// Categories help hosts group widgets in catalogs.
    /// </remarks>
    string Category { get; }
}

/// <summary>
/// Provides widget initialization behavior.
/// </summary>
/// <remarks>
/// Initialization should remain side-effect-aware and be idempotent where feasible.
/// </remarks>
public interface IWidgetInitializer
{
    /// <summary>
    /// Initializes a widget instance.
    /// </summary>
    /// <param name="context">The widget initialization context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when initialization has finished.</returns>
    /// <remarks>
    /// Implementations should avoid performing long blocking operations.
    /// </remarks>
    Task InitializeAsync(WidgetInitializationContext context, CancellationToken cancellationToken = default);
}

/// <summary>
/// Contributes assets required by a widget.
/// </summary>
/// <remarks>
/// Asset contributions are consumed by host adapters to build deterministic manifests.
/// </remarks>
public interface IWidgetAssetContributor
{
    /// <summary>
    /// Gets asset contributions for a widget.
    /// </summary>
    /// <param name="context">The asset contribution context.</param>
    /// <returns>A sequence of asset descriptors.</returns>
    /// <remarks>
    /// Returned assets should be stable and avoid per-request randomization.
    /// </remarks>
    IReadOnlyCollection<WidgetAssetDescriptor> Contribute(WidgetAssetContext context);
}

/// <summary>
/// Serializes widget options for transport and client bootstrapping.
/// </summary>
/// <remarks>
/// Serializers should be deterministic to support caching and diagnostics.
/// </remarks>
public interface IWidgetSerializer
{
    /// <summary>
    /// Serializes options to a string payload.
    /// </summary>
    /// <param name="options">The options object.</param>
    /// <returns>A serialized payload.</returns>
    /// <remarks>
    /// Payload format is implementation-defined but should be documented per widget package.
    /// </remarks>
    string Serialize(object options);
}

/// <summary>
/// Represents a widget initialization request context.
/// </summary>
/// <param name="WidgetKey">The widget key.</param>
/// <param name="TenantId">The tenant identifier.</param>
/// <param name="UserId">The user identifier.</param>
/// <remarks>
/// The context includes tenant and user identity for multi-tenant-safe initialization logic.
/// </remarks>
public sealed record WidgetInitializationContext(string WidgetKey, string TenantId, string UserId);

/// <summary>
/// Represents a widget asset contribution context.
/// </summary>
/// <param name="WidgetKey">The widget key.</param>
/// <remarks>
/// The context may be expanded later with environment information while keeping backward compatibility.
/// </remarks>
public sealed record WidgetAssetContext(string WidgetKey);

/// <summary>
/// Describes a widget asset reference.
/// </summary>
/// <param name="Path">The asset path.</param>
/// <param name="Type">The asset type.</param>
/// <param name="Order">The ordering index.</param>
/// <remarks>
/// Ordering values enable deterministic include order across contributors.
/// </remarks>
public sealed record WidgetAssetDescriptor(string Path, WidgetAssetType Type, int Order = 0);

/// <summary>
/// Identifies supported widget asset types.
/// </summary>
/// <remarks>
/// Types are interpreted by host integrations when building manifests.
/// </remarks>
public enum WidgetAssetType
{
    Script,
    Style,
    Module
}
