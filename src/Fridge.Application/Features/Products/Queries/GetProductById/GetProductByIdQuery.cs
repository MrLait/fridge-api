using Fridge.Application.Features.Products.Dtos;
using MediatR;

namespace Fridge.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;