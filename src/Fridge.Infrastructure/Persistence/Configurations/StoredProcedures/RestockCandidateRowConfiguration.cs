using Fridge.Infrastructure.Persistence.SpModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fridge.Infrastructure.Persistence.Configurations.StoredProcedures;

public class RestockCandidateRowConfiguration : IEntityTypeConfiguration<RestockCandidateRow>
{
    public void Configure(EntityTypeBuilder<RestockCandidateRow> builder)
    {
        builder.HasNoKey();
    }
}