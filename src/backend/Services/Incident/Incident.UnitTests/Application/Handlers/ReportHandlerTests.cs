using ErrorOr;
using Incident.Application.DTOs;
using Incident.Application.Handlers;
using Incident.Domain.Entities;
using Incident.Domain.Interfaces;
using NSubstitute;

namespace Incident.UnitTests.Application.Handlers;

public class ReportHandlerTests
{
    private readonly ITicketRepository _repository;
    private readonly ReportHandler _handler;
    private readonly ILostObjectRepository _lostObjectRepository;

    public ReportHandlerTests()
    {
        _repository = Substitute.For<ITicketRepository>();
        _lostObjectRepository = Substitute.For<ILostObjectRepository>();
        _handler = new ReportHandler(_repository, _lostObjectRepository);
    }

    private static CreateReportRequest ValidRequest(
        Guid? userRefId = null,
        Guid? structureRefId = null,
        int ticketType = (int)Tickets.Pending) =>
        new(
            UserRefId: userRefId ?? Guid.NewGuid(),
            StructureRefId: structureRefId ?? Guid.NewGuid(),
            TicketType: ticketType,
            IsActive: true,
            ReportedAt: new DateTime(2026, 10, 5, 10, 30, 0, DateTimeKind.Utc));

    // ---------- CreateReportAsync ----------

