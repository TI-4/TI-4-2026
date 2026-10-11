using Incident.Domain.Entities;
using Incident.IntegrationTests.Fixtures;
using Incident.Infrastructure.Persistence.Repositories;
using MongoDB.Bson;

namespace Incident.IntegrationTests.Persistence;

[Collection(MongoCollection.Name)]
public class ObjectRepositoryTests : MongoTestBase
{
    public ObjectRepositoryTests(MongoFixture fixture) : base(fixture) { }

    private ObjectRepository CreateRepository() => new(CreateContext());

    private static LostObject NewLostObject(
        Objectenum status = Objectenum.Pending,
        string title = "Mochila negra") => new()
        {
            Title = title,
            Description = "Dejada en la biblioteca",
            Status = status,
            PhotoUrl = "https://campus.uct.cl/mochila.png",
            StructureRefId = Guid.NewGuid()
        };

    [DockerFact]
    public async Task CreateAsync_LuegoDeInsertar_AsignaElIdYSePuedeLeer()
    {
        var repository = CreateRepository();
        var lostObject = NewLostObject(Objectenum.In_Process);

        await repository.CreateAsync(lostObject);

        Assert.False(string.IsNullOrEmpty(lostObject.Id));

        var persisted = await repository.GetByIdAsync(lostObject.Id!);
        Assert.NotNull(persisted);
        Assert.Equal("Mochila negra", persisted.Title);
        Assert.Equal("Dejada en la biblioteca", persisted.Description);
        Assert.Equal(Objectenum.In_Process, persisted.Status);
        Assert.Equal("https://campus.uct.cl/mochila.png", persisted.PhotoUrl);
        Assert.Equal(lostObject.StructureRefId, persisted.StructureRefId);
    }

    [DockerFact]
    public async Task GetByIdAsync_ConIdNoObjectId_RetornaNull()
    {
        var repository = CreateRepository();

        var result = await repository.GetByIdAsync("id-invalido");

        Assert.Null(result);
    }

    [DockerFact]
    public async Task FilterStatusAsync_DevuelveSoloLosQueCoincidenConElEstado()
    {
        var repository = CreateRepository();
        await repository.CreateAsync(NewLostObject(Objectenum.Pending));
        await repository.CreateAsync(NewLostObject(Objectenum.Pending, "Paraguas"));
        await repository.CreateAsync(NewLostObject(Objectenum.Resolved, "Casco"));

        var resolved = await repository.FilterStatusAsync(nameof(Objectenum.Resolved));

        Assert.Single(resolved);
        Assert.Equal("Casco", resolved[0].Title);
        Assert.Equal(Objectenum.Resolved, resolved[0].Status);
    }

    [DockerFact]
    public async Task FilterStatusAsync_CuandoNingunoCoincide_RetornaListaVacia()
    {
        var repository = CreateRepository();
        await repository.CreateAsync(NewLostObject(Objectenum.Pending));

        var canceled = await repository.FilterStatusAsync(nameof(Objectenum.Canceled));

        Assert.Empty(canceled);
    }

    [DockerFact]
    public async Task UpdateStatusAsync_CuandoElEstadoCambia_RetornaTrueYSePersiste()
    {
        var repository = CreateRepository();
        var lostObject = NewLostObject(Objectenum.Pending);
        await repository.CreateAsync(lostObject);

        var updated = await repository.UpdateStatusAsync(lostObject, (int)Objectenum.Canceled);

        Assert.True(updated);
        var persisted = await repository.GetByIdAsync(lostObject.Id!);
        Assert.Equal(Objectenum.Canceled, persisted!.Status);
    }

    [DockerFact]
    public async Task UpdateStatusAsync_CuandoElEstadoNoCambia_RetornaFalse()
    {
        // Comportamiento actual: si el documento de reemplazo es idéntico,
        // ModifiedCount es 0 y el método informa fallo.
        var repository = CreateRepository();
        var lostObject = NewLostObject(Objectenum.Pending);
        await repository.CreateAsync(lostObject);

        var updated = await repository.UpdateStatusAsync(lostObject, (int)Objectenum.Pending);

        Assert.False(updated);
    }

    [DockerFact]
    public async Task DeleteAsync_SiElDocumentoNoExiste_RetornaFalse()
    {
        var repository = CreateRepository();

        var deleted = await repository.DeleteAsync(ObjectId.GenerateNewId().ToString());

        Assert.False(deleted);
    }
}
