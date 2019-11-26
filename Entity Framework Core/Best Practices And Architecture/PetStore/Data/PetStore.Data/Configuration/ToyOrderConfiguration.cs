using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Data.Models;

namespace PetStore.Data.Configuration
{
    public class ToyOrderConfiguration : IEntityTypeConfiguration<ToyOrder>
    {
        public void Configure(EntityTypeBuilder<ToyOrder> builder)
        {
            builder.HasKey(to => new { to.OrderId, to.ToyId });

            builder.HasOne(to => to.Toy)
                .WithMany(to => to.Orders)
                .HasForeignKey(to => to.ToyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(to => to.Order)
                .WithMany(to => to.Toys)
                .HasForeignKey(to => to.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
