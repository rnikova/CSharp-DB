using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Data.Models;

namespace PetStore.Data.Configuration
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasMany(f => f.Food)
                .WithOne(f => f.Brand)
                .HasForeignKey(f => f.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Toys)
            .WithOne(t => t.Brand)
            .HasForeignKey(t => t.BrandId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
