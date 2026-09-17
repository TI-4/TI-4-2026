using Microsoft.EntityFrameworkCore;

namespace Campus.Infraestructure.Persistence
{
    public class CampusDbContext : DbContext
    {
        public CampusDbContext(DbContextOptions<CampusDbContext> options) : base(options)
        {
        }

        // Aquí agregaremos luego las entidades (CampusEntity, BuildingEntity, etc.)
    }
}
