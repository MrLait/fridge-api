
using Fridge.Application.Features.Fridges.Dtos;
using MediatR;

namespace Fridge.Application.Features.Fridges.Queries.GetFridgeById;

public sealed record GetFridgeByIdQuery(Guid Id) : IRequest<FridgeDto>;