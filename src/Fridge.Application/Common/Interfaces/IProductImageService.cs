namespace Fridge.Application.Common.Interfaces;

public interface IProductImageService
{
    Task SetPrimaryAsync(Guid productId, Guid imageId, CancellationToken ct = default);
}
