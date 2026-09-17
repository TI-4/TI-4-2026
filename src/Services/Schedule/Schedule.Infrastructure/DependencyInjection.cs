using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Schedule.Infrastructure.Persistence;

namespace Schedule.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ScheduleDb")
            ?? throw new InvalidOperationException(
                "No se encontro la cadena de conexion 'ScheduleDb'.");

        services.AddDbContext<ScheduleDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Schedule")));

        return services;
    }
}
