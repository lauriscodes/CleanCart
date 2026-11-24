using AutoMapper;
using EntityAxis.KeyMappers;
using FluentAssertions;
using Infrastructure.Persistence.EntityFramework;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.IntegrationTests.Persistence.Repositories;

[Collection(nameof(SharedTestCollection))]
public class ShoppingCartQueryRepositoryTest(TestInitializer testInitializer) : IAsyncLifetime
{
    private readonly IDbContextFactory<CoreDbContext> _dbContextFactory = testInitializer.DbContextFactory;
    private readonly IMapper _mapper = testInitializer.Mapper;
    private readonly Func<Task> _resetDatabase = testInitializer.ResetDatabaseAsync;
    private ShoppingCartQueryRepository Sut => new(_dbContextFactory, _mapper, new IdentityKeyMapper<Guid>());

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _resetDatabase();
    }

    [Fact]
    public async Task Can_GetByUserIdAsync()
    {
        var seeder = new DataSeeder(_dbContextFactory, _mapper);
        var user = await seeder.SeedUser();
        var product = await seeder.SeedProduct();
        var expectedShoppingCart = await seeder.SeedShoppingCart(user, product);

        var returnedShoppingCart = await Sut.GetByUserIdAsync(user.Id);

        returnedShoppingCart.Should().BeEquivalentTo(expectedShoppingCart);
    }
}