using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Data.Models;

namespace PetStore.Data.Configuration
{
    public class FoodOrderConfiguration : IEntityTypeConfiguration<FoodOrder>
    {
        public void Configure(EntityTypeBuilder<FoodOrder> builder)
        {
            builder.HasKey(fo => new { fo.OrderId, fo.FoodId });

            builder.HasOne(fo => fo.Food)
                .WithMany(fo => fo.Orders)
                .HasForeignKey(fo => fo.FoodId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(fo => fo.Order)
                .WithMany(fo => fo.Food)
                .HasForeignKey(fo => fo.FoodId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
