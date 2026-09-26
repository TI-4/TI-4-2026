using Microsoft.EntityFrameworkCore;
using Schedule.Infrastructure.Persistence;
using Schedule.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ScheduleDbContext>();
    await context.Database.MigrateAsync();
}

app.MapControllers();

app.Run();

public partial class Program
{
}


