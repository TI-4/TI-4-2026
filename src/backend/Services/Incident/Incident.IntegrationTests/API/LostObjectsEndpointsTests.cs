using Incident.Domain.Entities;
using Incident.IntegrationTests.Fixtures;
using Incident.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;

namespace Incident.IntegrationTests.API;

[Collection(MongoCollection.Name)]
public class LostObjectsEndpointsTests : ApiTestBase
{
    private const string Endpoint = "/api/incident/lost-objects";

    public LostObjectsEndpointsTests(MongoFixture fixture) : base(fixture) { }

    private ObjectRepository CreateRepository() => new(Factory.CreateContext());

    private static object NewLostObjectBody(
        Guid? structureId = null,
        string photoUrl = "https://campus.uct.cl/mochila.png",
        int status = 0) => new
        {
            title = "Mochila negra",
            description = "Dejada en la biblioteca",
            status,
            photoUrl,
            structureId = structureId ?? Guid.NewGuid()
        };

    private async Task<string> CreateLostObjectAsync(
        Guid? structureId = null,
        string photoUrl = "https://campus.uct.cl/mochila.png")
    {
        var response = await Http.PostAsync(Endpoint, JsonBody(NewLostObjectBody(structureId, photoUrl)));
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await ReadJsonAsync<CreatedResponse>(response);
        return created!.Id;
    }

    private sealed record CreatedResponse(string Id);

    [DockerFact]
    public async Task Post_CuandoElRequestEsValido_Retorna201ConElId()
    {
        var response = await Http.PostAsync(Endpoint, JsonBody(NewLostObjectBody()));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        Assert.Contains(Endpoint + "/", response.Headers.Location!.ToString());

        var created = await ReadJsonAsync<CreatedResponse>(response);
        Assert.False(string.IsNullOrEmpty(created!.Id));

        var persisted = await CreateRepository().GetByIdAsync(created.Id);
        Assert.NotNull(persisted);
        Assert.Equal("Mochila negra", persisted!.Title);
        Assert.Equal(Objectenum.Pending, persisted.Status);
    }

    [DockerFact]
    public async Task Post_CuandoLaPhotoUrlNoEsValida_Retorna400()
    {
        var response = await Http.PostAsync(Endpoint, JsonBody(NewLostObjectBody(photoUrl: "no-es-una-url")));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await ReadJsonAsync<ProblemDetails>(response);
        Assert.Equal("Invalid Photo URL format.", problem!.Title);
    }

    [DockerFact]
    public async Task Post_CuandoElStructureIdEsVacio_Retorna400()
    {
        var response = await Http.PostAsync(Endpoint, JsonBody(NewLostObjectBody(structureId: Guid.Empty)));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await ReadJsonAsync<ProblemDetails>(response);
        Assert.Equal("Structure ID is required.", problem!.Title);
    }

    [DockerFact]
    public async Task GetById_CuandoNoExiste_Retorna404()
    {
        var response = await Http.GetAsync($"{Endpoint}/64b000000000000000000000");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [DockerFact]
    public async Task GetById_CuandoExiste_Retorna204SinContenido()
    {
        // Comportamiento actual: el handler carga los datos pero el controller
        // responde 204 y descarta el body.
        var id = await CreateLostObjectAsync();

        var response = await Http.GetAsync($"{Endpoint}/{id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Empty(await response.Content.ReadAsStringAsync());
    }

    [DockerFact]
    public async Task GetStatus_CuandoHayObjetosConEseEstado_Retorna204()
    {
        var repository = CreateRepository();
        await repository.CreateAsync(new LostObject
        {
            Title = "Casco",
            Description = "Resuelto",
            Status = Objectenum.Resolved,
            StructureRefId = Guid.NewGuid()
        });
        await repository.CreateAsync(new LostObject
        {
            Title = "Paraguas",
            Description = "Pendiente",
            Status = Objectenum.Pending,
            StructureRefId = Guid.NewGuid()
        });

        var response = await Http.GetAsync($"{Endpoint}/status/{(int)Objectenum.Resolved}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [DockerFact]
    public async Task GetStatus_CuandoElStatusEsInvalido_Retorna400()
    {
        var response = await Http.GetAsync($"{Endpoint}/status/99");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [DockerFact]
    public async Task PatchStatus_CuandoElObjetoExiste_Retorna204YSePersisteElCambio()
    {
        var id = await CreateLostObjectAsync();

        var response = await Http.PatchAsync(
            $"{Endpoint}/{id}/lost/status",
            JsonBody(new { status = (int)Objectenum.Canceled }));

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var persisted = await CreateRepository().GetByIdAsync(id);
        Assert.Equal(Objectenum.Canceled, persisted!.Status);
    }

    [DockerFact]
    public async Task PatchStatus_CuandoElObjetoNoExiste_Retorna404()
    {
        var response = await Http.PatchAsync(
            $"{Endpoint}/64b000000000000000000000/lost/status",
            JsonBody(new { status = (int)Objectenum.Resolved }));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [DockerFact]
    public async Task PatchStatus_CuandoElStatusEsInvalido_Retorna400()
    {
        var id = await CreateLostObjectAsync();

        var response = await Http.PatchAsync(
            $"{Endpoint}/{id}/lost/status",
            JsonBody(new { status = 42 }));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
