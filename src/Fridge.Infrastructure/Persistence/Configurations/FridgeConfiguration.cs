using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fridge.Infrastructure.Persistence.Configurations
{
    public class FridgeConfiguration : IEntityTypeConfiguration<Domain.Entities.Fridge>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Fridge> e)
        {
            e.ToTable("fridge");

            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();

            e.Property(x => x.Name).HasColumnName("name").IsRequired();
            e.Property(x => x.OwnerName).HasColumnName("owner_name");

            e.Property(x => x.ModelId).HasColumnName("model_id").IsRequired();

            e.HasOne(x => x.Model)
            .WithMany(m => m.Fridges)
            .HasForeignKey(f => f.ModelId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}