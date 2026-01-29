
using Fridge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Domain.Entities.Fridge> Fridges { get; }
    DbSet<FridgeModel> FridgeModels { get; }
    DbSet<FridgeProduct> FridgeProducts { get; }
    DbSet<Product> Products { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}