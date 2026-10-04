using Campus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Campus.Infrastructure.Persistence.Configurations;

public class BuildingConfiguration : IEntityTypeConfiguration<Building>
{
    public void Configure(EntityTypeBuilder<Building> builder)
    {
        builder.HasMany(b => b.Rooms)
               .WithOne(r => r.Building)
               .HasForeignKey(r => r.BuildingId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(b => b.Coordinates);
    }
}
