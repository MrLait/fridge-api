using MediatR;
using Fridge.Application.Features.Fridges.Dtos;

namespace Fridge.Application.Features.Fridges.Queries.GetFridges;
public sealed record GetFridgesQuery : IRequest<IReadOnlyList<FridgeDto>>;