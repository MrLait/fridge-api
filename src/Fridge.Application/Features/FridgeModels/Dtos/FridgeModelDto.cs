namespace Fridge.Application.Features.FridgeModels.Dtos;

public sealed record FridgeModelDto
(
        Guid Id,
        string Name,
        int? Year
);