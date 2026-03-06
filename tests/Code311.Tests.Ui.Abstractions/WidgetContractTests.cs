using Xunit;
using Code311.Ui.Abstractions.Widgets;

namespace Code311.Tests.Ui.Abstractions;

/// <summary>
/// Tests widget abstraction contracts.
/// </summary>
/// <remarks>
/// The tests focus on DTO/record and enum contract behavior only.
/// </remarks>
public sealed class WidgetContractTests
{
    /// <summary>
    /// Verifies widget asset descriptor preserves provided values.
    /// </summary>
    /// <remarks>
    /// Deterministic descriptor behavior is required by asset manifest composition.
    /// </remarks>
    [Fact]
    public void WidgetAssetDescriptor_ShouldStoreInputValues()
    {
        var descriptor = new WidgetAssetDescriptor("/assets/widget.js", WidgetAssetType.Script, 20);

        Assert.Equal("/assets/widget.js", descriptor.Path);
        Assert.Equal(WidgetAssetType.Script, descriptor.Type);
        Assert.Equal(20, descriptor.Order);
    }

    /// <summary>
    /// Verifies initialization context stores tenant and user scope.
    /// </summary>
    /// <remarks>
    /// Tenant and user values are essential for multi-tenant-safe widget initialization.
    /// </remarks>
    [Fact]
    public void WidgetInitializationContext_ShouldStoreScopeValues()
    {
        var context = new WidgetInitializationContext("charts.sales", "tenant-a", "user-1");

        Assert.Equal("charts.sales", context.WidgetKey);
        Assert.Equal("tenant-a", context.TenantId);
        Assert.Equal("user-1", context.UserId);
    }
}
