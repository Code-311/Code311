using Code311.Host.Services;
using Code311.Licensing.DependencyInjection;
using Code311.Licensing.Models;
using Code311.Licensing.Validation;
using Code311.Persistence.EFCore;
using Code311.Persistence.EFCore.DependencyInjection;
using Code311.Tabler.Dashboard.DependencyInjection;
using Code311.Tabler.Mvc.DependencyInjection;
using Code311.Tabler.Razor.DependencyInjection;
using Code311.Tabler.Widgets.Calendar.DependencyInjection;
using Code311.Tabler.Widgets.Charts.DependencyInjection;
using Code311.Tabler.Widgets.DataTables.DependencyInjection;
using Code311.Ui.Abstractions.Preferences;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Explicit framework package registration for demonstration clarity.
builder.Services.AddCode311TablerMvc();
builder.Services.AddCode311TablerRazor();
builder.Services.AddCode311TablerDashboard();
builder.Services.AddCode311TablerDataTablesWidgets();
builder.Services.AddCode311TablerCalendarWidgets();
builder.Services.AddCode311TablerChartWidgets();

// Host-local provider choice: SQLite for practical demo persistence.
var connectionString = builder.Configuration.GetConnectionString("Code311Demo") ?? "Data Source=App_Data/code311-host-demo.db";
Directory.CreateDirectory("App_Data");
builder.Services.AddCode311PersistenceEfCore(options => options.UseSqlite(connectionString));

builder.Services.AddCode311Licensing(options =>
{
    options.RequireValidLicenseAtStartup = true;
    options.ExpiryWarningWindowDays = 30;
});
builder.Services.AddCode311InMemoryLicenseSource(new Code311License
{
    LicenseId = "demo-license-001",
    CustomerName = "Code311 Demo Host",
    Plan = "demo",
    NotBeforeUtc = DateTimeOffset.UtcNow.AddDays(-1),
    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(90),
    Features = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "dashboard.basic",
        "dashboard.advanced",
        "widgets.datatables",
        "widgets.calendar",
        "widgets.charts"
    }
});

builder.Services.AddScoped<IDemoUserContext, DemoUserContext>();
builder.Services.AddScoped<IPreferenceOrchestrator, PreferenceOrchestrator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Code311PreferenceDbContext>();
    await db.Database.EnsureCreatedAsync();

    var startupValidator = scope.ServiceProvider.GetRequiredService<IStartupLicenseValidator>();
    try
    {
        await startupValidator.ValidateAtStartupAsync();
    }
    catch (InvalidOperationException ex)
    {
        throw new HostStartupLicenseException("Code311.Host startup license validation failed.", ex);
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

await app.RunAsync();

public partial class Program;
