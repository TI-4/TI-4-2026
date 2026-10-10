using ErrorOr;
using Incident.API.Controllers;
using Incident.Application.DTOs;
using Incident.Application.Handlers;
using Incident.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Incident.UnitTests.API.Controllers;

public class ReportsControllerTests
{
    private readonly IReportHandler _handler;
    private readonly ReportsController _controller;

    public ReportsControllerTests()
    {
        _handler = Substitute.For<IReportHandler>();
        _controller = new ReportsController(_handler);
    }

    private static CreateReportRequest ValidRequest() =>
        new(Guid.NewGuid(), Guid.NewGuid(), (int)Tickets.Pending, true, DateTime.UtcNow);

    private static TicketResponse AnyTicket() =>
        new("64b00000000000000000000b", Guid.NewGuid(), Guid.NewGuid(),
            Tickets.Pending, true, DateTime.UtcNow);

    // ---------- POST ----------

    [Fact]
    public async Task Create_CuandoElHandlerTieneExito_Retorna201ConLocation()
    {
        const string id = "64b00000000000000000000b";
        _handler.CreateReportAsync(Arg.Any<CreateReportRequest>())
            .Returns(Task.FromResult<ErrorOr<string>>(id));

        var result = await _controller.Create(ValidRequest());

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, created.StatusCode);
        Assert.Equal(nameof(ReportsController.GetById), created.ActionName);
        Assert.Equal(id, created.RouteValues?["id"]);
    }

    [Fact]
    public async Task Create_CuandoElHandlerValida_Retorna400ConProblemDetails()
    {
        _handler.CreateReportAsync(Arg.Any<CreateReportRequest>())
            .Returns(Task.FromResult<ErrorOr<string>>(
                Error.Validation("Report.UserRefId", "User ID is required.")));

        var result = await _controller.Create(ValidRequest());

        var problem = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, problem.StatusCode);
        Assert.Equal("User ID is required.", Assert.IsType<ProblemDetails>(problem.Value).Title);
    }

    // ---------- GET by id ----------

    [Fact]
    public async Task GetById_CuandoExiste_Retorna200ConElTicket()
    {
        var ticket = AnyTicket();
        _handler.GetByIdAsync(ticket.Id).Returns(Task.FromResult<ErrorOr<TicketResponse>>(ticket));

        var result = await _controller.GetById(ticket.Id);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.Equal(ticket, ok.Value);
    }

    [Fact]
    public async Task GetById_CuandoNoExiste_Retorna404()
    {
        _handler.GetByIdAsync("64b00000000000000000000b")
            .Returns(Task.FromResult<ErrorOr<TicketResponse>>(
                Error.NotFound("Report.NotFound", "Report not found")));

        var result = await _controller.GetById("64b00000000000000000000b");

        var problem = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, problem.StatusCode);
        Assert.Equal("Report not found", Assert.IsType<ProblemDetails>(problem.Value).Title);
    }

    [Fact]
    public async Task GetById_CuandoHayErrorDeValidacion_Retorna400()
    {
        _handler.GetByIdAsync("64b00000000000000000000b")
            .Returns(Task.FromResult<ErrorOr<TicketResponse>>(
                Error.Validation("Report.Validation", "Invalid")));

        var result = await _controller.GetById("64b00000000000000000000b");

        var problem = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, problem.StatusCode);
    }

    // ---------- PATCH status ----------

    [Fact]
    public async Task UpdateStatus_CuandoTieneExito_Retorna204()
    {
        _handler.UpdateStatusTicketAsync("64b00000000000000000000b", Arg.Any<UpdateStatusReport>())
            .Returns(Task.FromResult<ErrorOr<Success>>(Result.Success));

        var result = await _controller.UpdateStatus(
            "64b00000000000000000000b", new UpdateStatusReport((int)Tickets.Resolved));

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateStatus_CuandoNoExiste_Retorna404()
    {
        _handler.UpdateStatusTicketAsync("64b00000000000000000000b", Arg.Any<UpdateStatusReport>())
            .Returns(Task.FromResult<ErrorOr<Success>>(
                Error.NotFound("Ticket.NotFound", "not found")));

        var result = await _controller.UpdateStatus(
            "64b00000000000000000000b", new UpdateStatusReport((int)Tickets.Resolved));

        var problem = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, problem.StatusCode);
    }

    [Fact]
    public async Task UpdateStatus_CuandoElStatusEsInvalido_Retorna400()
    {
        _handler.UpdateStatusTicketAsync("64b00000000000000000000b", Arg.Any<UpdateStatusReport>())
            .Returns(Task.FromResult<ErrorOr<Success>>(
                Error.Validation("Ticket.Validation", "Invalid status")));

        var result = await _controller.UpdateStatus(
            "64b00000000000000000000b", new UpdateStatusReport(42));

        var problem = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, problem.StatusCode);
    }
}
