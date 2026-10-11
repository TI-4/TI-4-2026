using Incident.Application.DTOs;
using Incident.Domain.Entities;
using Incident.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;

namespace Incident.IntegrationTests.API;

[Collection(MongoCollection.Name)]
public class ReportsEndpointsTests : ApiTestBase
{
    private const string Endpoint = "/api/incident/reports";

    public ReportsEndpointsTests(MongoFixture fixture) : base(fixture) { }

    private sealed record CreatedResponse(string Id);

    private static object NewReportBody(
        Guid? userRefId = null,
        int ticketType = (int)Tickets.Pending) => new
        {
            userRefId = userRefId ?? Guid.NewGuid(),
            structureRefId = Guid.NewGuid(),
            ticketType,
            isActive = true,
            reportedAt = new DateTime(2026, 10, 5, 10, 30, 0, DateTimeKind.Utc)
        };

    private async Task<string> CreateReportAsync(Guid? userRefId = null)
    {
        var response = await Http.PostAsync(Endpoint, JsonBody(NewReportBody(userRefId)));
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await ReadJsonAsync<CreatedResponse>(response);
        return created!.Id;
    }

    [DockerFact]
    public async Task Post_CuandoElRequestEsValido_Retorna201YElGetDevuelveElTicket()
    {
        var userRefId = Guid.NewGuid();
        var structureRefId = Guid.NewGuid();

        var response = await Http.PostAsync(Endpoint, JsonBody(new
        {
            userRefId,
            structureRefId,
            ticketType = (int)Tickets.In_Process,
            isActive = false,
            reportedAt = new DateTime(2026, 10, 5, 10, 30, 0, DateTimeKind.Utc)
        }));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        var created = await ReadJsonAsync<CreatedResponse>(response);

        var get = await Http.GetAsync($"{Endpoint}/{created!.Id}");
        Assert.Equal(HttpStatusCode.OK, get.StatusCode);

        var ticket = await ReadJsonAsync<TicketResponse>(get);
        Assert.NotNull(ticket);
        Assert.Equal(created.Id, ticket.Id);
        Assert.Equal(userRefId, ticket.UserRefId);
        Assert.Equal(structureRefId, ticket.StructureRefId);
        Assert.Equal(Tickets.In_Process, ticket.TicketType);
        Assert.False(ticket.IsActive);
        Assert.Equal(new DateTime(2026, 10, 5, 10, 30, 0, DateTimeKind.Utc), ticket.ReportedAt);
        Assert.Null(ticket.ComplaintDetails);
        Assert.Null(ticket.LostObjectId);
    }

    [DockerFact]
    public async Task Post_CuandoElUserRefIdEsVacio_Retorna400()
    {
        var response = await Http.PostAsync(Endpoint, JsonBody(NewReportBody(userRefId: Guid.Empty)));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await ReadJsonAsync<ProblemDetails>(response);
        Assert.Equal("User ID is required.", problem!.Title);
    }

    [DockerFact]
    public async Task GetById_CuandoNoExiste_Retorna404()
    {
        var response = await Http.GetAsync($"{Endpoint}/64b000000000000000000000");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [DockerFact]
    public async Task PatchStatus_CuandoElTicketExiste_Retorna204YElGetReflejaElNuevoEstado()
    {
        var id = await CreateReportAsync();

        var response = await Http.PatchAsync(
            $"{Endpoint}/{id}/report/status",
            JsonBody(new { status = (int)Tickets.Resolved }));

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var get = await Http.GetAsync($"{Endpoint}/{id}");
        var ticket = await ReadJsonAsync<TicketResponse>(get);
        Assert.Equal(Tickets.Resolved, ticket!.TicketType);
    }

    [DockerFact]
    public async Task PatchStatus_CuandoElTicketNoExiste_Retorna404()
    {
        var response = await Http.PatchAsync(
            $"{Endpoint}/64b000000000000000000000/report/status",
            JsonBody(new { status = (int)Tickets.Resolved }));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [DockerFact]
    public async Task PatchStatus_CuandoElStatusEsInvalido_Retorna400()
    {
        var id = await CreateReportAsync();

        var response = await Http.PatchAsync(
            $"{Endpoint}/{id}/report/status",
            JsonBody(new { status = 42 }));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
