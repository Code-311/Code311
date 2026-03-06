namespace Code311.Tabler.Widgets.DataTables.Options;

public sealed record DataTableWidgetOptions(
    int PageLength = 25,
    bool SearchEnabled = true,
    bool OrderingEnabled = true,
    string? DefaultSortColumn = null,
    string DefaultSortDirection = "asc");

public sealed class DataTableWidgetOptionsBuilder
{
    private int _pageLength = 25;
    private bool _searchEnabled = true;
    private bool _orderingEnabled = true;
    private string? _defaultSortColumn;
    private string _defaultSortDirection = "asc";

    public DataTableWidgetOptionsBuilder WithPageLength(int pageLength)
    {
        if (pageLength <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageLength));
        }

        _pageLength = pageLength;
        return this;
    }

    public DataTableWidgetOptionsBuilder EnableSearch(bool enabled = true)
    {
        _searchEnabled = enabled;
        return this;
    }

    public DataTableWidgetOptionsBuilder EnableOrdering(bool enabled = true)
    {
        _orderingEnabled = enabled;
        return this;
    }

    public DataTableWidgetOptionsBuilder WithDefaultSort(string column, string direction = "asc")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(column);
        _defaultSortColumn = column;
        _defaultSortDirection = direction.Equals("desc", StringComparison.OrdinalIgnoreCase) ? "desc" : "asc";
        return this;
    }

    public DataTableWidgetOptions Build()
        => new(_pageLength, _searchEnabled, _orderingEnabled, _defaultSortColumn, _defaultSortDirection);
}
