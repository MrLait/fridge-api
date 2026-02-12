namespace Fridge.Api.Contracts.Fridges;

public sealed record InitialProductRequest(Guid ProductId, int Quantity);

public sealed record CreateFridgeRequest
(
    string Name,
    string? OwnerName,
    Guid ModelId,
    List<InitialProductRequest>? InitialProducts
);
