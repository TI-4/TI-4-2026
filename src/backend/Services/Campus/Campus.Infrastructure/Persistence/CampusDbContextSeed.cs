using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Campus.Domain.Entities;
using Campus.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CampusEntity = Campus.Domain.Entities.Campus;
using StructureEntity = Campus.Domain.Entities.Structure;

namespace Campus.Infrastructure.Persistence;

public static class CampusDbContextSeed
{
    public static async Task SeedAsync(
        CampusDbContext context,
        ILogger? logger = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Database.IsRelational())
        {
            logger?.LogInformation("Aplicando migraciones pendientes de CampusDbContext...");
            await context.Database.MigrateAsync(cancellationToken);
        }

        if (await context.Campuses.AnyAsync(cancellationToken))
        {
            logger?.LogInformation("La base de datos de Campus ya contiene información. Omitiendo seed.");
            return;
        }

        logger?.LogInformation("Iniciando seed de datos iniciales de Campus");

        var campusSanFrancisco = new CampusEntity(
            "Campus San Francisco",
            "Manuel Montt 056, Temuco",
            new Coordinate(-38.7335, -72.6020),
            zoom: 16.0m,
            pitch: 50.0m,
            bearing: -20.0m
        );

        var campusSanJuanPablo = new CampusEntity(
            "Campus San Juan Pablo II",
            "Rudecindo Ortega 02950, Temuco",
            new Coordinate(-38.7118, -72.5855),
            zoom: 16.0m,
            pitch: 50.0m,
            bearing: -20.0m
        );

        await context.Campuses.AddRangeAsync(new[] { campusSanFrancisco, campusSanJuanPablo }, cancellationToken);

        var groupAcademico = new CategoryGroup("Infraestructura Académica");
        var groupEspaciosComunes = new CategoryGroup("Espacios de Estudio y Comunes");

        await context.CategoryGroups.AddRangeAsync(new[] { groupAcademico, groupEspaciosComunes }, cancellationToken);

        var catSalaClases = new Category(
            "Sala de Clases",
            "book",
            "Sala de uso general para clases teóricas y cátedras",
            groupAcademico
        );

        var catLaboratorio = new Category(
            "Laboratorio",
            "computer",
            "Laboratorio de computación, ciencias o docencia especializada",
            groupAcademico
        );

        var catEstudio = new Category(
            "Sala de Estudio",
            "users",
            "Espacio habilitado para estudio individual y grupal",
            groupEspaciosComunes
        );

        await context.Categories.AddRangeAsync(new[] { catSalaClases, catLaboratorio, catEstudio }, cancellationToken);

        var defaultOpenTime = new TimeOnly(8, 0);
        var defaultCloseTime = new TimeOnly(20, 0);
        var defaultServices = "WIFI, BAÑOS, ACCESO UNIVERSAL";

        var edificio16 = new Building("Edificio 16", 3, new Coordinate(-38.7336, -72.6021), campusSanFrancisco, defaultOpenTime, defaultCloseTime, defaultServices);
        var edificio12 = new Building("Edificio 12", 3, new Coordinate(-38.7332, -72.6015), campusSanFrancisco, defaultOpenTime, defaultCloseTime, defaultServices);

        var edificio8 = new Building("Edificio 8", 3, new Coordinate(-38.7115, -72.5850), campusSanJuanPablo, defaultOpenTime, defaultCloseTime, defaultServices);
        var edificio7 = new Building("Edificio 7", 3, new Coordinate(-38.7120, -72.5860), campusSanJuanPablo, defaultOpenTime, defaultCloseTime, defaultServices);

        await context.Buildings.AddRangeAsync(new[] { edificio16, edificio12, edificio8, edificio7 }, cancellationToken);

        var rooms = new List<Room>();

        void CreateRoomsForBuilding(Building building, string prefix)
        {
            rooms.Add(new Room($"Lab {prefix}-LAB", 1, building, catLaboratorio));

            for (int floor = 1; floor <= 3; floor++)
            {
                rooms.Add(new Room($"Sala {prefix}-{floor}01", floor, building, catSalaClases));
                rooms.Add(new Room($"Sala {prefix}-{floor}02", floor, building, catSalaClases));
            }
        }

        CreateRoomsForBuilding(edificio16, "16");
        CreateRoomsForBuilding(edificio12, "12");
        CreateRoomsForBuilding(edificio8, "8");
        CreateRoomsForBuilding(edificio7, "7");

        await context.Rooms.AddRangeAsync(rooms, cancellationToken);

        var structureSF = new StructureEntity(
            "Punto de Estudio San Francisco",
            new Coordinate(-38.7338, -72.6025),
            campusSanFrancisco,
            catEstudio
        );

        var structureSJP = new StructureEntity(
            "Centro de Estudio Juan Pablo II",
            new Coordinate(-38.7119, -72.5852),
            campusSanJuanPablo,
            catEstudio
        );

        await context.Structures.AddRangeAsync(new[] { structureSF, structureSJP }, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
        logger?.LogInformation("Seed de Campus completado exitosamente con 2 campus, 2 grupos, 3 categorías, 4 edificios, 28 salas y 2 estructuras.");
    }
}
