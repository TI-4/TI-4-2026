using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Campus.Domain.Entities;
using Campus.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using CampusEntity = Campus.Domain.Entities.Campus;
using StructureEntity = Campus.Domain.Entities.Structure;

namespace Campus.Infrastructure.Persistence
{
    public static class CampusDbContextSeed
    {
        public static async Task SeedAsync(CampusDbContext context)
        {
            if (context.Database.IsRelational())
            {
                await context.Database.MigrateAsync();
            }

            if (await context.Campuses.AnyAsync())
            {
                return;
            }

            var campusSanFrancisco = new CampusEntity(
                "Campus San Francisco", 
                "Manuel Montt 056, Temuco", 
                new Coordinate(-38.7335, -72.6020)
            );
            
            var campusSanJuanPablo = new CampusEntity(
                "Campus San Juan Pablo II", 
                "Rudecindo Ortega 02950, Temuco", 
                new Coordinate(-38.7118, -72.5855)
            );

            await context.Campuses.AddRangeAsync(campusSanFrancisco, campusSanJuanPablo);

            var catSalaClases = new Category("Sala de Clases", "book", "Sala de uso general para clases teóricas");
            var catLaboratorio = new Category("Laboratorio", "computer", "Laboratorio de computación o ciencias");
            var catEstudio = new Category("Sala de Estudio", "users", "Espacio para estudio grupal o individual");

            await context.Categories.AddRangeAsync(catSalaClases, catLaboratorio, catEstudio);

            var edificio16 = new Building("Edificio 16", 3, new Coordinate(-38.7336, -72.6021), campusSanFrancisco);
            var edificio12 = new Building("Edificio 12", 3, new Coordinate(-38.7332, -72.6015), campusSanFrancisco);

            var edificio8 = new Building("Edificio 8", 3, new Coordinate(-38.7115, -72.5850), campusSanJuanPablo);
            var edificio7 = new Building("Edificio 7", 3, new Coordinate(-38.7120, -72.5860), campusSanJuanPablo);

            await context.Buildings.AddRangeAsync(edificio16, edificio12, edificio8, edificio7);

            var rooms = new List<Room>();

            void CreateRoomsForBuilding(Building building, string prefix)
            {
                rooms.Add(new Room($"Laboratorio {prefix}-LAB", 1, $"{prefix}-LAB", building, catLaboratorio));

                for (int floor = 1; floor <= 3; floor++)
                {
                    rooms.Add(new Room($"Sala {prefix}-{floor}01", floor, $"{prefix}-{floor}01", building, catSalaClases));
                    rooms.Add(new Room($"Sala {prefix}-{floor}02", floor, $"{prefix}-{floor}02", building, catSalaClases));
                }
            }

            CreateRoomsForBuilding(edificio16, "16");
            CreateRoomsForBuilding(edificio12, "12");
            CreateRoomsForBuilding(edificio8, "8");
            CreateRoomsForBuilding(edificio7, "7");

            await context.Rooms.AddRangeAsync(rooms);

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

            await context.Structures.AddRangeAsync(structureSF, structureSJP);

            await context.SaveChangesAsync();
        }
    }
}
