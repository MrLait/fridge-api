using Fridge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fridge.Infrastructure.Persistence.Configurations;

public sealed class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> e)
    {
        e.ToTable("product_images");

        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();

        e.Property(x => x.StorageKey).HasColumnName("storage_key").IsRequired().HasMaxLength(500);
        e.Property(x => x.FileName).HasColumnName("file_name").IsRequired().HasMaxLength(255);
        e.Property(x => x.ContentType).HasColumnName("content_type").IsRequired().HasMaxLength(100);
        e.Property(x => x.Size).HasColumnName("size").IsRequired();
        e.Property(x => x.IsPrimary).HasColumnName("is_primary").IsRequired();
        e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        e.Property(x => x.ProductId).HasColumnName("product_id").IsRequired();
        e.HasOne(x => x.Product)
        .WithMany(x => x.Images)
        .HasForeignKey(x => x.ProductId)
        .OnDelete(DeleteBehavior.Cascade);

        e.HasIndex(x => x.ProductId);
        e.HasIndex(x => new { x.ProductId, x.IsPrimary });

        e.HasIndex(x => x.ProductId)
        .IsUnique()
        .HasFilter("[is_primary] = 1")
        .HasDatabaseName("UX_product_images_product_id_primary");
    }
}