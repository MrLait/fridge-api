using Fridge.Infrastructure.Persistence.SpModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fridge.Infrastructure.Persistence.Configurations.StoredProcedures;

public class RestockCandidateRowConfiguration : IEntityTypeConfiguration<RestockCandidateRow>
{
    public void Configure(EntityTypeBuilder<RestockCandidateRow> builder)
    {
        builder.HasNoKey();
        builder.Property(x => x.FridgeProductId).HasColumnName("fridge_product_id");
        builder.Property(x => x.FridgeId).HasColumnName("fridge_id");
        builder.Property(x => x.ProductId).HasColumnName("product_id");
        builder.Property(x => x.DefaultQuantity).HasColumnName("default_quantity");
    }
}