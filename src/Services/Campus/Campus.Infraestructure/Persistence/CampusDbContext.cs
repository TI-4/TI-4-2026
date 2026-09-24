using Microsoft.EntityFrameworkCore;
using Campus.Domain.Entities;
using Campus.Domain.ValueObjects;
using CampusEntity = Campus.Domain.Entities.Campus;

namespace Campus.Infraestructure.Persistence
{
    public class CampusDbContext : DbContext
    {
        public CampusDbContext(DbContextOptions<CampusDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CampusEntity>()
                .HasMany(c => c.Buildings)
                .WithOne(b => b.Campus)
                .HasForeignKey(b => b.CampusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CampusEntity>()
                .HasMany(c => c.Structures)
                .WithOne(s => s.Campus)
                .HasForeignKey(s => s.CampusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CampusEntity>()
                .OwnsOne(c => c.Coordinates);


            modelBuilder.Entity<Building>()
                .HasMany(b => b.Rooms)
                .WithOne(r => r.Building)
                .HasForeignKey(r => r.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Building>()
                .OwnsOne(b => b.Coordinates);


            modelBuilder.Entity<Category>()
                .HasMany(c => c.Structures)
                .WithOne(s => s.Category)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Rooms)
                .WithOne(r => r.Category)
                .HasForeignKey(r => r.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Structure>()
                .OwnsOne(s => s.Coordinates);
        }

        public DbSet<CampusEntity> Campuses { get; set; }
            public DbSet<Building> Buildings { get; set; }
            public DbSet<Room> Rooms { get; set; }
            public DbSet<Structure> Structures { get; set; }
            public DbSet<Category> Categories { get; set; }
        
    }
}