using Fridge.Application.Features.Fridges.Dtos;
using MediatR;

namespace Fridge.Application.Features.Fridges.Queries.GetFridgeProducts;
public sealed record GetFridgeProductsQuery(Guid FridgeId) : IRequest<IReadOnlyList<FridgeProductDto>>;