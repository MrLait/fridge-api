using Fridge.Application.Features.Products.Dtos;
using MediatR;

namespace Fridge.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    int? DefaultQuantity
) : IRequest<ProductDto>;