using AutoMapper;
using Domain.Entities;
using EntityAxis.KeyMappers;
using FluentAssertions;
using Infrastructure.Persistence.EntityFramework;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.IntegrationTests.Persistence.Repositories;

[Collection(nameof(SharedTestCollection))]
public class ProductCommandRepositoryTests(TestInitializer testInitializer) : IAsyncLifetime
{
    private readonly IDbContextFactory<CoreDbContext> _dbContextFactory = testInitializer.DbContextFactory;
    private readonly IMapper _mapper = testInitializer.Mapper;
    private readonly Func<Task> _resetDatabase = testInitializer.ResetDatabaseAsync;
    private ProductCommandRepository Sut => new(_dbContextFactory, _mapper, new IdentityKeyMapper<Guid>());

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _resetDatabase();
    }

    [Fact]
    public async Task Can_UpdateAsync()
    {
        var seeder = new DataSeeder(_dbContextFactory, _mapper);
        var existingProduct = await seeder.SeedProduct();
        existingProduct.UpdateStockLevel(existingProduct.StockLevel + 1000);
        var domainProduct = _mapper.Map<Product>(existingProduct);

        await Sut.UpdateAsync(domainProduct);

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var updatedProduct = await dbContext.Products.FirstAsync(p => p.Id == existingProduct.Id);
        updatedProduct.Should().BeEquivalentTo(existingProduct);
    }
}