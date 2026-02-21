namespace Fridge.Application.Features.Products.Dtos;

public sealed record ProductDto
(
    Guid Id,
    string Name,
    int? DefaultQuantity
);