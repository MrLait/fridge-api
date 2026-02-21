namespace Fridge.Api.Contracts.Products;

public sealed record UpdateProductRequest
(
    string Name,
    int? DefaultQuantity
);