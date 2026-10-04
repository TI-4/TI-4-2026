using Campus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Campus.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasMany(c => c.Structures)
               .WithOne(s => s.Category)
               .HasForeignKey(s => s.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Rooms)
               .WithOne(r => r.Category)
               .HasForeignKey(r => r.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
