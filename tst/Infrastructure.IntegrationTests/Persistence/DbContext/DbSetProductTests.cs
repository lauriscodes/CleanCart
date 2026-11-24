using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.IntegrationTests.Persistence.DbContext;

[Collection(nameof(SharedTestCollection))]
public class DbSetProductTests : IAsyncLifetime
{
    private readonly IEntityType _entityType;

    public DbSetProductTests(TestInitializer testInitializer)
    {
        using var dbContext = testInitializer.DbContextFactory.CreateDbContext();

        var entityKey = typeof(Product).FullName;
        Assert.NotNull(entityKey);

        _entityType = dbContext.GetService<IDesignTimeModel>().Model.FindEntityType(entityKey)!;
        Assert.NotNull(_entityType);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void PrimaryKey_IsConfigured()
    {
        var property = _entityType.FindProperty(nameof(Product.Id));
        AssertHelper.IsPrimaryKeyValid(property);
    }

    [Theory]
    [InlineData(nameof(Product.Name), 255)]
    [InlineData(nameof(Product.Price), null, null, false, false, "decimal(18,2)")]
    [InlineData(nameof(Product.StockLevel))]
    public void EntityProperties_AreConfigured(
        string propertyName,
        int? columnLength = null,
        int? columnOrder = null,
        bool isUniqueIndex = false,
        bool isNullable = false,
        string? columnType = null,
        ValueGenerated valueGenerated = ValueGenerated.Never
    )
    {
        var property = _entityType.FindProperty(propertyName);
        AssertHelper.IsPropertyValid(property, columnLength, columnOrder, isUniqueIndex, isNullable, valueGenerated,
            columnType: columnType);
    }
}