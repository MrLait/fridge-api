using Fridge.Application.Common.Interfaces;
using Fridge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Infrastructure.Services
{
    public sealed class FridgeProductService(IAppDbContext db) : IFridgeProductService
    {
        public async Task<Guid> AddProductAsync(Guid fridgeId, Guid productId, int quantity, bool saveChanges = true, CancellationToken ct = default)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be grater than 0");

            if (!await db.Fridges.AnyAsync(x => x.Id == fridgeId, ct))
                throw new KeyNotFoundException($"Fridge {fridgeId} not found");

            if (!await db.Products.AnyAsync(x => x.Id == productId, ct))
                throw new KeyNotFoundException($"Product {productId} not found");

            var existing = await db.FridgeProducts.SingleOrDefaultAsync(x => x.ProductId == productId && x.FridgeId == fridgeId, ct);
            Guid id;

            if (existing is null)
            {
                var entity = new FridgeProduct
                {
                    Id = Guid.NewGuid(),
                    FridgeId = fridgeId,
                    ProductId = productId,
                    Quantity = quantity
                };

                db.FridgeProducts.Add(entity);
                id = entity.Id;
            }
            else
            {
                existing.Quantity += quantity;
                id = existing.Id;
            }

            if (saveChanges)
                await db.SaveChangesAsync(ct);

            return id;
        }
    }
}