    [Fact]
    public async Task CreateReportAsync_ConUserRefIdVacio_RetornaValidation()
    {
        var result = await _handler.CreateReportAsync(ValidRequest(userRefId: Guid.Empty));

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.Errors[0].Type);
        Assert.Equal("Report.UserRefId", result.Errors[0].Code);
        await _repository.DidNotReceiveWithAnyArgs().CreateAsync(default!);
    }

    [Fact]
    public async Task CreateReportAsync_ConStructureRefIdVacio_RetornaValidation()
    {
        var result = await _handler.CreateReportAsync(ValidRequest(structureRefId: Guid.Empty));

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.Errors[0].Type);
        Assert.Equal("Report.StructureRefId", result.Errors[0].Code);
        await _repository.DidNotReceiveWithAnyArgs().CreateAsync(default!);
    }

    [Fact]
    public async Task CreateReportAsync_ConRequestValido_MapeaElEntityYLoPersiste()
    {
        var userRefId = Guid.NewGuid();
        var structureRefId = Guid.NewGuid();
        var reportedAt = new DateTime(2026, 10, 5, 10, 30, 0, DateTimeKind.Utc);

        var result = await _handler.CreateReportAsync(new CreateReportRequest(
            UserRefId: userRefId,
            StructureRefId: structureRefId,
            TicketType: (int)Tickets.In_Process,
            IsActive: false,
            ReportedAt: reportedAt));

        Assert.False(result.IsError);
        await _repository.Received(1).CreateAsync(Arg.Is<Ticket>(t =>
            t.UserRefId == userRefId &&
            t.StructureRefId == structureRefId &&
            t.TicketType == Tickets.In_Process &&
            t.IsActive == false &&
            t.ReportedAt == reportedAt));
    }

    [Fact]
    public async Task CreateReportAsync_CuandoElRepositorioNoSeteaElId_RetornaCadenaVacia()
    {
        var result = await _handler.CreateReportAsync(ValidRequest());

        Assert.False(result.IsError);
        Assert.Equal(string.Empty, result.Value);
    }

    // ---------- GetByIdAsync ----------

    [Fact]
    public async Task GetByIdAsync_CuandoNoExiste_RetornaNotFound()
    {
        _repository.GetByIdAsync("64b000000000000000000000")
            .Returns(Task.FromResult<Ticket?>(null));

        var result = await _handler.GetByIdAsync("64b000000000000000000000");

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.NotFound, result.Errors[0].Type);
        Assert.Equal("Report.NotFound", result.Errors[0].Code);
    }

    [Fact]
    public async Task GetByIdAsync_CuandoExiste_MapeaTodosLosCampos()
    {
        var userRefId = Guid.NewGuid();
        var structureRefId = Guid.NewGuid();
        var reportedAt = new DateTime(2026, 10, 5, 10, 30, 0, DateTimeKind.Utc);
        var complaint = new ComplaintDetails
        {
            Title = "Reclamo",
            Description = "Objeto no devuelto",
            Status = Complainenum.Claim
        };

        var entity = new Ticket
        {
            Id = "64b000000000000000000001",
            UserRefId = userRefId,
            StructureRefId = structureRefId,
            TicketType = Tickets.Resolved,
            IsActive = true,
            ReportedAt = reportedAt,
            ComplaintDetails = complaint,
            LostObjectId = "64b000000000000000000002"
        };
        _repository.GetByIdAsync(entity.Id).Returns(Task.FromResult<Ticket?>(entity));

        var result = await _handler.GetByIdAsync(entity.Id!);

        Assert.False(result.IsError);
        Assert.Equal(new TicketResponse(
            entity.Id!,
            userRefId,
            structureRefId,
            Tickets.Resolved,
            true,
            reportedAt,
            complaint,
            "64b000000000000000000002"), result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_CuandoNoTieneDetalles_OpcionalesSonNulos()
    {
        var entity = new Ticket
        {
            Id = "64b000000000000000000003",
            UserRefId = Guid.NewGuid(),
            StructureRefId = Guid.NewGuid(),
            TicketType = Tickets.Pending,
            IsActive = true,
            ReportedAt = DateTime.UtcNow
        };
        _repository.GetByIdAsync(entity.Id).Returns(Task.FromResult<Ticket?>(entity));

        var result = await _handler.GetByIdAsync(entity.Id!);

        Assert.False(result.IsError);
        Assert.Null(result.Value.ComplaintDetails);
        Assert.Null(result.Value.LostObjectId);
    }

    // ---------- UpdateStatusTicketAsync ----------

    [Fact]
    public async Task UpdateStatusTicketAsync_ConStatusFueraDelEnum_RetornaValidation()
    {
        var result = await _handler.UpdateStatusTicketAsync("64b000000000000000000000", new UpdateStatusReport(99));

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.Errors[0].Type);
        Assert.Equal("Ticket.Validation", result.Errors[0].Code);
    }

    [Fact]
    public async Task UpdateStatusTicketAsync_CuandoNoExiste_RetornaNotFound()
    {
        _repository.GetByIdAsync("64b000000000000000000000")
            .Returns(Task.FromResult<Ticket?>(null));

        var result = await _handler.UpdateStatusTicketAsync("64b000000000000000000000", new UpdateStatusReport((int)Tickets.Resolved));

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.NotFound, result.Errors[0].Type);
        Assert.Equal("Ticket.NotFound", result.Errors[0].Code);
    }

    [Fact]
    public async Task UpdateStatusTicketAsync_CuandoElRepositorioFalla_RetornaFailure()
    {
        var entity = new Ticket
        {
            Id = "64b000000000000000000000",
            UserRefId = Guid.NewGuid(),
            StructureRefId = Guid.NewGuid(),
            TicketType = Tickets.Pending,
            IsActive = true,
            ReportedAt = DateTime.UtcNow
        };
        _repository.GetByIdAsync(entity.Id).Returns(Task.FromResult<Ticket?>(entity));
        _repository.UpdateStatusAsync(entity, (int)Tickets.Resolved).Returns(Task.FromResult(false));

        var result = await _handler.UpdateStatusTicketAsync(entity.Id!, new UpdateStatusReport((int)Tickets.Resolved));

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Failure, result.Errors[0].Type);
        Assert.Equal("Ticket.UpdateFailed", result.Errors[0].Code);
    }

    [Fact]
    public async Task UpdateStatusTicketAsync_CuandoExiste_RetornaSuccess()
    {
        var entity = new Ticket
        {
            Id = "64b000000000000000000000",
            UserRefId = Guid.NewGuid(),
            StructureRefId = Guid.NewGuid(),
            TicketType = Tickets.Pending,
            IsActive = true,
            ReportedAt = DateTime.UtcNow
        };
        _repository.GetByIdAsync(entity.Id).Returns(Task.FromResult<Ticket?>(entity));
        _repository.UpdateStatusAsync(entity, (int)Tickets.Dismissed).Returns(Task.FromResult(true));

        var result = await _handler.UpdateStatusTicketAsync(entity.Id!, new UpdateStatusReport((int)Tickets.Dismissed));

        Assert.False(result.IsError);
        Assert.Equal(Result.Success, result.Value);
        await _repository.Received(1).UpdateStatusAsync(entity, (int)Tickets.Dismissed);
    }
}
