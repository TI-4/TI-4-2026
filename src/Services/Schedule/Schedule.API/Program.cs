using Schedule.Infrastructure;
using Schedule.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new
{
    service = "schedule-service",
    status = "healthy"
}));

app.MapGet("/health/db", async (ScheduleDbContext dbContext) =>
{
    var canConnect = await dbContext.Database.CanConnectAsync();

    if (!canConnect)
    {
        return Results.Problem(
            title: "No se pudo conectar a la base de datos.",
            statusCode: StatusCodes.Status503ServiceUnavailable);
    }

    return Results.Ok(new
    {
        service = "schedule-service",
        database = "postgresql",
        status = "healthy"
    });
});

app.Run();

public partial class Program
{
}
