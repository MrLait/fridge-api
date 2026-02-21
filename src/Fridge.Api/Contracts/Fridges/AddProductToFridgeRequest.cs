namespace Fridge.Api.Contracts.Fridges;

public sealed record AddProductToFridgeRequest(Guid ProductId, int Quantity);