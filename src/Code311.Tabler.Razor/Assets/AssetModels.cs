namespace Code311.Tabler.Razor.Assets;

/// <summary>
/// Represents a scoped asset entry for Razor request rendering.
/// </summary>
/// <param name="Path">The asset path.</param>
/// <param name="IsScript">True when the asset is a script.</param>
public sealed record Code311ScopedAsset(string Path, bool IsScript);

/// <summary>
/// Stores request-scoped assets for Razor Pages.
/// </summary>
public interface ICode311AssetRequestStore
{
    void AddStyle(string path);
    void AddScript(string path);
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
