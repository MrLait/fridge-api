namespace Fridge.Application.Features.Fridges.Commands.UpdateFridge;

public sealed record UpdateFridgeRequest
(
    string Name,
    string? OwnerName,
    Guid ModelId
);