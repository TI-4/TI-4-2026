using Incident.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // opcional, para OpenAPI futuro

var app = builder.Build();

app.MapControllers();

app.Run();

public partial class Program
{
}