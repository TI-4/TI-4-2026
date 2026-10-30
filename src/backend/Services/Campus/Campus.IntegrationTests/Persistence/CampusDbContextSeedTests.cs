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
        _dbContext.CategoryGroups.RemoveRange(_dbContext.CategoryGroups);
        _dbContext.Campuses.RemoveRange(_dbContext.Campuses);
        await _dbContext.SaveChangesAsync();

        await CampusDbContextSeed.SeedAsync(_dbContext);

        var campuses = await _dbContext.Campuses.ToListAsync();
        var buildings = await _dbContext.Buildings.ToListAsync();
        var categoryGroups = await _dbContext.CategoryGroups.ToListAsync();
        var categories = await _dbContext.Categories.ToListAsync();
        var rooms = await _dbContext.Rooms.ToListAsync();
        var structures = await _dbContext.Structures.ToListAsync();

        Assert.Equal(2, campuses.Count);
        Assert.Equal(4, buildings.Count);
        Assert.Equal(2, categoryGroups.Count);
        Assert.Equal(3, categories.Count);
        Assert.Equal(28, rooms.Count);
        Assert.Equal(2, structures.Count);
        
        Assert.All(campuses, c =>
        {
            Assert.False(string.IsNullOrWhiteSpace(c.Name));
            Assert.True(c.Zoom > 0);
            Assert.True(c.Coordinates.Latitude != 0);
            Assert.True(c.Coordinates.Longitude != 0);
        });

        Assert.All(buildings, b =>
        {
            Assert.Equal(3, b.FloorsCount);
            Assert.NotNull(b.OpenTime);
            Assert.NotNull(b.CloseTime);
            Assert.False(string.IsNullOrWhiteSpace(b.Services));
        });

        Assert.All(rooms, r =>
        {
            Assert.True(r.Name.Length <= 20);
            Assert.True(r.Floor >= 1 && r.Floor <= 3);
            Assert.NotEqual(Guid.Empty, r.BuildingId);
            Assert.NotEqual(Guid.Empty, r.CategoryId);
        });

        await CampusDbContextSeed.SeedAsync(_dbContext);

        Assert.Equal(2, await _dbContext.Campuses.CountAsync());
        Assert.Equal(4, await _dbContext.Buildings.CountAsync());
        Assert.Equal(2, await _dbContext.CategoryGroups.CountAsync());
        Assert.Equal(3, await _dbContext.Categories.CountAsync());
        Assert.Equal(28, await _dbContext.Rooms.CountAsync());
        Assert.Equal(2, await _dbContext.Structures.CountAsync());
    }
}
