namespace Fridge.Application.Features.Products.Dtos;

public sealed record ProductListDto(
    Guid Id,
    string Name,
    int? DefaultQuantity,
    string? PrimaryImageUrl
);