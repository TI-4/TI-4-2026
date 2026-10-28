using System.Threading.Tasks;
using Campus.Infrastructure.Persistence;
using Campus.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Campus.IntegrationTests.Persistence;

[Collection(PostgreSqlCollection.Name)]
public class CampusDbContextSeedTests : IAsyncLifetime
{
    private readonly PostgreSqlFixture _fixture;
    private CampusDbContext _dbContext = null!;

    public CampusDbContextSeedTests(PostgreSqlFixture fixture) => _fixture = fixture;

    public async Task InitializeAsync()
    {
        if (_fixture.IsAvailable)
        {
            var options = new DbContextOptionsBuilder<CampusDbContext>()
                .UseNpgsql(_fixture.ConnectionString)
                .Options;

            _dbContext = new CampusDbContext(options);
            await _dbContext.Database.MigrateAsync();
        }
    }

    public async Task DisposeAsync()
    {
        if (_dbContext is not null) await _dbContext.DisposeAsync();
    }

    [DockerFact]
    public async Task SeedAsync_WhenDatabaseIsEmptyAndThenPopulated_IsIdempotent()
    {
        _dbContext.Rooms.RemoveRange(_dbContext.Rooms);
        _dbContext.Structures.RemoveRange(_dbContext.Structures);
        _dbContext.Buildings.RemoveRange(_dbContext.Buildings);
        _dbContext.Categories.RemoveRange(_dbContext.Categories);
        _dbContext.Campuses.RemoveRange(_dbContext.Campuses);
        await _dbContext.SaveChangesAsync();

        await CampusDbContextSeed.SeedAsync(_dbContext);

        var campusesCount = await _dbContext.Campuses.CountAsync();
        var buildingsCount = await _dbContext.Buildings.CountAsync();
        var categoriesCount = await _dbContext.Categories.CountAsync();
        var roomsCount = await _dbContext.Rooms.CountAsync();
        var structuresCount = await _dbContext.Structures.CountAsync();

        Assert.Equal(2, campusesCount);
        Assert.Equal(4, buildingsCount);
        Assert.Equal(3, categoriesCount);
        Assert.Equal(28, roomsCount);
        Assert.Equal(2, structuresCount);

        await CampusDbContextSeed.SeedAsync(_dbContext);

        Assert.Equal(2, await _dbContext.Campuses.CountAsync());
    }
}
