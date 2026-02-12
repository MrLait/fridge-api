using Fridge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Infrastructure.Persistence;

public sealed class DbSeeder(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await _db.Database.MigrateAsync(cancellationToken);

        if (!await _db.FridgeModels.AnyAsync(cancellationToken))
        {
            var models = new[]
            {
                new FridgeModel { Id = Guid.NewGuid(), Name = "Abios Model 1", Year = 2020 },
                new FridgeModel { Id = Guid.NewGuid(), Name = "Amender Model 2", Year = 2021},
                new FridgeModel { Id = Guid.NewGuid(), Name = "Fridge Model 3", Year = 2022 },
                new FridgeModel { Id = Guid.NewGuid(), Name = "Fridge Model 4", Year = 2023},
                new FridgeModel { Id = Guid.NewGuid(), Name = "Fridge Model 5", Year = 2024 }
            };

            _db.FridgeModels.AddRange(models);
            await _db.SaveChangesAsync(cancellationToken);
        }

        if (!await _db.Products.AnyAsync(cancellationToken))
        {
            var products = new[]
            {
                new Product { Id = Guid.NewGuid(), Name = "Milk", DefaultQuantity = 2 },
                new Product { Id = Guid.NewGuid(), Name = "Eggs", DefaultQuantity = 10 },
                new Product { Id = Guid.NewGuid(), Name = "Cheese", DefaultQuantity = 1 },
                new Product { Id = Guid.NewGuid(), Name = "Apples", DefaultQuantity = 6 },
                new Product { Id = Guid.NewGuid(), Name = "Water", DefaultQuantity = 12 }
            };

            _db.Products.AddRange(products);
            await _db.SaveChangesAsync(cancellationToken);
        }

        if (!await _db.Fridges.AnyAsync(cancellationToken))
        {
            var modelIds = await _db.FridgeModels.Select(x => x.Id).ToListAsync(cancellationToken);

            if (modelIds.Count == 0) throw new InvalidOperationException("No fridge models found");

            var fridges = new[]
            {
                new Domain.Entities.Fridge { Id = new Guid("1549EEC3-E01B-4A17-B6E4-3726B90EF3CD"), Name = "Fridge 1", OwnerName = "John Doe1", ModelId = modelIds[0]},
                new Domain.Entities.Fridge { Id = Guid.NewGuid(), Name = "Fridge 2", OwnerName = "John Doe2", ModelId = modelIds[1]},
                new Domain.Entities.Fridge { Id = Guid.NewGuid(), Name = "Fridge 3", OwnerName = "John Doe3", ModelId = modelIds[2]},
                new Domain.Entities.Fridge { Id = Guid.NewGuid(), Name = "Fridge 4", OwnerName = "John Doe4", ModelId = modelIds[3]},
                new Domain.Entities.Fridge { Id = Guid.NewGuid(), Name = "Fridge 5", OwnerName = "John Doe1", ModelId = modelIds[4]},
                new Domain.Entities.Fridge { Id = Guid.NewGuid(), Name = "Fridge 6", OwnerName = "John Doe2", ModelId = modelIds[0]},
                new Domain.Entities.Fridge { Id = Guid.NewGuid(), Name = "Fridge 7", OwnerName = "John Doe3", ModelId = modelIds[1]},
                new Domain.Entities.Fridge { Id = Guid.NewGuid(), Name = "Fridge 8", OwnerName = "John Doe4", ModelId = modelIds[2]},
                new Domain.Entities.Fridge { Id = Guid.NewGuid(), Name = "Fridge 9", OwnerName = "John Doe5", ModelId = modelIds[3]},
                new Domain.Entities.Fridge { Id = Guid.NewGuid(), Name = "Fridge 10", OwnerName = "John Doe6", ModelId = modelIds[4]},
            };

            _db.Fridges.AddRange(fridges);
            await _db.SaveChangesAsync(cancellationToken);
        }

        if (!await _db.FridgeProducts.AnyAsync(cancellationToken))
        {
            var fridgeIds = await _db.Fridges.Select(x => x.Id).ToListAsync(cancellationToken);
            var productIds = await _db.Products.AsNoTracking().ToListAsync(cancellationToken);

            if (fridgeIds.Count == 0 || productIds.Count == 0) throw new InvalidOperationException("No fridge or product found");

            var fridgeProducts = new[]
            {
                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[0], ProductId = productIds[0].Id, Quantity = 2},
                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[0], ProductId = productIds[1].Id, Quantity = 1},
                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[0], ProductId = productIds[2].Id, Quantity = 5},
                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[0], ProductId = productIds[3].Id, Quantity = 7},
                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[0], ProductId = productIds[4].Id, Quantity = 3},

                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[1], ProductId = productIds[0].Id, Quantity = 50},
                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[1], ProductId = productIds[1].Id, Quantity = 2},
                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[1], ProductId = productIds[2].Id, Quantity = 4},
                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[1], ProductId = productIds[3].Id, Quantity = 9},
                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[1], ProductId = productIds[4].Id, Quantity = 1},

                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[2], ProductId = productIds[0].Id, Quantity = 7},
                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[2], ProductId = productIds[1].Id, Quantity = 3},
                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[2], ProductId = productIds[2].Id, Quantity = 1},


                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[3], ProductId = productIds[0].Id, Quantity = 0},
                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[3], ProductId = productIds[1].Id, Quantity = 1},
                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[3], ProductId = productIds[2].Id, Quantity = 0},
                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[3], ProductId = productIds[3].Id, Quantity = 0},
                new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridgeIds[3], ProductId = productIds[4].Id, Quantity = 0}
            };

            _db.FridgeProducts.AddRange(fridgeProducts);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}