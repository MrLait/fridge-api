namespace Fridge.Application.Features.Fridges.Commands.AddProductToFridge;

public sealed record AddProductToFridgeRequest(Guid ProductId, int Quantity);