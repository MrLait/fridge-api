namespace Fridge.Application.Features.Fridges.Commands.CreateFridge;

public sealed record CreateFridgeRequest
(
    string Name,
    string? OwnerName,
    Guid ModelId
);
