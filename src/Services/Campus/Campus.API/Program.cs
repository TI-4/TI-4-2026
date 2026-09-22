using Microsoft.EntityFrameworkCore;
using Campus.Infraestructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<CampusDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("CampusDb")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CampusDbContext>();
    dbContext.Database.Migrate();
}

app.MapControllers();

app.Run();