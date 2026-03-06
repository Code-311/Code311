namespace Code311.Tabler.Widgets.Calendar.Options;

public sealed record CalendarWidgetOptions(string InitialView = "dayGridMonth", bool WeekendsVisible = true, bool Editable = false);

public sealed class CalendarWidgetOptionsBuilder
{
    private string _initialView = "dayGridMonth";
    private bool _weekendsVisible = true;
    private bool _editable;

    public CalendarWidgetOptionsBuilder WithInitialView(string view)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(view);
        _initialView = view;
        return this;
    }

    public CalendarWidgetOptionsBuilder ShowWeekends(bool visible = true)
    {
        _weekendsVisible = visible;
        return this;
    }

    public CalendarWidgetOptionsBuilder AllowEditing(bool editable = true)
    {
        _editable = editable;
        return this;
    }

    public CalendarWidgetOptions Build() => new(_initialView, _weekendsVisible, _editable);
}
