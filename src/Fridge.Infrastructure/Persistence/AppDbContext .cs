using Fridge.Application.Common.Interfaces;
using Fridge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
    {
        public DbSet<Domain.Entities.Fridge> Fridges => Set<Domain.Entities.Fridge>();
        public DbSet<FridgeModel> FridgeModels => Set<FridgeModel>();
        public DbSet<FridgeProduct> FridgeProducts => Set<FridgeProduct>();
        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

    }
}