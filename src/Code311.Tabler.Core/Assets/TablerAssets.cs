namespace Code311.Tabler.Core.Assets;

/// <summary>
/// Identifies Tabler asset types.
/// </summary>
/// <remarks>
/// Asset type values are consumed by host adapters for manifest generation.
/// </remarks>
public enum TablerAssetType
{
    Script,
    Style
}

/// <summary>
/// Represents a Tabler asset manifest descriptor.
/// </summary>
/// <param name="Path">The asset path.</param>
/// <param name="Type">The asset type.</param>
/// <param name="Order">The deterministic load order.</param>
/// <remarks>
/// The descriptor remains neutral to host transport mechanisms.
/// </remarks>
public sealed record TablerAssetDescriptor(string Path, TablerAssetType Type, int Order = 0);

/// <summary>
/// Provides Tabler asset manifest entries.
/// </summary>
/// <remarks>
/// Providers can be extended by additional Tabler packages in later phases.
/// </remarks>
public interface ITablerAssetManifestProvider
{
    /// <summary>
    /// Returns ordered asset descriptors for Tabler core.
    /// </summary>
    /// <returns>A read-only list of ordered descriptors.</returns>
    /// <remarks>
    /// Consumers should preserve list order when constructing runtime manifests.
    /// </remarks>
    IReadOnlyList<TablerAssetDescriptor> GetAssets();
}

/// <summary>
/// Default Tabler core asset manifest provider.
/// </summary>
/// <remarks>
/// This provider includes only foundational Tabler assets and can be augmented by higher packages.
/// </remarks>
public sealed class DefaultTablerAssetManifestProvider : ITablerAssetManifestProvider
{
    private static readonly IReadOnlyList<TablerAssetDescriptor> Assets =
    [
        new("_content/Code311.Tabler.Core/css/tabler.min.css", TablerAssetType.Style, 10),
        new("_content/Code311.Tabler.Core/js/tabler.min.js", TablerAssetType.Script, 20)
    ];

    /// <inheritdoc />
    public IReadOnlyList<TablerAssetDescriptor> GetAssets() => Assets;
}
