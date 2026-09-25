using Microsoft.EntityFrameworkCore;
using Campus.Infraestructure.Persistence;
using Campus.Domain.Interfaces;
using Campus.Application.UseCases;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<CampusDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("CampusDb")));

// DI Registrations
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<CampusHandler>();
builder.Services.AddScoped<BuildingHandler>();
builder.Services.AddScoped<CategoryHandler>();
builder.Services.AddScoped<RoomHandler>();
builder.Services.AddScoped<StructureHandler>();


var app = builder.Build();

app.MapControllers();

app.Run();
