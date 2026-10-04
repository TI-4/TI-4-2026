using Campus.Domain.Entities;
using CampusEntity = Campus.Domain.Entities.Campus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Campus.Infrastructure.Persistence.Configurations;

public class CampusConfiguration : IEntityTypeConfiguration<CampusEntity>
{
    public void Configure(EntityTypeBuilder<CampusEntity> builder)
    {
        builder.HasMany(c => c.Buildings)
               .WithOne(b => b.Campus)
               .HasForeignKey(b => b.CampusId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Structures)
               .WithOne(s => s.Campus)
               .HasForeignKey(s => s.CampusId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(c => c.Coordinates);
    }
}
