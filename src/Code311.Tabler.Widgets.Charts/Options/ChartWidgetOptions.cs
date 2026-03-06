namespace Code311.Tabler.Widgets.Charts.Options;

public sealed record ChartWidgetOptions(string ChartType = "line", bool LegendVisible = true, bool Responsive = true);

public sealed class ChartWidgetOptionsBuilder
{
    private string _chartType = "line";
    private bool _legendVisible = true;
    private bool _responsive = true;

    public ChartWidgetOptionsBuilder WithType(string chartType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(chartType);
        _chartType = chartType;
        return this;
    }

    public ChartWidgetOptionsBuilder ShowLegend(bool visible = true)
    {
        _legendVisible = visible;
        return this;
    }

    public ChartWidgetOptionsBuilder UseResponsiveLayout(bool enabled = true)
    {
        _responsive = enabled;
        return this;
    }

    public ChartWidgetOptions Build() => new(_chartType, _legendVisible, _responsive);
}
