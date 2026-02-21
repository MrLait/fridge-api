namespace Fridge.Api.Contracts.Fridges;

public sealed record UpdateFridgeRequest
(
    string Name,
    string? OwnerName,
    Guid ModelId
);