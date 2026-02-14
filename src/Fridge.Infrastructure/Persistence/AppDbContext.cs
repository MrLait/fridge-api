using Fridge.Application.Common.Interfaces;
using Fridge.Domain.Entities;
using Fridge.Infrastructure.Persistence.SpModels;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
    {
        public DbSet<Domain.Entities.Fridge> Fridges => Set<Domain.Entities.Fridge>();
        public DbSet<FridgeModel> FridgeModels => Set<FridgeModel>();
        public DbSet<FridgeProduct> FridgeProducts => Set<FridgeProduct>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<RestockCandidateRow> RestockCandidateRows => Set<RestockCandidateRow>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

    }
}