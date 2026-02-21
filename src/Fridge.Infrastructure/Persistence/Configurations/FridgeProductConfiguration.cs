using Fridge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fridge.Infrastructure.Persistence.Configurations
{
    public class FridgeProductConfiguration : IEntityTypeConfiguration<FridgeProduct>
    {
        public void Configure(EntityTypeBuilder<FridgeProduct> e)
        {
            e.ToTable("fridge_products");

            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();

            e.Property(x => x.ProductId).HasColumnName("product_id").IsRequired();
            e.Property(x => x.FridgeId).HasColumnName("fridge_id").IsRequired();
            e.Property(x => x.Quantity).HasColumnName("quantity").IsRequired();

            e.HasOne(x => x.Product)
            .WithMany(m => m.FridgeProducts)
            .HasForeignKey(f => f.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Fridge)
            .WithMany(m => m.FridgeProducts)
            .HasForeignKey(f => f.FridgeId)
            .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => new { x.FridgeId, x.ProductId }).IsUnique();
        }
    }
}