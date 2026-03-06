using Code311.Persistence.EFCore.Entities;
using Code311.Persistence.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Code311.Tests.Persistence.EFCore;

public sealed class ModelBuilderExtensionsTests
{
    [Fact]
    public void ApplyCode311PreferenceStorage_ShouldConfigureCompositeKey()
    {
        var options = new DbContextOptionsBuilder<TestPreferenceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;

        using var context = new TestPreferenceDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(UserUiPreferenceEntity));

        Assert.NotNull(entityType);
        Assert.Equal(new[] { "TenantId", "UserId" }, entityType!.FindPrimaryKey()!.Properties.Select(x => x.Name));
    }

    private sealed class TestPreferenceDbContext(DbContextOptions<TestPreferenceDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyCode311PreferenceStorage();
            base.OnModelCreating(modelBuilder);
        }
    }
}
