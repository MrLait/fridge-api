namespace Fridge.Application.Common.Interfaces;

public interface IFridgeProductService
{
    Task<Guid> AddProductAsync(Guid fridgeId, Guid productId, int quantity, CancellationToken ct);
}