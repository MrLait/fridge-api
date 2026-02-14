using Fridge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fridge.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> e)
        {
            e.ToTable("products");

            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();

            e.Property(x => x.Name).HasColumnName("name").IsRequired();
            e.Property(x => x.DefaultQuantity).HasColumnName("default_quantity");
        }
    }
}