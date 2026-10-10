using ErrorOr;
using Incident.Application.DTOs;
using Incident.Application.Handlers;
using Incident.Domain.Entities;
using Incident.Domain.Interfaces;
using NSubstitute;

namespace Incident.UnitTests.Application.Handlers;

public class LostObjectHandlerTests
{
    private readonly ILostObjectRepository _repository;
    private readonly LostObjectHandler _handler;

    public LostObjectHandlerTests()
    {
        _repository = Substitute.For<ILostObjectRepository>();
        _handler = new LostObjectHandler(_repository);
    }

    private static CreateLostObjectRequest ValidRequest(
        string photoUrl = "https://campus.uct.cl/foto.png",
        Guid? structureId = null) =>
        new(
            Title: "Mochila negra",
            Description: "Mochila con libros dejada en la biblioteca",
            Status: (int)Objectenum.Pending,
            PhotoUrl: photoUrl,
            StructureId: structureId ?? Guid.NewGuid());

    // ---------- CreateObjectAsync ----------

    [Fact]
    public async Task CreateObjectAsync_ConPhotoUrlNoEsUrl_RetornaValidation()
    {
        var result = await _handler.CreateObjectAsync(ValidRequest(photoUrl: "no-es-una-url"));

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.Errors[0].Type);
        Assert.Equal("LostObject.PhotoUrl", result.Errors[0].Code);
        await _repository.DidNotReceiveWithAnyArgs().CreateAsync(default!);
    }

    [Theory]
    [InlineData("ftp://campus.uct.cl/foto.png")]
    [InlineData("javascript:alert(1)")]
    [InlineData("/ruta/relativa/foto.png")]
    public async Task CreateObjectAsync_ConPhotoUrlDeSchemeInvalido_RetornaValidation(string photoUrl)
    {
        var result = await _handler.CreateObjectAsync(ValidRequest(photoUrl: photoUrl));

        Assert.True(result.IsError);
        Assert.Equal("LostObject.PhotoUrl", result.Errors[0].Code);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task CreateObjectAsync_ConPhotoUrlVacia_NoValidaElFormato(string? photoUrl)
    {
        var result = await _handler.CreateObjectAsync(ValidRequest(photoUrl: photoUrl!));

        Assert.False(result.IsError);
        await _repository.Received(1).CreateAsync(Arg.Any<LostObject>());
    }

    [Fact]
    public async Task CreateObjectAsync_ConStructureIdVacio_RetornaValidation()
    {
        var result = await _handler.CreateObjectAsync(ValidRequest(structureId: Guid.Empty));

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.Errors[0].Type);
        Assert.Equal("LostObject.StructureId", result.Errors[0].Code);
        await _repository.DidNotReceiveWithAnyArgs().CreateAsync(default!);
    }

    [Fact]
    public async Task CreateObjectAsync_ConRequestValido_MapeaElEntityYLoPersiste()
    {
        var structureId = Guid.NewGuid();
        var request = new CreateLostObjectRequest(
            Title: "Paraguas azul",
            Description: "Paraguas en el pasillo del piso 2",
            Status: (int)Objectenum.Resolved,
            PhotoUrl: "https://campus.uct.cl/paraguas.jpg",
            StructureId: structureId);

        var result = await _handler.CreateObjectAsync(request);

        Assert.False(result.IsError);
        await _repository.Received(1).CreateAsync(Arg.Is<LostObject>(lo =>
            lo.Title == "Paraguas azul" &&
            lo.Description == "Paraguas en el pasillo del piso 2" &&
            lo.Status == Objectenum.Resolved &&
            lo.PhotoUrl == "https://campus.uct.cl/paraguas.jpg" &&
            lo.StructureRefId == structureId));
    }

    [Fact]
    public async Task CreateObjectAsync_CuandoElRepositorioNoSeteaElId_RetornaCadenaVacia()
    {
        // El Id lo asigna Mongo al insertar; el handler devuelve string.Empty en ese caso.
        var result = await _handler.CreateObjectAsync(ValidRequest());

        Assert.False(result.IsError);
        Assert.Equal(string.Empty, result.Value);
    }

    // ---------- GetByIdAsync ----------

    [Fact]
    public async Task GetByIdAsync_CuandoNoExiste_RetornaNotFound()
    {
        _repository.GetByIdAsync("64b000000000000000000000")
            .Returns(Task.FromResult<LostObject?>(null));

        var result = await _handler.GetByIdAsync("64b000000000000000000000");

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.NotFound, result.Errors[0].Type);
        Assert.Equal("LostObject.NotFound", result.Errors[0].Code);
    }

    [Fact]
    public async Task GetByIdAsync_CuandoExiste_MapeaTodosLosCampos()
    {
        var structureId = Guid.NewGuid();
        var entity = new LostObject
        {
            Id = "64b000000000000000000001",
            Title = "Audifonos",
            Description = "Audifonos inalambricos",
            Status = Objectenum.In_Process,
            PhotoUrl = "https://campus.uct.cl/audifonos.jpg",
            StructureRefId = structureId
        };
        _repository.GetByIdAsync(entity.Id).Returns(Task.FromResult<LostObject?>(entity));

        var result = await _handler.GetByIdAsync(entity.Id);

        Assert.False(result.IsError);
        Assert.Equal(new LostObjectResponse(
            "Audifonos",
            "Audifonos inalambricos",
            nameof(Objectenum.In_Process),
            "https://campus.uct.cl/audifonos.jpg",
            structureId), result.Value);
    }

    // ---------- FilterStatusAsync ----------

    [Fact]
    public async Task FilterStatusAsync_ConStatusFueraDelEnum_RetornaValidation()
    {
        var result = await _handler.FilterStatusAsync(99);

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.Errors[0].Type);
        Assert.Equal("LostObject.Validation", result.Errors[0].Code);
        await _repository.DidNotReceiveWithAnyArgs().FilterStatusAsync(default!);
    }

    [Fact]
    public async Task FilterStatusAsync_ConStatusValido_FiltraPorElNombreDelEnum()
    {
        _repository.FilterStatusAsync(nameof(Objectenum.Resolved))
            .Returns(Task.FromResult(new List<LostObject>
            {
                new()
                {
                    Id = "64b000000000000000000002",
                    Title = "Casco",
                    Description = "Casco de bicicleta",
                    Status = Objectenum.Resolved,
                    PhotoUrl = "https://campus.uct.cl/casco.jpg",
                    StructureRefId = Guid.NewGuid()
                },
                new()
                {
                    Id = "64b000000000000000000003",
                    Title = "Llavero",
                    Description = "Llavero con llaves",
                    Status = Objectenum.Resolved,
                    PhotoUrl = null,
                    StructureRefId = Guid.NewGuid()
                }
            }));

        var result = await _handler.FilterStatusAsync((int)Objectenum.Resolved);

        Assert.False(result.IsError);
        await _repository.Received(1).FilterStatusAsync(nameof(Objectenum.Resolved));
        Assert.Equal(2, result.Value.TotalCount);
        Assert.Equal(2, result.Value.Items.Count);
        Assert.All(result.Value.Items, i => Assert.Equal(nameof(Objectenum.Resolved), i.Status));
        Assert.Equal("Casco", result.Value.Items[0].Title);
        Assert.Equal("Llavero", result.Value.Items[1].Title);
    }

    [Fact]
    public async Task FilterStatusAsync_CuandoNoHayResultados_RetornaListaVacia()
    {
        _repository.FilterStatusAsync(nameof(Objectenum.Canceled))
            .Returns(Task.FromResult(new List<LostObject>()));

        var result = await _handler.FilterStatusAsync((int)Objectenum.Canceled);

        Assert.False(result.IsError);
        Assert.Equal(0, result.Value.TotalCount);
        Assert.Empty(result.Value.Items);
    }

    // ---------- UpdateStatusAsync ----------

    [Fact]
    public async Task UpdateStatusAsync_ConStatusFueraDelEnum_RetornaValidation()
    {
        var result = await _handler.UpdateStatusAsync("64b000000000000000000000", new UpdateStatus(42));

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.Errors[0].Type);
        Assert.Equal("LostObject.Validation", result.Errors[0].Code);
    }

    [Fact]
    public async Task UpdateStatusAsync_CuandoElObjetoNoExiste_RetornaNotFound()
    {
        _repository.GetByIdAsync("64b000000000000000000000")
            .Returns(Task.FromResult<LostObject?>(null));

        var result = await _handler.UpdateStatusAsync("64b000000000000000000000", new UpdateStatus((int)Objectenum.Resolved));

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.NotFound, result.Errors[0].Type);
        Assert.Equal("LostObject.NotFound", result.Errors[0].Code);
    }

    [Fact]
    public async Task UpdateStatusAsync_CuandoElRepositorioFalla_RetornaFailure()
    {
        var entity = new LostObject
        {
            Id = "64b000000000000000000000",
            Title = "Mochila",
            Description = "Mochila",
            Status = Objectenum.Pending,
            StructureRefId = Guid.NewGuid()
        };
        _repository.GetByIdAsync(entity.Id).Returns(Task.FromResult<LostObject?>(entity));
        _repository.UpdateStatusAsync(entity, (int)Objectenum.Resolved).Returns(Task.FromResult(false));

        var result = await _handler.UpdateStatusAsync(entity.Id!, new UpdateStatus((int)Objectenum.Resolved));

        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Failure, result.Errors[0].Type);
        Assert.Equal("LostObject.UpdateFailed", result.Errors[0].Code);
    }

    [Fact]
    public async Task UpdateStatusAsync_CuandoExiste_RetornaSuccess()
    {
        var entity = new LostObject
        {
            Id = "64b000000000000000000000",
            Title = "Mochila",
            Description = "Mochila",
            Status = Objectenum.Pending,
            StructureRefId = Guid.NewGuid()
        };
        _repository.GetByIdAsync(entity.Id).Returns(Task.FromResult<LostObject?>(entity));
        _repository.UpdateStatusAsync(entity, (int)Objectenum.Canceled).Returns(Task.FromResult(true));

        var result = await _handler.UpdateStatusAsync(entity.Id!, new UpdateStatus((int)Objectenum.Canceled));

        Assert.False(result.IsError);
        Assert.Equal(Result.Success, result.Value);
        await _repository.Received(1).UpdateStatusAsync(entity, (int)Objectenum.Canceled);
    }
}
