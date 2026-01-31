namespace Fridge.Application.Features.Fridges.Dtos;
public sealed record FridgeProductDto
(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int Quantity,
    int? ProductDefaultQuantity
);