using Incident.Application.Services;
using Incident.Domain.Repositories;
using Incident.Infrastructure.Persistence;
using Incident.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Incident.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Leer los nombres exactos de tu docker-compose.yml (reemplazando __ por :)
        var connectionString = configuration["MongoDB:ConnectionString"]
                               ?? "mongodb://root:rootpassword@mongodb:27017/?authSource=admin";

        var databaseName = configuration["MongoDB:DatabaseName"]
                           ?? "uct_incident_db";

        // 2. Registrar el Cliente de Mongo
        services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

        // 3. Registrar la Base de Datos (Crucial para que no de error el repositorio)
        services.AddScoped<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(databaseName);
        });

        // 4. Registrar tu Contexto
        services.AddScoped<IMongoDbContext>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return new MongoDbContext(client, databaseName);
        });

        // 5. Registrar Repositorios y Servicios
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<ITicketService, TicketService>();

        return services;
    }
}
