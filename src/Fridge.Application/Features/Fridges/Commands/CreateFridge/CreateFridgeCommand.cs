using MediatR;

namespace Fridge.Application.Features.Fridges.Commands.CreateFridge;

public sealed record CreateFridgeCommand(string Name, string? OwnerName, Guid ModelId)
    : IRequest<Guid>;