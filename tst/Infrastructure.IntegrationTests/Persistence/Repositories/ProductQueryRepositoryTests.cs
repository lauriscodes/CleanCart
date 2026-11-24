using AutoMapper;
using EntityAxis.KeyMappers;
using FluentAssertions;
using Infrastructure.Persistence.EntityFramework;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.IntegrationTests.Persistence.Repositories;

[Collection(nameof(SharedTestCollection))]
public class ProductQueryRepositoryTests(TestInitializer testInitializer) : IAsyncLifetime
{
    private readonly IDbContextFactory<CoreDbContext> _dbContextFactory = testInitializer.DbContextFactory;
    private readonly IMapper _mapper = testInitializer.Mapper;
    private readonly Func<Task> _resetDatabase = testInitializer.ResetDatabaseAsync;
    private ProductQueryRepository Sut => new(_dbContextFactory, _mapper, new IdentityKeyMapper<Guid>());

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _resetDatabase();
    }

    [Fact]
    public async Task Can_GetProductByIdAsync()
    {
        var seeder = new DataSeeder(_dbContextFactory, _mapper);
        var existingProduct = await seeder.SeedProduct();

        var product = await Sut.GetByIdAsync(existingProduct.Id);

        product.Should().BeEquivalentTo(existingProduct);
    }

    [Fact]
    public async Task Can_GetAllProductsAsync()
    {
        var seeder = new DataSeeder(_dbContextFactory, _mapper);
        var existingProducts = await Task.WhenAll(Enumerable.Range(0, 5).Select(_ => seeder.SeedProduct()));

        var products = await Sut.GetAllAsync();

        products.Should().BeEquivalentTo(existingProducts);
    }
}