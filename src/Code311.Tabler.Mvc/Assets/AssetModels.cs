namespace Code311.Tabler.Mvc.Assets;

/// <summary>
/// Represents a scoped asset descriptor registered during a request.
/// </summary>
/// <param name="Path">The asset path.</param>
/// <param name="IsScript">Indicates whether the asset is a script entry.</param>
/// <remarks>
/// Asset descriptors remain host-adapter concerns and are resolved at view rendering time.
/// </remarks>
public sealed record Code311ScopedAsset(string Path, bool IsScript);

/// <summary>
/// Stores request-scoped asset registrations.
/// </summary>
/// <remarks>
/// The store provides deterministic per-request asset handoff for MVC views.
/// </remarks>
public interface ICode311AssetRequestStore
{
    /// <summary>
    /// Registers a style asset path.
    /// </summary>
    /// <param name="path">The style path.</param>
    void AddStyle(string path);

    /// <summary>
    /// Registers a script asset path.
    /// </summary>
    /// <param name="path">The script path.</param>
    void AddScript(string path);

    /// <summary>
    /// Returns all registered assets.
    /// </summary>
    /// <returns>Read-only ordered assets.</returns>
    IReadOnlyList<Code311ScopedAsset> GetAll();
}

internal sealed class Code311AssetRequestStore : ICode311AssetRequestStore
{
    private readonly List<Code311ScopedAsset> _assets = [];

    public void AddStyle(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        _assets.Add(new Code311ScopedAsset(path, false));
    }

    public void AddScript(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        _assets.Add(new Code311ScopedAsset(path, true));
    }

    public IReadOnlyList<Code311ScopedAsset> GetAll() => _assets;
}
