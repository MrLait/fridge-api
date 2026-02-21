using Fridge.Application.Features.FridgeModels.Dtos;
using MediatR;

namespace Fridge.Application.Features.FridgeModels.Queries.GetFridgeModels;

public sealed record GetFridgeModelsQuery
    : IRequest<IReadOnlyList<FridgeModelDto>>;