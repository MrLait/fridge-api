namespace Fridge.Application.Common.Interfaces;

public interface IRestockService
{
    Task<int> RestockZeroQuantityAsync(CancellationToken ct = default);
}