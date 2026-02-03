namespace Fridge.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdateProductRequest
(
    string Name,
    int? DefaultQuantity
);